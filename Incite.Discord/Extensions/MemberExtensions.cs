using DSharpPlus.CommandsNext;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Extensions
{
    public static class MemberExtensions
    {
        public static Task<bool> IsMemberRegistered(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .AnyAsync(x => x.Guild.DiscordGuildId == discordGuildId && x.DiscordUserId == discordMemberId);
        }

        public static Task<bool> IsMemberRegistered(this DbSet<Member> members, CommandContext context)
        {
            return members.IsMemberRegistered(context.Guild.Id, context.Member.Id);
        }

        public static Task<Member> GetCurrentMemberAsync(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .FirstAsync(x => x.Guild.DiscordGuildId == discordGuildId && x.DiscordUserId == discordMemberId);
        }

        public static Task<Member> GetCurrentMemberAsync(this DbSet<Member> members, CommandContext context)
        {
            return members.GetCurrentMemberAsync(context.Guild.Id, context.Member.Id);
        }

        public static Task<Member?> TryGetCurrentMemberAsync(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .FirstOrDefaultAsync(x => x.Guild.DiscordGuildId == discordGuildId && x.DiscordUserId == discordMemberId);
        }

        public static Task<Member?> TryGetCurrentMemberAsync(this DbSet<Member> members, CommandContext context)
        {
            return members.GetCurrentMemberAsync(context.Guild.Id, context.Member.Id);
        }
    }
}
