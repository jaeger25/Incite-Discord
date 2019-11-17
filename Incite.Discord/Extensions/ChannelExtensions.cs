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
        public static DiscordChannel GetDiscordChannel(this Channel channel, CommandContext context)
        {
            return context.Guild.GetChannel(channel.DiscordId);
        }
    }
}
