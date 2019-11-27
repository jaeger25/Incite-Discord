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
        public User User { get; private set; }

        public override async Task BeforeExecutionAsync(CommandContext ctx)
        {
            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.DiscordId == ctx.User.Id);

            if (user == null)
            {
                user = new User()
                {
                    DiscordId = ctx.User.Id
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            User = user;
        }
    }
}
