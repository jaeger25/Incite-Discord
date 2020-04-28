using Castle.Core;
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
    public class DiscordEventMessage
    {
        readonly EmojiService m_emojis;
        readonly Models.Event m_guildEvent;
        readonly EpgpSnapshot? m_epgpSnapshot;

        DiscordMessage m_message;

        public DiscordEventMessage(DiscordClient client, DiscordMessage message, Models.Event guildEvent, EpgpSnapshot? epgpSnapshot = null)
        {
            m_message = message;
            m_guildEvent = guildEvent;
            m_epgpSnapshot = epgpSnapshot;
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

        public async Task UpdateAsync()
        {
            await m_message.ModifyAsync(embed: CreateEventMessageEmbed(m_guildEvent));
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
            await m_message.CreateReactionAsync(m_emojis.Events.Icon_Maybe);
            await m_message.CreateReactionAsync(m_emojis.Events.Icon_Absent);
        }

        void AddMemberListField(DiscordEmbedBuilder embed, string header, string? className, EpgpSnapshot? epgpSnapshot, IEnumerable<EventMember> members, bool inline = true, bool inclueEmoji = true)
        {
            var memberList = members
                .Select(x => Tuple.Create(x, epgpSnapshot?.Standings
                        .FirstOrDefault(y => x.Member.User.WowCharacters.Contains(y.WowCharacter))
                        ?.EffortPoints ?? 0))
                .OrderByDescending(x => x.Item2)
                .ToArray();

            StringBuilder classColumnText = new StringBuilder($"__**{header}**__ - {memberList.Length}\n");
            foreach(var memberEpPair in memberList)
            {
                var member = memberEpPair.Item1;
                var ep = memberEpPair.Item2;
                string separator = inline ? "\n" : ", ";

                // TODO: Get name by class
                var characterName = member.Member.User.WowCharacters
                    .Where(x => string.IsNullOrEmpty(className) ? true : x.WowClass.Name == className)
                    .FirstOrDefault()
                    ?.Name;

                if (string.IsNullOrEmpty(characterName))
                {
                    characterName = "(Unknown) - ensure char added";
                }

                string epString = ep == 0 ? "" : $" - {ep}ep";
                string memberString = inclueEmoji ?
                    $"{(inline ? "\n" : "")}{m_emojis.GetByDiscordName(member.EmojiDiscordName)} {characterName}{epString}{(inline ? "" : ", ")}" :
                    $"{(inline ? "\n" : "")}{characterName}{epString}{(inline ? "" : ", ")}";

                classColumnText.Append(memberString);
            }

            embed.AddField("\u200b", classColumnText.ToString().TrimEnd( new[] { ' ', ',' }), inline);
        }

        DiscordEmbed CreateEventMessageEmbed(Models.Event guildEvent)
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = $"{guildEvent.Name}",
                Description = $"{guildEvent.Description}",
            };

            string epgpSnapshotString = m_epgpSnapshot == null ? "" : $"EP Snapshot: {m_epgpSnapshot.DateTime.UtcDateTime}\n";
            embed.WithFooter($"=====================================================================\n{epgpSnapshotString}EventId: {guildEvent.Id}");

            // Player count and date
            AddEventField(embed, $"{m_emojis.Events.Icon_Count}", $"{guildEvent.EventMembers.Count(x => x.EmojiDiscordName != m_emojis.Events.Icon_Late.Name && x.EmojiDiscordName != m_emojis.Events.Icon_Maybe.Name && x.EmojiDiscordName != m_emojis.Events.Icon_Absent.Name )}");

            var guildUtcOffset = guildEvent.Guild.WowServer.UtcOffset;
            AddEventField(embed, $"{m_emojis.Events.Icon_Date}", $"{guildEvent.DateTime.ToOffset(guildUtcOffset).ToString("MM-dd")}");
            AddEventField(embed, $"{m_emojis.Events.Icon_Time}", $"{guildEvent.DateTime.ToOffset(guildUtcOffset).ToString("h:mm tt zzz")}");

            // Melee, Ranged, Healer counts
            AddEventField(embed, $"{m_emojis.Events.Role_Tank}", $"{guildEvent.EventMembers.Count(x => m_emojis.TankEmojis().Select(x => x.Name).Contains(x.EmojiDiscordName))}");
            AddEventField(embed, $"{m_emojis.Events.Role_Melee}", $"{m_emojis.Events.Role_Range}", $"{guildEvent.EventMembers.Count(x => m_emojis.MeleeEmojis().Select(x => x.Name).Contains(x.EmojiDiscordName))}",
                $"{guildEvent.EventMembers.Count(x => m_emojis.RangeEmojis().Select(x => x.Name).Contains(x.EmojiDiscordName))}");
            AddEventField(embed, $"{m_emojis.Events.Role_Healer}", $"{guildEvent.EventMembers.Count(x => m_emojis.HealerEmojis().Select(x => x.Name).Contains(x.EmojiDiscordName))}");

            // Class counts
            AddMemberListField(embed, "Warrior", "Warrior", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.WarriorEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Rogue", "Rogue", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.RogueEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Hunter", "Hunter", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.HunterEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Mage", "Mage", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.MageEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Warlock", "Warlock", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.WarlockEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Druid", "Druid", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.DruidEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Shaman", "Shaman", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.ShamanEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            AddMemberListField(embed, "Priest", "Priest", m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => m_emojis.PriestEmojis()
                    .Select(x => x.Name)
                    .Contains(x.EmojiDiscordName)));

            embed.AddBlankFields(true);

            AddMemberListField(embed, "Late", null, m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => x.EmojiDiscordName == m_emojis.Events.Icon_Late.Name), false, false);

            AddMemberListField(embed, "Maybe", null, m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => x.EmojiDiscordName == m_emojis.Events.Icon_Maybe.Name), false, false);

            AddMemberListField(embed, "Absent", null, m_epgpSnapshot, guildEvent.EventMembers
                .Where(x => x.EmojiDiscordName == m_emojis.Events.Icon_Absent.Name), false, false);

            return embed.Build();
        }

        static DiscordEmbedBuilder AddEventField(DiscordEmbedBuilder embed, string label, string format)
        {
            return embed.AddField("\u200b", $"{label} - {format}", true);
        }

        static DiscordEmbedBuilder AddEventField(DiscordEmbedBuilder embed, string label1, string label2, string format1, string format2)
        {
            return embed.AddField("\u200b", $"{label1} - {format1} {label2} - {format2}", true);
        }
    }
}
