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
            [Description("Server role")] DiscordRole role)
        {
            var existingRole = await m_dbContext.Roles
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == roleKind);

            if (existingRole == null)
            {
                m_dbContext.Roles.Add(new Role()
                {
                    DiscordId = role.Id,
                    GuildId = (await m_dbContext.Guilds.GetCurrentGuildAsync(context)).Id,
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

            await context.Message.DeleteAsync();
            await context.Channel.SendMessageAsync($"Role set: {roleKind.ToString()} - {role}");
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
                    GuildId = (await m_dbContext.Guilds.GetCurrentGuildAsync(context)).Id,
                    Kind = channelKind
                });
            }
            else
            {
                existingChannel.DiscordId = channel.Id;
                m_dbContext.Channels.Update(existingChannel);
            }

            await m_dbContext.SaveChangesAsync();

            await context.Message.DeleteAsync();
            await context.Channel.SendMessageAsync($"Channel set: {channelKind.ToString()} - {channel}");
        }
    }
}
