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
    [Description("Commands for managing guild members and settings")]
    public class GuildCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public GuildCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("set-server")]
        [RequireInciteRole(RoleKind.Leader)]
        [Description("Sets the WoW server for the guild")]
        public async Task SetRealm(CommandContext context, WowServer server)
        {
            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            guild.WowServerId = server.Id;
            await m_dbContext.SaveChangesAsync();
        }
    }
}
