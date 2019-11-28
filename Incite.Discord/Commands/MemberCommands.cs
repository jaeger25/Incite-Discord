using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Incite.Discord.ApiModels;
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
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing guild members")]
    public class MemberCommands : BaseInciteCommand
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

            StringBuilder memberList = new StringBuilder("UserId | MemberId | Discord Name | Character Name");
            memberList.AppendLine();

            foreach (var member in Guild.Members)
            {
                string discordName = context.Guild.Members.ContainsKey(member.User.DiscordId) ?
                    (context.Guild.Members[member.User.DiscordId].Nickname ?? context.Guild.Members[member.User.DiscordId].DisplayName) :
                    "(Unknown)";

                memberList.AppendLine($"{member.UserId} | {member.Id} | {discordName} | {member.PrimaryWowCharacter}");
            }

            await channel.SendMessageAsync(memberList.ToString());
        }

        [Command("register")]
        [Description("Lists the official guild raid days and times")]
        public async Task Register(CommandContext context,
            [Description(Descriptions.WowCharacter)] UserWowCharacter primaryCharacter)
        {
            await context.Message.DeleteAsync();

            var channel = await context.Member.CreateDmChannelAsync();

            Member.PrimaryWowCharacterId = primaryCharacter.Character.Id;
            await m_dbContext.SaveChangesAsync();

            var adminChannel = Guild.Channels
                .First(x => x.Kind == ChannelKind.Admin);

            await adminChannel.GetDiscordChannel(context).SendMessageAsync($"-----------\n{context.Member} has registered as {primaryCharacter.Character.Name}. Please assign them a role using the \"!member grant-role {context.User.Username}#{context.User.Discriminator}\" command");

            await channel.SendMessageAsync("Your registration is complete, but pending Officer role assignment");
        }

        [Command("grant-role")]
        [RequireInciteRole(RoleKind.Officer)]
        [Description("Grants the role for a user")]
        public async Task GrantRole(CommandContext context,
            DiscordUser user,
            [Description("Values: Everyone, Member, Officer, Leader")] RoleKind roleKind)
        {
            bool allowedToChange = PermissionMethods.HasPermission(context.Member.PermissionsIn(context.Channel), Permissions.ManageGuild);
            if (!allowedToChange)
            {
                allowedToChange = Member.MemberRoles
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

            var guildRoles = member.Guild.Roles
                .Where(x => x.Kind > RoleKind.Everyone && x.Kind <= roleKind);

            var rolesToAdd = guildRoles
                .Except(member.MemberRoles
                    .Select(x => x.Role));

            foreach(var roleToAdd in rolesToAdd)
            {
                member.MemberRoles.Add(new MemberRole()
                {
                    MemberId = member.Id,
                    RoleId = roleToAdd.Id
                });
            }

            await m_dbContext.SaveChangesAsync();

            StringBuilder rolesString = new StringBuilder();
            var discordMember = await context.Guild.GetMemberAsync(user.Id);
            foreach (var role in member.MemberRoles.Select(x => x.Role))
            {
                if (!discordMember.Roles.Any(x => x.Id == role.DiscordId))
                {
                    var discordRole = context.Guild.GetRole(role.DiscordId);
                    try
                    {
                        await discordMember.GrantRoleAsync(discordRole);

                        rolesString.Append(discordRole.Name).Append(",");
                    }
                    catch (UnauthorizedException e)
                    {
                        var dmChannel = await context.Member.CreateDmChannelAsync();
                        await dmChannel.SendMessageAsync("You must manually edit the server roles such that the 'Incite' bot role is higher than any other roles you wish it to auto-assign");
                    }
                }
            }

            if (rolesString.Length > 0)
            {
                var dmChannel = await discordMember.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync($"<{context.Guild}> guild role(s) added: {rolesString.ToString().Trim(',')}");
            }
        }
    }
}
