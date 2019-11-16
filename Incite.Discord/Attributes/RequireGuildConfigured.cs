using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
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
    public class RequireGuildConfigured : CheckBaseAttribute
    {
        static HashSet<UInt64> ConfiguredGuildsCache { get; } = new HashSet<UInt64>();

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

            InciteDbContext dbContext = new InciteDbContext(null);
            var guild = await dbContext.Guilds.GetCurrentGuildAsync(context);

            int roleCount = await dbContext.Roles
                .Where(x => x.GuildId == guild.Id &&
                    (x.Kind == RoleKind.Everyone || x.Kind == RoleKind.Member || x.Kind == RoleKind.Officer || x.Kind == RoleKind.Leader))
                .CountAsync();

            int channelCount = await dbContext.Channels
                .Where(x => x.Guild.Id == guild.Id &&
                    (x.Kind == ChannelKind.Admin))
                .CountAsync();

            bool configured = roleCount == 4 &&
                channelCount == 1;

            if (configured)
            {
                ConfiguredGuildsCache.Add(context.Guild.Id);
            }

            return configured;
        }
    }
}
