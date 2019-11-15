using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Attributes;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("member")]
    [RequireGuildConfigured]
    [Description("Commands for managing guild members")]
    public class MemberCommands : BaseCommandModule
    {
        [Command("list")]
        [Description("Lists the registered guild members")]
        public async Task List(CommandContext context)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();

            using var dbContext = new InciteDbContext();

            var members = dbContext.Members
                .Where(x => x.Guild.DiscordId == context.Guild.Id)
                .AsAsyncEnumerable();

            StringBuilder memberList = new StringBuilder("Discord Name : Character Name");
            memberList.AppendLine();

            await foreach(var member in members)
            {
                string discordName = context.Guild.Members.ContainsKey(member.DiscordId) ?
                    (context.Guild.Members[member.DiscordId].Nickname ?? context.Guild.Members[member.DiscordId].DisplayName) :
                    "(Unknown)";

                memberList.AppendLine($"{discordName} : {member.PrimaryCharacterName} ");
            }

            await channel.SendMessageAsync(memberList.ToString());
        }

        [Command("register")]
        [Description("Lists the official guild raid days and times")]
        public async Task Register(CommandContext context, string primaryCharacterName)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();

            using var dbContext = new InciteDbContext();

            var guild = dbContext.Guilds
                .First(x => x.DiscordId == context.Guild.Id);

            var member = await dbContext.Members
                .TryGetCurrentMemberAsync(context);

            if (member == null)
            {
                dbContext.Add(new Member()
                {
                    DiscordId = context.Member.Id,
                    GuildId = guild.Id,
                    PrimaryCharacterName = primaryCharacterName
                });
            }
            else
            {
                member.PrimaryCharacterName = primaryCharacterName;
                dbContext.Update(member);
            }

            await dbContext.SaveChangesAsync();
            await channel.SendMessageAsync("Successfully registered!");
        }
    }
}
