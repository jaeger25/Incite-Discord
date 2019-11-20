using DSharpPlus;
using DSharpPlus.Entities;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incite.Discord.Services
{
    public class EmojiService
    {
        readonly DiscordClient m_client;
        readonly DiscordGuild m_inciteGuild;
        readonly Dictionary<string, DiscordEmoji> m_emojis = new Dictionary<string, DiscordEmoji>();

        public class EventEmojis
        {
            public DiscordEmoji Icon_Absent { get; set; }
            public DiscordEmoji Icon_Count { get; set; }
            public DiscordEmoji Icon_Date { get; set; }
            public DiscordEmoji Icon_Late { get; set; }
            public DiscordEmoji Icon_Time { get; set; }
            public DiscordEmoji Role_Range { get; set; }
            public DiscordEmoji Role_Tank { get; set; }
            public DiscordEmoji Role_Healer { get; set; }
            public DiscordEmoji Role_Melee { get; set; }
            public DiscordEmoji Shaman_Resto { get; set; }
            public DiscordEmoji Shaman_Enhance { get; set; }
            public DiscordEmoji Shaman_Ele { get; set; }
            public DiscordEmoji Class_Shaman { get; set; }
            public DiscordEmoji Druid_Bear { get; set; }
            public DiscordEmoji Druid_Cat { get; set; }
            public DiscordEmoji Druid_Boomkin { get; set; }
            public DiscordEmoji Druid_Resto { get; set; }
            public DiscordEmoji Class_Druid { get; set; }
            public DiscordEmoji Priest_Healer { get; set; }
            public DiscordEmoji Priest_Shadow { get; set; }
            public DiscordEmoji Class_Priest { get; set; }
            public DiscordEmoji Class_Rogue { get; set; }
            public DiscordEmoji Class_Hunter { get; set; }
            public DiscordEmoji Class_Mage { get; set; }
            public DiscordEmoji Class_Warlock { get; set; }
            public DiscordEmoji Warrior_Prot { get; set; }
            public DiscordEmoji Warrior_Dps { get; set; }
            public DiscordEmoji Class_Warrior { get; set; }
        }

        public EventEmojis Events { get; } = new EventEmojis();

        public EmojiService(DiscordClient client)
        {
            m_client = client;

            m_inciteGuild = m_client.Guilds.ContainsKey(Guild.InciteDiscordId) ?
                m_client.Guilds[Guild.InciteDiscordId] :
                m_client.Guilds[Guild.InciteTestDiscordId];

            Events.Icon_Absent = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Icon_Absent").Value;
            Events.Icon_Count = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Icon_Count").Value;
            Events.Icon_Date = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Icon_Date").Value;
            Events.Icon_Late = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Icon_Late").Value;
            Events.Icon_Time = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Icon_Time").Value;

            Events.Role_Range = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Role_Range").Value;
            Events.Role_Tank = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Role_Tank").Value;
            Events.Role_Healer = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Role_Healer").Value;
            Events.Role_Melee = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Role_Melee").Value;

            Events.Shaman_Resto = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Shaman_Resto").Value;
            Events.Shaman_Enhance = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Shaman_Enhance").Value;
            Events.Shaman_Ele = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Shaman_Ele").Value;
            Events.Class_Shaman = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Shaman").Value;

            Events.Druid_Bear = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Druid_Bear").Value;
            Events.Druid_Cat = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Druid_Cat").Value;
            Events.Druid_Boomkin = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Druid_Boomkin").Value;
            Events.Druid_Resto = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Druid_Resto").Value;
            Events.Class_Druid = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Druid").Value;

            Events.Priest_Healer = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Priest_Healer").Value;
            Events.Priest_Shadow = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Priest_Shadow").Value;
            Events.Class_Priest = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Priest").Value;

            Events.Warrior_Prot = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Warrior_Prot").Value;
            Events.Warrior_Dps = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Warrior_Dps").Value;
            Events.Class_Warrior = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Warrior").Value;

            Events.Class_Warlock = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Warlock").Value;
            Events.Class_Rogue = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Rouge").Value;
            Events.Class_Hunter = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Hunter").Value;
            Events.Class_Mage = m_inciteGuild.Emojis.First(x => x.Value.Name == "Event_Class_Mage").Value;
        }

        public DiscordEmoji GetByDiscordName(string emojiName)
        {
            return m_inciteGuild.Emojis.First(x => x.Value.Name == emojiName).Value;
        }

        public IEnumerable<DiscordEmoji> WarriorEmojis()
        {
            return new[] { Events.Warrior_Dps, Events.Warrior_Prot };
        }

        public IEnumerable<DiscordEmoji> MageEmojis()
        {
            return new[] { Events.Class_Mage };
        }

        public IEnumerable<DiscordEmoji> HunterEmojis()
        {
            return new[] { Events.Class_Hunter };
        }

        public IEnumerable<DiscordEmoji> RogueEmojis()
        {
            return new[] { Events.Class_Rogue };
        }

        public IEnumerable<DiscordEmoji> PriestEmojis()
        {
            return new[] { Events.Priest_Shadow, Events.Priest_Healer };
        }

        public IEnumerable<DiscordEmoji> WarlockEmojis()
        {
            return new[] { Events.Class_Warlock };
        }

        public IEnumerable<DiscordEmoji> DruidEmojis()
        {
            return new[] { Events.Druid_Bear, Events.Druid_Boomkin, Events.Druid_Cat, Events.Druid_Resto };
        }

        public IEnumerable<DiscordEmoji> ShamanEmojis()
        {
            return new[] { Events.Shaman_Ele, Events.Shaman_Enhance, Events.Shaman_Resto };
        }

        public IEnumerable<DiscordEmoji> TankEmojis()
        {
            return new[] { Events.Warrior_Prot, Events.Druid_Bear };
        }

        public IEnumerable<DiscordEmoji> MeleeEmojis()
        {
            return new[] { Events.Class_Rogue, Events.Warrior_Dps, Events.Druid_Cat, Events.Shaman_Enhance };
        }

        public IEnumerable<DiscordEmoji> RangeEmojis()
        {
            return new[] { Events.Shaman_Ele, Events.Druid_Boomkin, Events.Class_Warlock, Events.Class_Mage, Events.Priest_Shadow, Events.Class_Hunter };
        }

        public IEnumerable<DiscordEmoji> HealerEmojis()
        {
            return new[] { Events.Druid_Resto, Events.Shaman_Resto, Events.Priest_Healer };
        }
    }
}
