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
        readonly Dictionary<ulong, DiscordEmoji> m_emojis = new Dictionary<ulong, DiscordEmoji>();

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

            var guild = m_client.Guilds.ContainsKey(Guild.InciteDiscordId) ?
                m_client.Guilds[Guild.InciteDiscordId] :
                m_client.Guilds[Guild.InciteTestDiscordId];

            Events.Icon_Absent = guild.Emojis.First(x => x.Value.Name == "Event_Icon_Absent").Value;
            Events.Icon_Count = guild.Emojis.First(x => x.Value.Name == "Event_Icon_Count").Value;
            Events.Icon_Date = guild.Emojis.First(x => x.Value.Name == "Event_Icon_Date").Value;
            Events.Icon_Late = guild.Emojis.First(x => x.Value.Name == "Event_Icon_Late").Value;
            Events.Icon_Time = guild.Emojis.First(x => x.Value.Name == "Event_Icon_Time").Value;

            Events.Role_Range = guild.Emojis.First(x => x.Value.Name == "Event_Role_Range").Value;
            Events.Role_Tank = guild.Emojis.First(x => x.Value.Name == "Event_Role_Tank").Value;
            Events.Role_Healer = guild.Emojis.First(x => x.Value.Name == "Event_Role_Healer").Value;
            Events.Role_Melee = guild.Emojis.First(x => x.Value.Name == "Event_Role_Melee").Value;

            Events.Shaman_Resto = guild.Emojis.First(x => x.Value.Name == "Event_Shaman_Resto").Value;
            Events.Shaman_Enhance = guild.Emojis.First(x => x.Value.Name == "Event_Shaman_Enhance").Value;
            Events.Shaman_Ele = guild.Emojis.First(x => x.Value.Name == "Event_Shaman_Ele").Value;
            Events.Class_Shaman = guild.Emojis.First(x => x.Value.Name == "Event_Class_Shaman").Value;

            Events.Druid_Bear = guild.Emojis.First(x => x.Value.Name == "Event_Druid_Bear").Value;
            Events.Druid_Cat = guild.Emojis.First(x => x.Value.Name == "Event_Druid_Cat").Value;
            Events.Druid_Boomkin = guild.Emojis.First(x => x.Value.Name == "Event_Druid_Boomkin").Value;
            Events.Druid_Resto = guild.Emojis.First(x => x.Value.Name == "Event_Druid_Resto").Value;
            Events.Class_Druid = guild.Emojis.First(x => x.Value.Name == "Event_Class_Druid").Value;

            Events.Priest_Healer = guild.Emojis.First(x => x.Value.Name == "Event_Priest_Healer").Value;
            Events.Priest_Shadow = guild.Emojis.First(x => x.Value.Name == "Event_Priest_Shadow").Value;
            Events.Class_Priest = guild.Emojis.First(x => x.Value.Name == "Event_Class_Priest").Value;

            Events.Warrior_Prot = guild.Emojis.First(x => x.Value.Name == "Event_Warrior_Prot").Value;
            Events.Warrior_Dps = guild.Emojis.First(x => x.Value.Name == "Event_Warrior_Dps").Value;
            Events.Class_Warrior = guild.Emojis.First(x => x.Value.Name == "Event_Class_Warrior").Value;

            Events.Class_Warlock = guild.Emojis.First(x => x.Value.Name == "Event_Class_Warlock").Value;
            Events.Class_Rogue = guild.Emojis.First(x => x.Value.Name == "Event_Class_Rouge").Value;
            Events.Class_Hunter = guild.Emojis.First(x => x.Value.Name == "Event_Class_Hunter").Value;
            Events.Class_Mage = guild.Emojis.First(x => x.Value.Name == "Event_Class_Mage").Value;

            var emojis = GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DiscordEmoji))
                .Select(x => (DiscordEmoji)x.GetValue(this));

            foreach (var emoji in emojis)
            {
                m_emojis.Add(emoji.Id, emoji);
            }
        }

        public DiscordEmoji GetByDiscordId(ulong id)
        {
            return m_emojis[id];
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
