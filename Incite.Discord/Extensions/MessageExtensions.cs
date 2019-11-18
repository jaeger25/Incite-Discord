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

        public static async Task<DiscordMessage> RefreshAsync(this DiscordMessage message)
        {
            return await message.Channel.GetMessageAsync(message.Id);
        }

        public static DiscordEmbedBuilder AddBlankFields(this DiscordEmbedBuilder embed, bool inline = false, int count = 1)
        {
            for(int i = 0; i < count; i++)
            {
                embed.AddField("\u200b", "\u200b", inline);
            }

            return embed;
        }
    }
}
