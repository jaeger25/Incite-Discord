using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Models;
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

        public int TankCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Prot || x.Item1 == InciteEmoji.Bear)
            .Sum(x => x.Item2) ?? 0;

        public int MeleeCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Cat || x.Item1 == InciteEmoji.Enhance || x.Item1 == InciteEmoji.Rouge || x.Item1 == InciteEmoji.Warrior)
            .Sum(x => x.Item2) ?? 0;

        public int RangedCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Boomkin || x.Item1 == InciteEmoji.EleShaman || x.Item1 == InciteEmoji.Hunter || x.Item1 == InciteEmoji.Mage || x.Item1 == InciteEmoji.Shadow || x.Item1 == InciteEmoji.Warlock)
            .Sum(x => x.Item2) ?? 0;

        public int HealerCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Priest || x.Item1 == InciteEmoji.RestoDruid || x.Item1 == InciteEmoji.RestoShaman)
            .Sum(x => x.Item2) ?? 0;

        public int AttendingCount => TankCount + MeleeCount + RangedCount + HealerCount;

        public int WarriorCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Warrior || x.Item1 == InciteEmoji.Prot)
            .Sum(x => x.Item2) ?? 0;

        public int RogueCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Rouge)
            .Sum(x => x.Item2) ?? 0;

        public int HunterCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Hunter)
            .Sum(x => x.Item2) ?? 0;

        public int MageCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Mage)
            .Sum(x => x.Item2) ?? 0;

        public int WarlockCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Warlock)
            .Sum(x => x.Item2) ?? 0;

        public int DruidCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Bear || x.Item1 == InciteEmoji.Cat || x.Item1 == InciteEmoji.RestoDruid || x.Item1 == InciteEmoji.Boomkin)
            .Sum(x => x.Item2) ?? 0;

        public int ShamanCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.RestoShaman || x.Item1 == InciteEmoji.EleShaman || x.Item1 == InciteEmoji.Enhance)
            .Sum(x => x.Item2) ?? 0;

        public int PriestCount => m_message?.Reactions
            .Select(x => Tuple.Create(x.Emoji.GetDiscordName(), x.Count - 1))
            .Where(x => x.Item1 == InciteEmoji.Priest || x.Item1 == InciteEmoji.Shadow)
            .Sum(x => x.Item2) ?? 0;

        EventMessage(DiscordClient client, DiscordMessage message) : this(client)
        {
            m_message = message;
        }

        EventMessage(DiscordClient client)
        {
            m_client = client;
        }

        public static async Task<EventMessage> TryCreateFromMessageAsync(DiscordClient client, DiscordMessage message)
        {
            message = await message.HydrateAsync();
            if (!IsEventMessage(message))
            {
                return null;
            }

            return new EventMessage(client, message);
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

        public async Task AddUserAsync(Member member, DiscordEmoji emoji)
        {
            await m_message.ModifyAsync(embed: ModifyEventMessageEmbed(member, emoji, true));
        }

        public async Task RemoveUserAsync(Member member, DiscordEmoji emoji)
        {
            await m_message.ModifyAsync(embed: ModifyEventMessageEmbed(member, emoji, false));
        }

        public static async Task<EventMessage> CreateEventMessageAsync(CommandContext context, string title, string description, DateTimeOffset date)
        {
            var message = new EventMessage(context.Client);
            message.m_message = await context.RespondAsync(embed: CreateEventMessageEmbed(title, description, date));

            await message.AddReactionsToEventMessageAsync();
            return message;
        }

        DiscordEmbed ModifyEventMessageEmbed(Member member, DiscordEmoji emoji, bool isAdd)
        {
            var previousEmbed = m_message.Embeds[0];
            var embed = new DiscordEmbedBuilder()
            {
                Title = previousEmbed.Title,
                Description = previousEmbed.Description,
            };

            embed.WithFooter(previousEmbed.Footer.Text);
            for (int i = 0; i < 2; i++)
            {
                var field = previousEmbed.Fields[i];
                embed.AddField(field.Name, field.Value, field.Inline);
            }

            return AddCountFieldsToEmbed(embed, member, emoji, isAdd);
        }

        async Task AddReactionsToEventMessageAsync()
        {
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Prot));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Bear));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Warrior));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Rouge));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Cat));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Enhance));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Hunter));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Mage));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Warlock));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Shadow));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.EleShaman));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Boomkin));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.RestoDruid));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.RestoShaman));
            await m_message.CreateReactionAsync(DiscordEmoji.FromName(m_client, InciteEmoji.Priest));
        }

        DiscordEmbedBuilder AddCountFieldsToEmbed(DiscordEmbedBuilder embed, Member member, DiscordEmoji emoji, bool isAdd)
        {
            // Attending count
            AddEventField(embed, "Count", $"{AttendingCount}");

            // Melee, Ranged, Healer counts
            AddEventField(embed, "Tanks", $"{TankCount}");
            AddEventField(embed, "DPS", $"{MeleeCount}", $"{RangedCount}");
            AddEventField(embed, "Healers", $"{HealerCount}");

            // Class counts
            AddClassField(embed, "Warrior", WarriorCount, member, emoji, isAdd);
            AddClassField(embed, "Rogue", RogueCount, member, emoji, isAdd);
            AddClassField(embed, "Hunter", HunterCount, member, emoji, isAdd);

            AddClassField(embed, "Mage", MageCount, member, emoji, isAdd);
            AddClassField(embed, "Warlock", WarlockCount, member, emoji, isAdd);
            AddClassField(embed, "Druid", DruidCount, member, emoji, isAdd);

            AddClassField(embed, "Shaman", ShamanCount, member, emoji, isAdd);
            AddClassField(embed, "Priest", PriestCount, member, emoji, isAdd);

            embed.AddBlankField();

            return embed;
        }

        DiscordEmbedBuilder AddClassField(DiscordEmbedBuilder embed, string className, int count, Member member, DiscordEmoji emoji, bool isAdd)
        {
            if (count == 0)
            {
                return embed;
            }

            string userText = $"\n{emoji} {member.PrimaryCharacterName}";
            string classText = $"__**{className}**__";

            if (count == 1 && isAdd)
            {
                return embed.AddField("\u200b", $"{classText} - {count}{userText}", true);
            }
            else
            {
                var field = m_message.Embeds[0].Fields
                    .Where(x => x.Value.Contains(classText))
                    .First();

                return isAdd ?
                    embed.AddField("\u200b", $"{field.Value}{userText}", true) :
                    embed.AddField("\u200b", $"{field.Value.Replace(userText, "")}", true);
            }
        }

        static DiscordEmbedBuilder CreateEventMessageEmbed(string title, string description, DateTimeOffset date)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{title}",
                Description = $"{description}",
            };

            embed.WithFooter("Event");

            // Player count and date
            AddEventField(embed, "Date", $"{date.ToString("MM-dd")}");
            AddEventField(embed, "Time", $"{date.ToString("t")}");

            return embed;
        }

        static DiscordEmbedBuilder AddEventField(DiscordEmbedBuilder embed, string label, string format)
        {
            return embed.AddField("\u200b", $"__**{label}**__ - {format}", true);
        }

        static DiscordEmbedBuilder AddEventField(DiscordEmbedBuilder embed, string label, string format1, string format2)
        {
            return embed.AddField("\u200b", $"{format1} - __**{label}**__ - {format2}", true);
        }

        static bool IsEventMessage(DiscordMessage message)
        {
            return message.Author.IsCurrent &&
                message.Embeds.Count > 0 &&
                message.Embeds[0].Footer.Text.Trim() == "Event";
        }
    }
}
