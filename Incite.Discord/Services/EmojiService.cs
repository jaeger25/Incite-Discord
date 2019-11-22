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
        readonly IReadOnlyList<DiscordEmoji> m_emojis;

        public class EventEmojis
        {
            public DiscordEmoji Icon_Absent { get; set; }
            public DiscordEmoji Icon_Count { get; set; }
            public DiscordEmoji Icon_Date { get; set; }
            public DiscordEmoji Icon_Late { get; set; }
            public DiscordEmoji Icon_Maybe { get; set; }
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

            var guild = m_client.Guilds[Guild.InciteBotDiscordId];
            m_emojis = guild.GetEmojisAsync().WaitForResult();

            Events.Icon_Absent = m_emojis.First(x => x.Name == "Event_Icon_Absent");
            Events.Icon_Count = m_emojis.First(x => x.Name == "Event_Icon_Count");
            Events.Icon_Date = m_emojis.First(x => x.Name == "Event_Icon_Date");
            Events.Icon_Late = m_emojis.First(x => x.Name == "Event_Icon_Late");
            Events.Icon_Maybe = m_emojis.First(x => x.Name == "Event_Icon_Maybe");
            Events.Icon_Time = m_emojis.First(x => x.Name == "Event_Icon_Time");

            Events.Role_Range = m_emojis.First(x => x.Name == "Event_Role_Range");
            Events.Role_Tank = m_emojis.First(x => x.Name == "Event_Role_Tank");
            Events.Role_Healer = m_emojis.First(x => x.Name == "Event_Role_Healer");
            Events.Role_Melee = m_emojis.First(x => x.Name == "Event_Role_Melee");

            Events.Shaman_Resto = m_emojis.First(x => x.Name == "Event_Shaman_Resto");
            Events.Shaman_Enhance = m_emojis.First(x => x.Name == "Event_Shaman_Enhance");
            Events.Shaman_Ele = m_emojis.First(x => x.Name == "Event_Shaman_Ele");
            Events.Class_Shaman = m_emojis.First(x => x.Name == "Event_Class_Shaman");

            Events.Druid_Bear = m_emojis.First(x => x.Name == "Event_Druid_Bear");
            Events.Druid_Cat = m_emojis.First(x => x.Name == "Event_Druid_Cat");
            Events.Druid_Boomkin = m_emojis.First(x => x.Name == "Event_Druid_Boomkin");
            Events.Druid_Resto = m_emojis.First(x => x.Name == "Event_Druid_Resto");
            Events.Class_Druid = m_emojis.First(x => x.Name == "Event_Class_Druid");

            Events.Priest_Healer = m_emojis.First(x => x.Name == "Event_Priest_Healer");
            Events.Priest_Shadow = m_emojis.First(x => x.Name == "Event_Priest_Shadow");
            Events.Class_Priest = m_emojis.First(x => x.Name == "Event_Class_Priest");

            Events.Warrior_Prot = m_emojis.First(x => x.Name == "Event_Warrior_Prot");
            Events.Warrior_Dps = m_emojis.First(x => x.Name == "Event_Warrior_Dps");
            Events.Class_Warrior = m_emojis.First(x => x.Name == "Event_Class_Warrior");

            Events.Class_Warlock = m_emojis.First(x => x.Name == "Event_Class_Warlock");
            Events.Class_Rogue = m_emojis.First(x => x.Name == "Event_Class_Rouge");
            Events.Class_Hunter = m_emojis.First(x => x.Name == "Event_Class_Hunter");
            Events.Class_Mage = m_emojis.First(x => x.Name == "Event_Class_Mage");
        }

        public DiscordEmoji GetByDiscordName(string emojiName)
        {
            return m_emojis.First(x => x.Name == emojiName);
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
