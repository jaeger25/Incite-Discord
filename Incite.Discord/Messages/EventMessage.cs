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
        }

        public async Task UpdateAsync(Models.Event guildEvent)
        {
            await m_message.ModifyAsync(embed: CreateEventMessageEmbed(guildEvent));
        }

        public async Task AddReactionsToEventMessageAsync()
        {
            await m_message.CreateReactionAsync(m_emojis.Events.Warrior_Prot);
            await m_message.CreateReactionAsync(m_emojis.Events.Warrior_Dps);
            await m_message.CreateReactionAsync(m_emojis.Events.Druid_Bear);
            await m_message.CreateReactionAsync(m_emojis.Events.Druid_Cat);
            await m_message.CreateReactionAsync(m_emojis.Events.Druid_Boomkin);
            await m_message.CreateReactionAsync(m_emojis.Events.Druid_Resto);
            await m_message.CreateReactionAsync(m_emojis.Events.Shaman_Ele);
            await m_message.CreateReactionAsync(m_emojis.Events.Shaman_Enhance);
            await m_message.CreateReactionAsync(m_emojis.Events.Shaman_Resto);
            await m_message.CreateReactionAsync(m_emojis.Events.Priest_Healer);
            await m_message.CreateReactionAsync(m_emojis.Events.Priest_Shadow);
            await m_message.CreateReactionAsync(m_emojis.Events.Class_Warlock);
            await m_message.CreateReactionAsync(m_emojis.Events.Class_Mage);
            await m_message.CreateReactionAsync(m_emojis.Events.Class_Hunter);
            await m_message.CreateReactionAsync(m_emojis.Events.Class_Rogue);
            await m_message.CreateReactionAsync(m_emojis.Events.Icon_Late);
            await m_message.CreateReactionAsync(m_emojis.Events.Icon_Absent);
        }

        void AddMemberListField(DiscordEmbedBuilder embed, string header, IEnumerable<EventMember> members, bool inline = true, bool inclueEmoji = true)
        {
            var memberList = members
                .ToArray();

            StringBuilder classColumnText = new StringBuilder($"__**{header}**__ - {memberList.Length}\n");
            foreach(var member in memberList)
            {
                string separator = inline ? "\n" : ", ";

                string memberString = inclueEmoji ?
                    $"{(inline ? "\n" : "")}{m_emojis.GetByDiscordId(member.EmojiDiscordId)} {member.Member.PrimaryCharacterName}{(inline ? "" : ", ")}" :
                    $"{(inline ? "\n" : "")}{member.Member.PrimaryCharacterName}{(inline ? "" : ", ")}";

                classColumnText.Append(memberString);
            }

            embed.AddField("\u200b", classColumnText.ToString(), inline);
        }

        DiscordEmbed CreateEventMessageEmbed(Models.Event guildEvent)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{guildEvent.Name}",
                Description = $"{guildEvent.Description}",
            };

            // Player count and date
            AddEventField(embed, "Count", $"{guildEvent.EventMembers.Count(x => x.EmojiDiscordId != m_emojis.Events.Icon_Late.Id && x.EmojiDiscordId != m_emojis.Events.Icon_Absent.Id )}");
            AddEventField(embed, "Date", $"{guildEvent.DateTime.ToString("MM-dd")}");
            AddEventField(embed, "Time", $"{guildEvent.DateTime.ToString("t")}");

            // Melee, Ranged, Healer counts
            AddEventField(embed, "Tanks", $"{guildEvent.EventMembers.Count(x => m_emojis.TankEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");
            AddEventField(embed, "DPS", $"{guildEvent.EventMembers.Count(x => m_emojis.MeleeEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}",
                $"{guildEvent.EventMembers.Count(x => m_emojis.RangeEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");
            AddEventField(embed, "Healers", $"{guildEvent.EventMembers.Count(x => m_emojis.HealerEmojis().Select(x => x.Id).Contains(x.EmojiDiscordId))}");

            // Class counts
            AddMemberListField(embed, "Warrior", guildEvent.EventMembers
                .Where(x => m_emojis.WarriorEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Rogue", guildEvent.EventMembers
                .Where(x => m_emojis.RogueEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Hunter", guildEvent.EventMembers
                .Where(x => m_emojis.HunterEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Mage", guildEvent.EventMembers
                .Where(x => m_emojis.MageEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Warlock", guildEvent.EventMembers
                .Where(x => m_emojis.WarlockEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Druid", guildEvent.EventMembers
                .Where(x => m_emojis.DruidEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Shaman", guildEvent.EventMembers
                .Where(x => m_emojis.ShamanEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            AddMemberListField(embed, "Priest", guildEvent.EventMembers
                .Where(x => m_emojis.PriestEmojis()
                    .Select(x => x.Id)
                    .Contains(x.EmojiDiscordId)));

            embed.AddBlankFields(true);

            AddMemberListField(embed, "Late", guildEvent.EventMembers
                .Where(x => x.EmojiDiscordId == m_emojis.Events.Icon_Late.Id), false, false);

            AddMemberListField(embed, "Absent", guildEvent.EventMembers
                .Where(x => x.EmojiDiscordId == m_emojis.Events.Icon_Absent.Id), false, false);

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
