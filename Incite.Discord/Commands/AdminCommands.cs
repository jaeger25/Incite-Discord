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
    public class AdminCommands : BaseCommandModule
    {
        [Command("setrole")]
        [Description("Sets the server role which corresponds with the RoleKind")]
        public async Task SetRole(CommandContext context,
            [Description("Values: Member, Officer, Admin")] RoleKind roleKind,
            [Description("Server role")] DiscordRole role)
        {
            using var dbContext = new InciteDbContext();
            var existingRole = await dbContext.Roles
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == roleKind);

            if (existingRole == null)
            {
                dbContext.Roles.Add(new Role()
                {
                    DiscordId = role.Id,
                    GuildId = (await dbContext.Guilds.GetCurrentGuildAsync(context)).Id,
                    Kind = roleKind
                });
            }
            else
            {
                existingRole.DiscordId = role.Id;
                dbContext.Roles.Update(existingRole);
            }

            await dbContext.SaveChangesAsync();
        }

        [Command("setchannel")]
        [Description("Sets the server channel which corresponds with the ChannelKind")]
        public async Task SetChannel(CommandContext context,
            [Description("Values: Unspecified, Admin")] ChannelKind channelKind,
            [Description("Server channel")] DiscordChannel channel)
        {
            using var dbContext = new InciteDbContext();
            var existingChannel = await dbContext.Channels
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == channelKind);

            if (existingChannel == null && channelKind != ChannelKind.Unspecified)
            {
                dbContext.Channels.Add(new Channel()
                {
                    DiscordId = channel.Id,
                    GuildId = (await dbContext.Guilds.GetCurrentGuildAsync(context)).Id,
                    Kind = channelKind
                });
            }
            else
            {
                existingChannel.DiscordId = channel.Id;
                dbContext.Channels.Update(existingChannel);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
