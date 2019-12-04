using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
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
    [Group("guild")]
    [RequireGuild]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing guild members and settings")]
    public class GuildCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public GuildCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list-members")]
        [RequireInciteRole(RoleKind.Member)]
        [Description("Lists the registered guild members")]
        public async Task List(CommandContext context)
        {
            StringBuilder memberList = new StringBuilder("UserId | MemberId | Discord Name | Character Name\n");

            foreach (var member in Guild.Members)
            {
                string discordName = context.Guild.Members.ContainsKey(member.User.DiscordId) ?
                    context.Guild.Members[member.User.DiscordId].DisplayName :
                    "(Unknown)";

                memberList.AppendLine($"{member.UserId} | {member.Id} | {discordName} | {member.PrimaryWowCharacter}");
            }

            var interactivity = context.Client.GetInteractivity();
            var pages = interactivity.GeneratePagesInContent(memberList.ToString(), SplitType.Line);
            await interactivity.SendPaginatedMessageAsync(context.Channel, context.User, pages, timeoutoverride: TimeSpan.FromMinutes(5));
        }

        [Command("list-profession")]
        [Aliases("list-prof")]
        [RequireGuildConfigured]
        [Description("Lists the users which are the specified profession")]
        public Task List(CommandContext context,
            [Description(Descriptions.WowProfession)] WowProfession profession)
        {
            var characters = Guild.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowProfessionId == profession.Id));

            StringBuilder message = new StringBuilder($"__**{profession}**__\n");
            foreach (var character in characters)
            {
                message.Append($"{character}\n");
            }

            ResponseString = message.ToString();

            return Task.CompletedTask;
        }

        [Command("list-recipe")]
        [RequireGuildConfigured]
        [Description("Lists the users which know the specified recipe")]
        public Task List(CommandContext context,
            [Description(Descriptions.WowItemRecipe)] [RemainingText] WowItemRecipe recipe)
        {
            var characters = Guild.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowCharacterRecipes
                        .Any(x => x.RecipeId == recipe.Recipe.Id)));

            StringBuilder message = new StringBuilder($"__**{recipe.Recipe}**__\n");
            foreach (var character in characters)
            {
                message.AppendLine($"{character}");
            }

            ResponseString = message.ToString();
            return Task.CompletedTask;
        }

        [Group("admin")]
        [RequireGuild]
        [RequireInciteRole(RoleKind.Officer)]
        [ModuleLifespan(ModuleLifespan.Transient)]
        [Description("Commands for managing guild settings")]
        public class AdminCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public AdminCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("set-server")]
            [RequireInciteRole(RoleKind.Leader)]
            [Description("Sets the WoW server for the guild")]
            public async Task SetRealm(CommandContext context,
                [Description(Descriptions.WowServer)] WowServer server)
            {
                Guild.WowServerId = server.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-faction")]
            [RequireInciteRole(RoleKind.Leader)]
            [Description("Sets the WoW faction for the guild")]
            public async Task SetFaction(CommandContext context,
                [Description(Descriptions.WowFaction)] WowFaction faction)
            {
                Guild.WowFaction = faction;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-role")]
            [RequireInciteRole(RoleKind.Leader)]
            [Description("Sets the server role which corresponds with the RoleKind")]
            public async Task SetRole(CommandContext context,
                [Description("Values: Member, Officer, Leader")] RoleKind roleKind,
                [Description("Name of the corresponding discord role")] DiscordRole discordRole)
            {
                if (roleKind == RoleKind.Everyone)
                {
                    return;
                }

                var existingRole = Guild.Roles
                    .FirstOrDefault(x => x.Kind == roleKind);

                if (existingRole == null)
                {
                    existingRole = new Role()
                    {
                        DiscordId = discordRole.Id,
                        GuildId = Guild.Id,
                        Kind = roleKind
                    };

                    m_dbContext.Roles.Add(existingRole);
                }
                else
                {
                    existingRole.DiscordId = discordRole.Id;
                    m_dbContext.Roles.Update(existingRole);
                }

                await m_dbContext.SaveChangesAsync();

                if (!discordRole.IsMentionable)
                {
                    try
                    {
                        await discordRole.ModifyAsync(mentionable: true);
                    }
                    catch (UnauthorizedException)
                    {
                        ResponseString = "Commands completed, but you need to manually re-order the roles in your serve so that 'Incite Bot' is above any roles you are trying to set here.";
                    }
                }
            }

            [Command("import-members")]
            [RequireInciteRole(RoleKind.Leader)]
            [RequireGuildConfigured]
            [Description("Attempts to import a list of guild characters from a 'Guild Roster Manager' export. Header must be included in export and character names must match Discord names exactly to be imported.")]
            public async Task ImportMembers(CommandContext context,
                [Description("Optional. Defaults to ';'")] char delimeterCharacter = ';')
            {
                var interactivity = context.Client.GetInteractivity();

                await context.Message.RespondAsync("Please export your guild roster from the 'Guild Roster Manager' addon. Copy the export text into a file, and response to this message with the file.");
                var exportFileMessage = await interactivity.WaitForMessageAsync(x => x.Author.Id == context.User.Id && x.Attachments.Count == 1, TimeSpan.FromMinutes(3));
                if (exportFileMessage.TimedOut)
                {
                    ResponseString = "Abandoning wait";
                    return;
                }

                var exportFileUrl = exportFileMessage.Result.Attachments[0].Url;

                var wowClasses = await m_dbContext.WowClasses
                    .ToArrayAsync();

                var guildMembers = await m_dbContext.Members
                    .Include(x => x.User)
                        .ThenInclude(x => x.WowCharacters)
                    .Where(x => x.GuildId == Guild.Id)
                    .ToArrayAsync();

                var memberRole = context.Guild.GetRole(Guild.Roles
                    .First(x => x.Kind == RoleKind.Member).DiscordId);

                var discordMembers = context.Guild.Members;
                int importCount = 0;
                int existingCount = 0;

                try
                {
                    int iName = -1;
                    int iClass = -1;
                    await foreach(var line in ReadExportFile(exportFileUrl))
                    {
                        string[] parts = line.Split(delimeterCharacter);
                        if (iName == -1)
                        {
                            iName = Array.FindIndex(parts, x => x == "Name");
                            iClass = Array.FindIndex(parts, x => x == "Class");
                            if (iName == -1 || iClass == -1)
                            {
                                throw new KeyNotFoundException();
                            }

                            continue;
                        }

                        var charName = parts[iName];
                        var charClass = parts[iClass];
                        var discordMember = discordMembers.Values
                            .FirstOrDefault(x => x.DisplayName.Equals(charName, StringComparison.InvariantCultureIgnoreCase));
                        if (discordMember == null)
                        {
                            continue;
                        }

                        var guildMember = guildMembers
                            .FirstOrDefault(x => x.User.DiscordId == discordMember.Id);
                        if (guildMember == null)
                        {
                            continue;
                        }

                        if (!guildMember.User.WowCharacters
                            .Any(x => x.Name.Equals(charName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            guildMember.User.WowCharacters.Add(new WowCharacter()
                            {
                                GuildId = Guild.Id,
                                Name = charName,
                                UserId = guildMember.UserId,
                                WowFaction = Guild.WowFaction.Value,
                                WowServerId = Guild.WowServerId.Value,
                                WowClassId = wowClasses.First(x => x.Name == charClass).Id,
                            });

                            importCount++;
                        }
                        else
                        {
                            existingCount++;
                        }

                        if (!discordMember.Roles.Contains(memberRole))
                        {
                            try
                            {
                                await discordMember.GrantRoleAsync(memberRole);
                            }
                            catch(UnauthorizedException)
                            {
                            }
                        }
                    }
                }
                catch(Exception)
                {
                    ResponseString = "Failed to read export file. Ensure that the header and both Name and Class are included.";
                    return;
                }

                await m_dbContext.SaveChangesAsync();

                ResponseString = $"Imported: {importCount} characters\nSkipped existing: {existingCount} characters\nTo view your member list, use the '!guild list-characters' command.";
            }

            async IAsyncEnumerable<string> ReadExportFile(string exportFileUrl)
            {
                using HttpClient test = new HttpClient();
                using var response = await test.GetStreamAsync(exportFileUrl);
                using TextReader reader = new StreamReader(response);

                for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
                {
                    yield return line;
                }
            }
        }
    }
}
