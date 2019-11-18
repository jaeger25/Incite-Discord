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
        readonly Models.Event m_guildEvent;

        DiscordMessage m_message;

        public EventMessage(DiscordClient client, DiscordMessage message, Models.Event guildEvent)
        {
            m_client = client;
            m_message = message;
            m_guildEvent = guildEvent;
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

        public async Task UpdateAsync(Models.Event guildEvent)
        {
            await m_message.ModifyAsync(embed: CreateEventMessageEmbed(guildEvent));
        }

        public async Task AddReactionsToEventMessageAsync()
        {
            await m_message.CreateReactionAsync(m_emojis.ProtEmoji);
            await m_message.CreateReactionAsync(m_emojis.BearEmoji);
            await m_message.CreateReactionAsync(m_emojis.WarriorEmoji);
            await m_message.CreateReactionAsync(m_emojis.RogueEmoji);
            await m_message.CreateReactionAsync(m_emojis.CatEmoji);
            await m_message.CreateReactionAsync(m_emojis.EnhanceEmoji);
            await m_message.CreateReactionAsync(m_emojis.HunterEmoji);
            await m_message.CreateReactionAsync(m_emojis.MageEmoji);
            await m_message.CreateReactionAsync(m_emojis.WarlockEmoji);
            await m_message.CreateReactionAsync(m_emojis.ShadowEmoji);
            await m_message.CreateReactionAsync(m_emojis.EleShamanEmoji);
            await m_message.CreateReactionAsync(m_emojis.BoomkinEmoji);
            await m_message.CreateReactionAsync(m_emojis.RestoDruidEmoji);
            await m_message.CreateReactionAsync(m_emojis.RestoShamanEmoji);
            await m_message.CreateReactionAsync(m_emojis.PriestEmoji);
            await m_message.CreateReactionAsync(m_emojis.AbsentEmoji);
            await m_message.CreateReactionAsync(m_emojis.LateEmoji);
        }

        void AddClassField(DiscordEmbedBuilder embed, string className, IEnumerable<EventMember> members)
        {
            var classMembers = members
                .ToArray();

            StringBuilder classColumnText = new StringBuilder($"__**{className}**__ - {classMembers.Length}");
            foreach(var member in classMembers)
            {
                classColumnText.Append($"\n{m_emojis.GetByDiscordId(member.EmojiDiscordId)} {member.Member.PrimaryCharacterName}");
            }
        }

        DiscordEmbed CreateEventMessageEmbed(Models.Event guildEvent)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{guildEvent.Name}",
                Description = $"{guildEvent.Description}",
            };

            // Player count and date
            AddEventField(embed, "Count", $"{guildEvent.EventMembers.Count(x => x.EmojiDiscordId != m_emojis.LateEmoji.Id && x.EmojiDiscordId != m_emojis.AbsentEmoji.Id )}");
            AddEventField(embed, "Date", $"{guildEvent.DateTime.ToString("MM-dd")}");
            AddEventField(embed, "Time", $"{guildEvent.DateTime.ToString("t")}");

            // Melee, Ranged, Healer counts
            AddEventField(embed, "Tanks", $"{guildEvent.EventMembers.Count(x => m_emojis.TankEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");
            AddEventField(embed, "DPS", $"{guildEvent.EventMembers.Count(x => m_emojis.MeleeEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}",
                $"{guildEvent.EventMembers.Count(x => m_emojis.RangeEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");
            AddEventField(embed, "Healers", $"{guildEvent.EventMembers.Count(x => m_emojis.HealerEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");

            // Class counts
            AddClassField(embed, "Warrior", guildEvent.EventMembers
                .Where(x => m_emojis.WarriorEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Rogue", guildEvent.EventMembers
                .Where(x => m_emojis.RogueEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Hunter", guildEvent.EventMembers
                .Where(x => m_emojis.HunterEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Mage", guildEvent.EventMembers
                .Where(x => m_emojis.MageEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Warlock", guildEvent.EventMembers
                .Where(x => m_emojis.WarlockEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Druid", guildEvent.EventMembers
                .Where(x => m_emojis.DruidEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Shaman", guildEvent.EventMembers
                .Where(x => m_emojis.ShamanEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddClassField(embed, "Priest", guildEvent.EventMembers
                .Where(x => m_emojis.PriestEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            embed.AddBlankFields();

            AddEventField(embed, "Late", $"{guildEvent.EventMembers.Count(x => x.EmojiDiscordId == m_emojis.LateEmoji.Id)}");
            AddEventField(embed, "Absent", $"{guildEvent.EventMembers.Count(x => x.EmojiDiscordId == m_emojis.AbsentEmoji.Id)}");

            return embed.Build();
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
