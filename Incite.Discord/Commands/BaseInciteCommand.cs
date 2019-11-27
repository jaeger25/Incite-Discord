using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.ApiModels;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Incite.Discord.Commands
{
    public abstract class BaseInciteCommand : BaseCommandModule
    {
        public Guild Guild { get; private set; }
        public Member Member { get; private set; }
        public User User { get; private set; }

        protected string ResponseString = "Command executed";

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            var dbContext = ctx.Services.GetService<InciteDbContext>();
            Member = await dbContext.Members
                .Include(x => x.Guild)
                    .ThenInclude(x => x.Roles)
                .Include(x => x.MemberRoles)
                .Include(x => x.PrimaryWowCharacter)
                .Include(x => x.User)
                    .ThenInclude(x => x.WowCharacters)
                .FirstOrDefaultAsync(x => x.User.DiscordId == ctx.User.Id);

            if (Member == null)
            {
                User = await dbContext.Users
                    .FirstOrDefaultAsync(x => x.DiscordId == ctx.User.Id);

                if (User == null)
                {
                    User = new User()
                    {
                        DiscordId = ctx.User.Id
                    };

                    dbContext.Users.Add(User);
                }

                Guild = await dbContext.Guilds
                    .FirstAsync(x => x.DiscordId == ctx.Guild.Id);

                Member = new Member()
                {
                    Guild = Guild,
                    User = User,
                };

                dbContext.Members.Add(Member);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                User = Member.User;
                Guild = Member.Guild;
            }
        }

        public override async Task AfterExecutionAsync(CommandContext ctx)
        {
            if (!string.IsNullOrEmpty(ResponseString))
            {
                await ctx.Message.RespondAsync(ResponseString);
            }
        }
    }
}
