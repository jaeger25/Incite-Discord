using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("guild")]
    [RequireGuildConfigured]
    [Description("Commands for managing guild settings")]
    public class GuildCommands : BaseCommandModule
    {
        readonly InciteDbContext m_dbContext;

        public GuildCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("set-realm")]
        [RequireInciteRole(RoleKind.Leader)]
        [Description("Sets the WoW realm for the guild")]
        public async Task SetRealm(CommandContext context,
            string realmName)
        {
            var realm = await m_dbContext.WowServers
                .FirstOrDefaultAsync(x => x.Name == realmName);

            if (realm == null)
            {
                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync($"Realm not found: {dmChannel}");
                return;
            }

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            guild.WowServerId = realm.Id;
            await m_dbContext.SaveChangesAsync();
        }

        [Command("list-characters")]
        [Description("Lists the characters in the guild")]
        public async Task ListCharacters(CommandContext context)
        {
            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            StringBuilder characterList = new StringBuilder("\n__UserId__ - __Id__ - __Name__\n");
            foreach (var character in guild.WowCharacters)
            {
                characterList.Append($"{character.UserId} - {character.Id} - {character.Name}\n");
            }

            var dmChannel = await context.Member.CreateDmChannelAsync();
            await dmChannel.SendMessageAsync(characterList.ToString());
        }
    }
}