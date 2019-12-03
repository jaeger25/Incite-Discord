﻿using DSharpPlus.CommandsNext;
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
            if (Member == null)
            {
                var dbContext = ctx.Services.GetService<InciteDbContext>();
                Member = await dbContext.Members
                    .Include(x => x.Guild)
                        .ThenInclude(x => x.Roles)
                    .Include(x => x.PrimaryWowCharacter)
                    .Include(x => x.User)
                        .ThenInclude(x => x.WowCharacters)
                    .FirstAsync(x => x.User.DiscordId == ctx.User.Id);

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
