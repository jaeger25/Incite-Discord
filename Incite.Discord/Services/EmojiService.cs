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

        public EmojiService(DiscordClient client)
        {
            m_client = client;

            AbsentEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482817134593];
            LateEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482884374535];
            RangedEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482892632104];
            TankEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482850689024];
            HealerEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482733248512];
            MeleeEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482657619979];
            RestoShamanEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155483177975824];
            EnhanceEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482905214976];
            EleShamanEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482879918090];
            ShamanEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482678591498];
            BearEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482942963722];
            CatEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482900889610];
            BoomkinEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482888306688];
            RestoDruidEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482825654292];
            DruidEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482389446676];
            WarlockEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482800357396];
            PriestEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482477527041];
            ShadowEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482770866176];
            RogueEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482716602399];
            HunterEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482704019486];
            MageEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482678722570];
            ProtEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482301235211];
            WarriorEmoji = m_client.Guilds[Guild.InciteDiscordId].Emojis[637155482301104129];

            var emojis = GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DiscordEmoji))
                .Select(x => (DiscordEmoji)x.GetValue(this));

            foreach (var emoji in emojis)
            {
                m_emojis.Add(emoji.Id, emoji);
            }
        }

        public DiscordEmoji AbsentEmoji { get; }
        public DiscordEmoji LateEmoji { get; }
        public DiscordEmoji RangedEmoji { get; }
        public DiscordEmoji TankEmoji { get; }
        public DiscordEmoji HealerEmoji { get; }
        public DiscordEmoji MeleeEmoji { get; }
        public DiscordEmoji RestoShamanEmoji { get; }
        public DiscordEmoji EnhanceEmoji { get; }
        public DiscordEmoji EleShamanEmoji { get; }
        public DiscordEmoji ShamanEmoji { get; }
        public DiscordEmoji BearEmoji { get; }
        public DiscordEmoji CatEmoji { get; }
        public DiscordEmoji BoomkinEmoji { get; }
        public DiscordEmoji RestoDruidEmoji { get; }
        public DiscordEmoji DruidEmoji { get; }
        public DiscordEmoji WarlockEmoji { get; }
        public DiscordEmoji PriestEmoji { get; }
        public DiscordEmoji ShadowEmoji { get; }
        public DiscordEmoji RogueEmoji { get; }
        public DiscordEmoji HunterEmoji { get; }
        public DiscordEmoji MageEmoji { get; }
        public DiscordEmoji ProtEmoji { get; }
        public DiscordEmoji WarriorEmoji { get; }

        public DiscordEmoji GetByDiscordId(ulong id)
        {
            return m_emojis[id];
        }

        public IEnumerable<DiscordEmoji> WarriorEmojis()
        {
            return new[] { ProtEmoji, WarriorEmoji };
        }

        public IEnumerable<DiscordEmoji> MageEmojis()
        {
            return new[] { MageEmoji };
        }

        public IEnumerable<DiscordEmoji> HunterEmojis()
        {
            return new[] { HunterEmoji };
        }

        public IEnumerable<DiscordEmoji> RogueEmojis()
        {
            return new[] { RogueEmoji };
        }

        public IEnumerable<DiscordEmoji> PriestEmojis()
        {
            return new[] { PriestEmoji, ShadowEmoji };
        }

        public IEnumerable<DiscordEmoji> WarlockEmojis()
        {
            return new[] { WarlockEmoji };
        }

        public IEnumerable<DiscordEmoji> DruidEmojis()
        {
            return new[] { DruidEmoji, RestoDruidEmoji, BoomkinEmoji, CatEmoji, BearEmoji };
        }

        public IEnumerable<DiscordEmoji> ShamanEmojis()
        {
            return new[] { ShamanEmoji, RestoShamanEmoji, EleShamanEmoji, EnhanceEmoji };
        }

        public IEnumerable<DiscordEmoji> TankEmojis()
        {
            return new[] { ProtEmoji, BearEmoji };
        }

        public IEnumerable<DiscordEmoji> MeleeEmojis()
        {
            return new[] { RogueEmoji, WarriorEmoji, CatEmoji, EnhanceEmoji };
        }

        public IEnumerable<DiscordEmoji> RangeEmojis()
        {
            return new[] { EleShamanEmoji, BoomkinEmoji, WarlockEmoji, MageEmoji, ShadowEmoji, HunterEmoji };
        }

        public IEnumerable<DiscordEmoji> HealerEmojis()
        {
            return new[] { RestoShamanEmoji, RestoDruidEmoji, PriestEmoji };
        }
    }
}
