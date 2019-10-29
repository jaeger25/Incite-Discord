using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Extensions
{
    public static class MessageExtensions
    {
        public static async Task<DiscordMessage> HydrateAsync(this DiscordMessage message)
        {
            return message.Author != null ?
                message :
                await message.Channel.GetMessageAsync(message.Id);
        }
    }
}
