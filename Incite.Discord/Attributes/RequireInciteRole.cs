using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Models;
using Incite.Discord.Extensions;
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
            var user = await context.Guild.GetMemberAsync(context.User.Id);
            if (user.IsOwner)
            {
                return true;
            }

            InciteDbContext m_dbContext = new InciteDbContext(null);

            var member = await m_dbContext.Members.GetCurrentMemberAsync(context);
            var memberRoles = await m_dbContext.MemberRoles.GetMemberRolesAsync(member);
            return memberRoles.Any(x => x.Kind >= RoleKind);
        }
    }
}
