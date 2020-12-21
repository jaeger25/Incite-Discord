using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.Helpers;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("epgp")]
    [RequireGuild]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing and viewing EPGP")]
    public class EpgpCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public EpgpCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list-current")]
        [Aliases("list")]
        [RequireInciteRole(RoleKind.Member)]
        [Description("Lists the guild members' EPGP")]
        public async Task ListCurrent(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            var latestSnapshot = await m_dbContext.EpgpSnapshots
                .Include(x => x.Standings)
                    .ThenInclude(x => x.WowCharacter)
                        .ThenInclude(x => x.WowClass)
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync(x => x.GuildId == Guild.Id);

            if (latestSnapshot == null)
            {
                ResponseString = "No EPGP standings imported yet.";
                return;
            }

            var sortedStandings = latestSnapshot.Standings
                .OrderBy(x => x.WowCharacter.WowClassId)
                    .ThenByDescending(x => x.Priority);

            StringBuilder standings = new StringBuilder($"Snapshot - {latestSnapshot.DateTime.ToString("d")}\n");

            string currentClass = "";
            foreach(var standing in sortedStandings)
            {
                if (currentClass != standing.WowCharacter.WowClass.Name)
                {
                    currentClass = standing.WowCharacter.WowClass.Name;
                    standings.AppendLine($"----- **{currentClass}** -----");
                }

                standings.AppendLine($"{standing.WowCharacter.Name}: {standing.EffortPoints} , {standing.GearPoints} , {standing.Priority}");
            }

            var pages = interactivity.GeneratePagesInContent(standings.ToString(), SplitType.Line);
            await interactivity.SendPaginatedMessageAsync(context.Channel, context.User, pages, timeoutoverride: TimeSpan.FromMinutes(3));
        }

        [Command("list-snapshots")]
        [RequireInciteRole(RoleKind.Member)]
        [Description("Lists the guild EPGP snapshots")]
        public async Task ListSnapshots(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            var epgpSnapshots = await m_dbContext.EpgpSnapshots
                .Include(x => x.Standings)
                    .ThenInclude(x => x.WowCharacter)
                        .ThenInclude(x => x.WowClass)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();

            if (epgpSnapshots.Count == 0)
            {
                ResponseString = "No EPGP standings imported yet.";
                return;
            }

            StringBuilder snapshots = new StringBuilder($"ID | Date\n");

            foreach (var epgpSnapshot in epgpSnapshots)
            {
                snapshots.AppendLine($"{epgpSnapshot.Id} | { epgpSnapshot.DateTime}");
            }

            var pages = interactivity.GeneratePagesInContent(snapshots.ToString(), SplitType.Line);
            await interactivity.SendPaginatedMessageAsync(context.Channel, context.User, pages, timeoutoverride: TimeSpan.FromMinutes(3));
        }

        [Command("import")]
        [RequireInciteRole(RoleKind.Officer)]
        [Description("Imports the guild's current EPGP snapshot from a CEPGP addon export")]
        public async Task Import(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            await context.Message.RespondAsync("Please export your guild EPGP from the 'CEPGP' addon. Copy the export text into a file, and respond to this message with the file.");
            var exportFileMessage = await interactivity.WaitForMessageAsync(x => x.Author.Id == context.User.Id && x.Attachments.Count == 1, TimeSpan.FromMinutes(3));
            if (exportFileMessage.TimedOut)
            {
                ResponseString = "Abandoning wait";
                return;
            }

            var exportFileUrl = exportFileMessage.Result.Attachments[0].Url;

            var guildCharacters = await m_dbContext.WowCharacters
                .Where(x => x.GuildId == Guild.Id)
                .ToListAsync();

            var snapshot = new EpgpSnapshot()
            {
                DateTime = DateTimeOffset.UtcNow,
                GuildId = Guild.Id
            };

            HashSet<string> unregisteredCharacters = new HashSet<string>();
            try
            {
                await foreach (var line in FileHelper.ReadExportFileAsync(exportFileUrl))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 7)
                    {
                        throw new InvalidDataException();
                    }

                    string charName = parts[0];
                    int ep = int.Parse(parts[3]);
                    int gp = int.Parse(parts[4]);

                    if (ep > 0 && gp > 1)
                    {
                        var wowChar = guildCharacters
                            .FirstOrDefault(x => x.Name.Equals(charName, StringComparison.OrdinalIgnoreCase));

                        if (wowChar != null)
                        {
                            snapshot.Standings.Add(new EpgpStanding()
                            {
                                EffortPoints = ep,
                                GearPoints = gp,
                                WowCharacterId = wowChar.Id,
                            });
                        }
                        else
                        {
                            unregisteredCharacters.Add(charName);
                        }
                    }
                }
            }
            catch(Exception)
            {
                ResponseString = "Import failed. Ensure all options are checked when exporting data from CEPGP.";
                return;
            }

            m_dbContext.EpgpSnapshots.Add(snapshot);
            await m_dbContext.SaveChangesAsync();

            if (unregisteredCharacters.Count > 0)
            {
                StringBuilder unregistedList = new StringBuilder("**WARNING** - The following characters are not registered. They will not appear in standings and will not be included in backups or exports.\n");

                foreach (var wowChar in unregisteredCharacters)
                {
                    unregistedList.AppendLine(wowChar);
                }

                var pages = interactivity.GeneratePagesInContent(unregistedList.ToString(), SplitType.Line);
                await interactivity.SendPaginatedMessageAsync(context.Channel, context.User, pages, timeoutoverride: TimeSpan.FromMinutes(5));
            }
        }
    }
}
