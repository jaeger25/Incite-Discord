using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Models;
using Incite.Discord.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            using var dbContext = new InciteDbContext();

            var member = await dbContext.Members.GetCurrentMemberAsync(context);
            return member.Role.Kind >= RoleKind;
        }
    }
}
