using DSharpPlus.CommandsNext;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Extensions
{
    public static class RoleExtensions
    {
        public static Task<Models.Role> GetRoleAsync(this DbSet<Models.Role> roles, UInt64 discordGuildId, RoleKind roleKind)
        {
            return roles
                .FirstAsync(x => x.Guild.DiscordId == discordGuildId && x.Kind == roleKind);
        }

        public static Task<Models.Role> GetRoleAsync(this DbSet<Models.Role> roles, CommandContext context, RoleKind roleKind)
        {
            return roles.GetRoleAsync(context.Guild.Id, roleKind);
        }
    }
}
