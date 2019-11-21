using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
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
        readonly InciteDbContext m_dbContext;

        public MemberCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list")]
        [RequireMemberRegistered]
        [Description("Lists the registered guild members")]
        public async Task List(CommandContext context)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            StringBuilder memberList = new StringBuilder("Discord Name : Character Name");
            memberList.AppendLine();

            foreach (var member in guild.Members)
            {
                string discordName = context.Guild.Members.ContainsKey(member.User.DiscordId) ?
                    (context.Guild.Members[member.User.DiscordId].Nickname ?? context.Guild.Members[member.User.DiscordId].DisplayName) :
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

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            User user = await m_dbContext.Users.TryGetCurrentUserAsync(context);
            if (user == null)
            {
                user = new User()
                {
                    DiscordId = context.User.Id
                };

                m_dbContext.Users.Add(user);
            }

            var member = await m_dbContext.Members
                .TryGetCurrentMemberAsync(context);

            if (member == null)
            {
                member = new Member()
                {
                    User = user,
                    GuildId = guild.Id,
                    PrimaryCharacterName = primaryCharacterName
                };

                m_dbContext.Members.Add(member);
            }
            else
            {
                member.PrimaryCharacterName = primaryCharacterName;
                m_dbContext.Update(member);
            }

            await m_dbContext.SaveChangesAsync();

            var adminChannel = guild.Channels
                .First(x => x.Kind == ChannelKind.Admin);

            await adminChannel.GetDiscordChannel(context).SendMessageAsync($"{context.Member} has registered as <{primaryCharacterName}>. Please assign them a role using the \"!member grantrole\" command");

            await channel.SendMessageAsync("Your registration is complete, but pending Officer role assignment");
        }

        [Command("grantrole")]
        [RequireInciteRole(RoleKind.Officer)]
        [Description("Grants the role for a user")]
        public async Task GrantRole(CommandContext context,
            DiscordUser user,
            [Description("Values: Everyone, Member, Officer, Leader")] RoleKind roleKind)
        {
            bool allowedToChange = PermissionMethods.HasPermission(context.Member.PermissionsIn(context.Channel), Permissions.ManageGuild);
            if (!allowedToChange)
            {
                var officer = await m_dbContext.Members.GetCurrentMemberAsync(context);
                allowedToChange = officer.MemberRoles
                    .Any(x => x.Role.Kind >= roleKind);
            }

            if (!allowedToChange)
            {
                await context.Channel.SendMessageAsync("Cannot set user's role higher than your own.");
                return;
            }

            var member = await m_dbContext.Members.TryGetMemberAsync(context.Guild.Id, user.Id);
            if (member == null)
            {
                await context.Channel.SendMessageAsync("User is not registered.");
                return;
            }

            var role = await m_dbContext.Roles.GetRoleAsync(context, roleKind);
            bool hasRole = member.MemberRoles
                .Any(x => x.RoleId == role.Id);

            if (!hasRole)
            {
                m_dbContext.MemberRoles.Add(new MemberRole()
                {
                    MemberId = member.Id,
                    RoleId = role.Id
                });

                await m_dbContext.SaveChangesAsync();
            }

            var discordRole = context.Guild.GetRole(role.DiscordId);
            var discordMember = await context.Guild.GetMemberAsync(user.Id);
            try
            {
                await discordMember.GrantRoleAsync(discordRole);

                var dmChannel = await discordMember.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync($"You're guild role has been set to: {roleKind.ToString()}");
            }
            catch(UnauthorizedException e)
            {
                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync("You must manually edit the server roles such that the 'Incite' role is higher than any other roles you wish it to auto-assign");
            }

            await context.Message.ModifyAsync($"Role set for: {user}");
        }
    }
}
