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
            var discordMember = await context.Guild.GetMemberAsync(context.User.Id);
            if (PermissionMethods.HasPermission(discordMember.PermissionsIn(context.Channel), Permissions.ManageGuild))
            {
                return true;
            }

            var dbContext = context.Services.GetService<InciteDbContext>();
            var member = await dbContext.Members.TryGetCurrentMemberAsync(context);

            return member?.MemberRoles
                .Any(x => x.Role.Kind >= RoleKind) ?? false;
        }
    }
}
