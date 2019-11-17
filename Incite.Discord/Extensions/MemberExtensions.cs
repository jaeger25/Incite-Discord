using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
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
        public static Task<bool> IsMemberRegisteredAsync(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .AnyAsync(x => x.Guild.DiscordId == discordGuildId && x.User.DiscordId == discordMemberId);
        }

        public static Task<bool> IsCurrentMemberRegisteredAsync(this DbSet<Member> members, CommandContext context)
        {
            return members.IsMemberRegisteredAsync(context.Guild.Id, context.User.Id);
        }

        public static Task<Member> GetMemberAsync(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .FirstAsync(x => x.Guild.DiscordId == discordGuildId && x.User.DiscordId == discordMemberId);
        }

        public static Task<Member> GetCurrentMemberAsync(this DbSet<Member> members, CommandContext context)
        {
            return members.GetMemberAsync(context.Guild.Id, context.User.Id);
        }

        public static Task<Member?> TryGetMemberAsync(this DbSet<Member> members, UInt64 discordGuildId, UInt64 discordMemberId)
        {
            return members
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == discordGuildId && x.User.DiscordId == discordMemberId);
        }

        public static Task<Member?> TryGetCurrentMemberAsync(this DbSet<Member> members, CommandContext context)
        {
            return members.TryGetMemberAsync(context.Guild.Id, context.User.Id);
        }

        public static Task<User?> TryGetUserAsync(this DbSet<User> users, UInt64 discordUserId)
        {
            return users
                .FirstOrDefaultAsync(x => x.DiscordId == discordUserId);
        }

        public static Task<User?> TryGetCurrentUserAsync(this DbSet<User> users, CommandContext context)
        {
            return users.TryGetUserAsync(context.User.Id);
        }

        public static Task<Member> GetMemberAsync(this DbSet<Member> members, DiscordGuild guild, DiscordUser user)
        {
            return members
                .FirstAsync(x => x.User.DiscordId == user.Id && x.Guild.DiscordId == guild.Id);
        }
    }
}
