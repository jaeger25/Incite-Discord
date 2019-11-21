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
    public class RequireMemberRegisteredAttribute : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            var dbContext = context.Services.GetService<InciteDbContext>();
            bool isRegistered = await dbContext.Members.IsCurrentMemberRegisteredAsync(context);

            if (!isRegistered)
            {
                var discordMember = await context.Guild.GetMemberAsync(context.User.Id);
                var dmChannel = await discordMember.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("You must first register with the '!member register' command");
            }

            return isRegistered;
        }
    }
}
