using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
    [Group("admin")]
    [RequireGuild]
    [RequireUserPermissions(Permissions.ManageGuild)]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing bot settings for this guild")]
    public class AdminCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public AdminCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("set-role")]
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

        [Command("set-channel")]
        [Description("Sets the server channel which corresponds with the ChannelKind")]
        public async Task SetChannel(CommandContext context,
            [Description("Values: Unspecified, Admin")] ChannelKind channelKind,
            [Description("Server channel")] DiscordChannel channel)
        {
            var existingChannel = await m_dbContext.Channels
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == channelKind);

            if (existingChannel == null && channelKind != ChannelKind.Unspecified)
            {
                m_dbContext.Channels.Add(new Channel()
                {
                    DiscordId = channel.Id,
                    GuildId = Guild.Id,
                    Kind = channelKind
                });
            }
            else
            {
                existingChannel.DiscordId = channel.Id;
                m_dbContext.Channels.Update(existingChannel);
            }

            await m_dbContext.SaveChangesAsync();
        }
    }
}
