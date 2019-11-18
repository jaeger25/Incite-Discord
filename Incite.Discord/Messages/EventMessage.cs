using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Messages
{
    public class EventMessage
    {
        readonly EmojiService m_emojis;
        readonly DiscordClient m_client;
        DiscordMessage m_message;

        public int TankCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.ProtEmoji.Value || x.Emoji == m_emojis.BearEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int MeleeCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.CatEmoji.Value || x.Emoji == m_emojis.EnhanceEmoji.Value || x.Emoji == m_emojis.RougeEmoji.Value || x.Emoji == m_emojis.WarriorEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int RangedCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.BoomkinEmoji.Value || x.Emoji == m_emojis.EleShamanEmoji.Value || x.Emoji == m_emojis.HunterEmoji.Value || x.Emoji == m_emojis.MageEmoji.Value || x.Emoji == m_emojis.ShadowEmoji.Value || x.Emoji == m_emojis.WarlockEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int HealerCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.PriestEmoji.Value || x.Emoji == m_emojis.RestoDruidEmoji.Value || x.Emoji == m_emojis.RestoShamanEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int AttendingCount => TankCount + MeleeCount + RangedCount + HealerCount;

        public int LateCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.LateEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int AbsentCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.AbsentEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int WarriorCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.WarriorEmoji.Value || x.Emoji == m_emojis.ProtEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int RogueCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.RougeEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int HunterCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.HunterEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int MageCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.MageEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int WarlockCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.WarlockEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int DruidCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.BearEmoji.Value || x.Emoji == m_emojis.CatEmoji.Value || x.Emoji == m_emojis.RestoDruidEmoji.Value || x.Emoji == m_emojis.BoomkinEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int ShamanCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.RestoShamanEmoji.Value || x.Emoji == m_emojis.EleShamanEmoji.Value || x.Emoji == m_emojis.EnhanceEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public int PriestCount => m_message?.Reactions
            .Where(x => x.Emoji == m_emojis.PriestEmoji.Value || x.Emoji == m_emojis.ShadowEmoji.Value)
            .Sum(x => x.Count - 1) ?? 0;

        public EventMessage(DiscordClient client, DiscordMessage message) : this(client)
        {
            m_message = message;
        }

        EventMessage(DiscordClient client)
        {
            m_client = client;
            m_emojis = client.GetCommandsNext().Services.GetService<EmojiService>();
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
            await m_message.CreateReactionAsync(m_emojis.ProtEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.BearEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.WarriorEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.RougeEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.CatEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.EnhanceEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.HunterEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.MageEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.WarlockEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.ShadowEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.EleShamanEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.BoomkinEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.RestoDruidEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.RestoShamanEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.PriestEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.AbsentEmoji.Value);
            await m_message.CreateReactionAsync(m_emojis.LateEmoji.Value);
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

            embed.AddBlankFields();

            AddEventField(embed, "Late", $"{LateCount}");
            AddEventField(embed, "Absent", $"{AbsentCount}");

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
                    .FirstOrDefault();

                if (field != null)
                {
                    return isAdd ?
                        embed.AddField("\u200b", $"{field.Value}{userText}", true) :
                        embed.AddField("\u200b", $"{field.Value.Replace(userText, "")}", true);
                }
            }

            return embed;
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
