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
        [Priority(100)]
        [RequireInciteRole(RoleKind.Member)]
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
        [Priority(90)]
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
        [Priority(80)]
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
        [RequireInciteRole(RoleKind.Officer)]
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
                [Description("Values: Member, Officer, Leader")] RoleKind roleKind,
                [Description("Name of the corresponding discord role")] DiscordRole discordRole)
            {
                if (roleKind == RoleKind.Everyone)
                {
                    return;
                }

                var existingRole = Guild.Roles
                    .FirstOrDefault(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == roleKind);

                if (existingRole == null)
                {
                    existingRole = new Role()
                    {
                        DiscordId = discordRole.Id,
                        GuildId = Guild.Id,
                        Kind = roleKind
                    };

                    m_dbContext.Roles.Add(existingRole);
                }
                else
                {
                    existingRole.DiscordId = discordRole.Id;
                    m_dbContext.Roles.Update(existingRole);
                }

                await m_dbContext.SaveChangesAsync();

                if (!discordRole.IsMentionable)
                {
                    try
                    {
                        await discordRole.ModifyAsync(mentionable: true);
                    }
                    catch(UnauthorizedException)
                    {
                        ResponseString = "Commands completed, but you need to manually re-order the roles in your serve so that 'Incite Bot' is above any roles you are trying to set here.";
                    }
                }
            }
        }
    }
}
