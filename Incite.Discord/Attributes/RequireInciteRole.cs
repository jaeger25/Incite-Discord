using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Models;
using Incite.Discord.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Incite.Discord.Attributes
{
    public class RequireInciteRoleAttribute : CheckBaseAttribute
    {
        public RoleKind RoleKind { get; set; }

        public RequireInciteRoleAttribute(RoleKind kind)
        {
            RoleKind = kind;
        }

        public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            var discordMember = context.Member;
            if (PermissionMethods.HasPermission(discordMember.PermissionsIn(context.Channel), Permissions.ManageGuild))
            {
                return true;
            }

            var dbContext = context.Services.GetService<InciteDbContext>();
            var role = await dbContext.Roles
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == RoleKind);

            if (role == null)
            {
                return false;
            }

            return discordMember.Roles
                .Any(x => x.Id == role.DiscordId);
        }
    }
}
