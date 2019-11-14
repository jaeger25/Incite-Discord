using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("member")]
    [Description("Commands for managing guild members")]
    public class MemberCommands : BaseCommandModule
    {
        [Command("list")]
        [Description("Lists the official guild raid days and times")]
        public async Task List(CommandContext context)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();
            await channel.TriggerTypingAsync();

            using var dbContext = new InciteDbContext();

            var members = dbContext.Members
                .Where(x => x.Guild.DiscordGuildId == context.Guild.Id);

            StringBuilder memberList = new StringBuilder("Character Name : Discord Name");

            foreach(var member in members)
            {
                string discordName = context.Guild.Members.ContainsKey(member.DiscordUserId) ?
                    context.Guild.Members[member.DiscordUserId].Nickname :
                    "(Unknown)";

                memberList.AppendLine($"{member.PrimaryCharacterName} : {discordName}");
            }

            await channel.SendMessageAsync(memberList.ToString());
        }

        [Command("register")]
        [Description("Lists the official guild raid days and times")]
        public async Task Register(CommandContext context, string primaryCharacterName)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();
            await channel.TriggerTypingAsync();

            using var dbContext = new InciteDbContext();

            var guild = dbContext.Guilds
                .First(x => x.DiscordGuildId == context.Guild.Id);

            var member = dbContext.Members
                .FirstOrDefault(x => x.Guild.DiscordGuildId == context.Guild.Id && x.DiscordUserId == context.Member.Id);

            if (member == null)
            {
                dbContext.Add(new Member()
                {
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
            await channel.SendMessageAsync("Success!");
        }
    }
}
