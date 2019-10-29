using DSharpPlus;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Messages
{
    public class EventMessage
    {
        DiscordClient m_client;
        DiscordMessage m_message;

        private EventMessage(DiscordClient client, DiscordMessage message)
        {
            m_client = client;
            m_message = message;
        }

        public static async Task<EventMessage> TryCreateFromMessageAsync(DiscordClient client, DiscordMessage message)
        {
            message = await message.HydrateAsync();
            if (!IsEventMessage(message))
            {
                return null;
            }

            var eventMessage = new EventMessage(client, message);

            return eventMessage;
        }

        public async Task RemovePreviousReactionsAsync(DiscordUser user, DiscordEmoji currentReaction)
        {
            var reactions = m_message.Reactions
                .Where(x => x.Count > 1);

            foreach (var reaction in reactions)
            {
                await m_message.DeleteReactionAsync(reaction.Emoji, user);
            }
        }

        public async Task AddUserAsync(DiscordUser user, DiscordEmoji emoji)
        {
            string[] NonClassReactions = { InciteEmoji.Absent, InciteEmoji.Late, InciteEmoji.Tank, InciteEmoji.Melee, InciteEmoji.Ranged, InciteEmoji.Healer };

            var embed = m_message.Embeds[0];

            var count = m_message.Reactions
                .Where(x => x.Count > 1 && !NonClassReactions.Contains(x.Emoji.GetDiscordName()))
                .Sum(x => x.Count - 1);

            await m_message.ModifyAsync(null, CreateEventMessageEmbed(embed.Title, DateTimeOffset.Parse(embed.Fields[1].Name), count));
        }

        public static DiscordEmbed CreateEventMessageEmbed(string name, DateTimeOffset date, int count = 0)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"Event - {name}",
            };

            embed.WithFooter("Event");

            // Player count and date
            embed.AddField($"Count - {count}", "\u200b", true);
            embed.AddField($"{date.ToString("MM-dd")}", "\u200b", true);
            embed.AddField($"Time - {date.ToString("t")}", "\u200b", true);

            // Melee, Ranged, Healer counts
            embed.AddField($"Tanks - {0}", "\u200b", true);
            embed.AddField($"{0} - DPS - {0}", "\u200b", true);
            embed.AddField($"Healers - {0}", "\u200b", true);

            // Class counts
            embed.AddField($"Warrior - {0}", "\u200b", true);
            embed.AddField($"Rogue - {0}", "\u200b", true);
            embed.AddField($"Hunter - {0}", "\u200b", true);
            embed.AddField($"Mage - {0}", "\u200b", true);
            embed.AddField($"Warlock - {0}", "\u200bn", true);
            embed.AddField($"Druid - {0}", "\u200b", true);
            embed.AddField($"Shaman - {0}", "\u200b", true);
            embed.AddField($"Priest - {0}", "\u200b", true);
            embed.AddField("\u200b", "\u200b", true);

            return embed;
        }

        public static async Task AddReactionsToEventMessageAsync(DiscordClient client, DiscordMessage message)
        {
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Prot));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Bear));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Warrior));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Rouge));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Cat));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Enhance));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Hunter));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Mage));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Warlock));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Shadow));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.EleShaman));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Boomkin));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.RestoDruid));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.RestoShaman));
            await message.CreateReactionAsync(DiscordEmoji.FromName(client, InciteEmoji.Priest));
        }

        static bool IsEventMessage(DiscordMessage message)
        {
            return message.Author.IsCurrent &&
                message.Embeds.Count > 0 &&
                message.Embeds[0].Footer.Text == "Event";
        }
    }
}
