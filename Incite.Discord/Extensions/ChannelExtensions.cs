using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Extensions
{
    public static class ChannelExtensions
    {
        public static Task<Channel> GetChannelAsync(this DbSet<Channel> channels, UInt64 discordGuildId, ChannelKind channelKind)
        {
            return channels
                .FirstAsync(x => x.Guild.DiscordId == discordGuildId && x.Kind == channelKind);
        }

        public static Task<Channel> GetChannelAsync(this DbSet<Channel> channels, CommandContext context, ChannelKind channelKind)
        {
            return channels.GetChannelAsync(context.Guild.Id, channelKind);
        }

        public static DiscordChannel GetDiscordChannel(this Channel channel, CommandContext context)
        {
            return context.Guild.GetChannel(channel.DiscordId);
        }
    }
}
