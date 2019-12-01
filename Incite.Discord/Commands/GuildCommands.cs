using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
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
    [Group("guild")]
    [RequireGuild]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing guild members and settings")]
    public class GuildCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public GuildCommands(InciteDbContext dbContext)
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

        [Command("list")]
        [RequireGuildConfigured]
        [Description("Lists the users which are the specified profession")]
        public async Task List(CommandContext context,
            [Description(Descriptions.WowProfession)] WowProfession profession)
        {
            var characters = Guild.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowProfessionId == profession.Id));

            StringBuilder message = new StringBuilder($"__**{profession}**__\n");
            foreach (var character in characters)
            {
                message.Append($"{character}\n");
            }

            ResponseString = message.ToString();
        }

        [Command("list")]
        [RequireGuildConfigured]
        [Description("Lists the users which know the specified recipe")]
        public async Task List(CommandContext context,
            [Description(Descriptions.WowItemRecipe)] [RemainingText] WowItemRecipe recipe)
        {
            var characters = Guild.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowCharacterRecipes
                        .Any(x => x.RecipeId == recipe.Recipe.Id)));

            StringBuilder message = new StringBuilder($"__**{recipe.Recipe}**__\n");
            foreach (var character in characters)
            {
                message.AppendLine($"{character}");
            }

            ResponseString = message.ToString();
        }

        [Group("admin")]
        [RequireGuild]
        [ModuleLifespan(ModuleLifespan.Transient)]
        [Description("Commands for managing guild settings")]
        public class AdminCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public AdminCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("set-server")]
            [RequireInciteRole(RoleKind.Leader)]
            [Description("Sets the WoW server for the guild")]
            public async Task SetRealm(CommandContext context,
                [Description(Descriptions.WowServer)] WowServer server)
            {
                Guild.WowServerId = server.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-role")]
            [RequireInciteRole(RoleKind.Leader)]
            [Description("Sets the server role which corresponds with the RoleKind")]
            public async Task SetRole(CommandContext context,
                [Description("Values: Everyone, Member, Officer, Leader")] RoleKind roleKind,
                [Description("Name of the corresponding discord role")] DiscordRole role)
            {
                var existingRole = Guild.Roles
                    .FirstOrDefault(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == roleKind);

                if (existingRole == null)
                {
                    m_dbContext.Roles.Add(new Role()
                    {
                        DiscordId = role.Id,
                        GuildId = Guild.Id,
                        Kind = roleKind
                    });
                }
                else
                {
                    existingRole.DiscordId = role.Id;
                    m_dbContext.Roles.Update(existingRole);
                }

                await m_dbContext.SaveChangesAsync();

                if (!role.IsMentionable)
                {
                    await role.ModifyAsync(mentionable: true);
                }
            }

            [Command("grant-role")]
            [RequireInciteRole(RoleKind.Officer)]
            [Description("Grants the role for a user")]
            public async Task GrantRole(CommandContext context,
                DiscordUser user,
                [Description("Values: Member, Officer, Leader")] RoleKind roleKind)
            {
                if (roleKind == RoleKind.Everyone)
                {
                    ResponseString = "Assignment not needed for Everyone role";
                    return;
                }

                bool allowedToChange = PermissionMethods.HasPermission(context.Member.PermissionsIn(context.Channel), Permissions.ManageGuild);
                if (!allowedToChange)
                {
                    allowedToChange = Member.MemberRoles
                        .Any(x => x.Role.Kind >= roleKind);
                }

                if (!allowedToChange)
                {
                    ResponseString = "Cannot set user's role higher than your own.";
                    return;
                }

                var member = await m_dbContext.Members.TryGetMemberAsync(context.Guild.Id, user.Id);
                if (member == null)
                {
                    ResponseString = "User is not registered.";
                    return;
                }

                var guildRoles = member.Guild.Roles
                    .Where(x => x.Kind > RoleKind.Everyone && x.Kind <= roleKind);

                var rolesToAdd = guildRoles
                    .Except(member.MemberRoles
                        .Select(x => x.Role));

                foreach (var roleToAdd in rolesToAdd)
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
                    ResponseString = $"<{context.Guild}> guild role(s) added: {rolesString.ToString().Trim(',')}";
                }
            }
        }
    }
}
