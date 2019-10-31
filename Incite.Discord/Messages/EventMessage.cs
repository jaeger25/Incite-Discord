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

        public int TankCount => m_message.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Prot || x.Item1 == InciteEmoji.Bear)
            .Sum(x => x.Item2);

        public int MeleeCount => m_message.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Cat || x.Item1 == InciteEmoji.Enhance || x.Item1 == InciteEmoji.Rouge || x.Item1 == InciteEmoji.Warrior)
            .Sum(x => x.Item2);

        public int RangedCount => m_message.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Boomkin || x.Item1 == InciteEmoji.EleShaman || x.Item1 == InciteEmoji.Hunter || x.Item1 == InciteEmoji.Mage || x.Item1 == InciteEmoji.Shadow || x.Item1 == InciteEmoji.Warlock)
            .Sum(x => x.Item2);

        public int HealerCount => m_message.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Priest || x.Item1 == InciteEmoji.RestoDruid || x.Item1 == InciteEmoji.RestoShaman)
            .Sum(x => x.Item2);

        public int AttendingCount => TankCount + MeleeCount + RangedCount + HealerCount;


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
                .Where(x => x.Count > 1 && x.Emoji != currentReaction);

            foreach (var reaction in reactions)
            {
                await m_message.DeleteReactionAsync(reaction.Emoji, user);
            }

            m_message = await m_message.RefreshAsync();
        }

        public async Task AddUserAsync(DiscordUser user, DiscordEmoji emoji)
        {
            await m_message.ModifyAsync(embed: ModifyEventMessageEmbed(user, emoji));
        }

        public async Task RemoveUserAsync(DiscordUser user, DiscordEmoji emoji)
        {

        }

        DiscordEmbed ModifyEventMessageEmbed(DiscordUser user, DiscordEmoji emoji)
        {
            var embed = m_message.Embeds[0];
            embed.Fields[0].Name = $"Count - {AttendingCount}";

            embed.Fields[3].Name = $"Tanks - {TankCount}";
            embed.Fields[4].Name = $"{MeleeCount} - DPS - {RangedCount}";
            embed.Fields[5].Name = $"Healers - {HealerCount}";

            embed.Fields[GetFieldIndexForEmoji(emoji)].Value += $"{user.Username}\n";

            return embed;
        }

        public static DiscordEmbed CreateEventMessageEmbed(string name, DateTimeOffset date)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{name}",
            };

            embed.WithFooter("Event");

            // Player count and date
            embed.AddField($"Count - {0}", "\u200b", true);
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
            embed.AddField($"Warlock - {0}", "\u200b", true);
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

        static int GetFieldIndexForEmoji(DiscordEmoji emoji)
        {
            switch(emoji.GetDiscordName())
            {
                case InciteEmoji.Warrior:
                case InciteEmoji.Prot:
                    return 6;
                case InciteEmoji.Rouge:
                    return 7;
                case InciteEmoji.Hunter:
                    return 8;
                case InciteEmoji.Mage:
                    return 9;
                case InciteEmoji.Warlock:
                    return 10;
                case InciteEmoji.Bear:
                case InciteEmoji.Cat:
                case InciteEmoji.Boomkin:
                case InciteEmoji.RestoDruid:
                    return 11;
                case InciteEmoji.Enhance:
                case InciteEmoji.EleShaman:
                case InciteEmoji.RestoShaman:
                    return 12;
                case InciteEmoji.Priest:
                case InciteEmoji.Shadow:
                    return 13;
                default:
                    throw new ArgumentException($"Unexpected DiscordEmoji: {emoji.GetDiscordName()}");
            }
        }
    }
}
