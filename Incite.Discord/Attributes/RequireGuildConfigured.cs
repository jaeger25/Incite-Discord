using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Attributes
{
    /// <summary>
    /// Defines that a command is only usable within a configured guild.
    /// </summary>
    public class RequireGuildConfiguredAttribute : CheckBaseAttribute
    {
        static HashSet<UInt64> ConfiguredGuildsCache { get; } = new HashSet<UInt64>();

        public RequireGuildConfiguredAttribute()
        {
        }

        public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            if (context.Guild == null)
            {
                return false;
            }
            else if (ConfiguredGuildsCache.Contains(context.Guild.Id))
            {
                return true;
            }

            var dbContext = context.Services.GetService<InciteDbContext>();
            var guild = await dbContext.Guilds.GetCurrentGuildAsync(context);

            var roles = await dbContext.Roles
                .Where(x => x.GuildId == guild.Id &&
                    (x.Kind == RoleKind.Everyone || x.Kind == RoleKind.Member || x.Kind == RoleKind.Officer || x.Kind == RoleKind.Leader))
                .ToArrayAsync();

            bool configured = roles.Length == 4 &&
                guild.WowServerId.HasValue &&
                guild.WowFaction.HasValue;

            if (configured)
            {
                ConfiguredGuildsCache.Add(context.Guild.Id);
            }
            else if (help)
            {
                StringBuilder response = new StringBuilder("Guild is not configured. Ask a user with 'Manage Server' permissions to type '!help guild admin'\n");
                if (roles.Length != 4)
                {
                    response.AppendLine($"\tMissing roles. Ensure you run '!guild admin set-role RoleKind yourdiscordrolehere' for RoleKind: Member, Officer, Leader");
                }
                if (!guild.WowServerId.HasValue)
                {
                    response.AppendLine("\tWowServer not assigned. Please assign your guild's server using '!guild admin set-server servername'");
                }
                if (!guild.WowFaction.HasValue)
                {
                    response.AppendLine("\tWowFaction not assigned. Please assign your guild's facting using '!guild admin set-faction WowFaction'");
                }

                await context.Message.RespondAsync(response.ToString());
            }

            return configured;
        }
    }
}
