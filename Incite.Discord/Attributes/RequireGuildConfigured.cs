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

            int roleCount = await dbContext.Roles
                .Where(x => x.GuildId == guild.Id &&
                    (x.Kind == RoleKind.Member || x.Kind == RoleKind.Officer || x.Kind == RoleKind.Leader))
                .CountAsync();

            bool configured = roleCount == 3;

            if (configured)
            {
                ConfiguredGuildsCache.Add(context.Guild.Id);
            }

            return configured;
        }
    }
}
