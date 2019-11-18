using DSharpPlus;
using DSharpPlus.Entities;
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

            AbsentEmoji = DiscordEmoji.FromGuildEmote(m_client, 612343589070045200);
            LateEmoji = DiscordEmoji.FromGuildEmote(m_client, 612373443551297689);
            RangedEmoji = DiscordEmoji.FromGuildEmote(m_client, 592446395596931072);
            TankEmoji = DiscordEmoji.FromGuildEmote(m_client, 598989638098747403);
            HealerEmoji = DiscordEmoji.FromGuildEmote(m_client, 592438128057253898);
            MeleeEmoji = DiscordEmoji.FromGuildEmote(m_client, 592440132129521664);
            RestoShamanEmoji = DiscordEmoji.FromGuildEmote(m_client, 606343376497016854);
            EnhanceEmoji = DiscordEmoji.FromGuildEmote(m_client, 597942199593730059);
            EleShamanEmoji = DiscordEmoji.FromGuildEmote(m_client, 597939529394946068);
            ShamanEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532030056857600);
            BearEmoji = DiscordEmoji.FromGuildEmote(m_client, 603627895956439079);
            CatEmoji = DiscordEmoji.FromGuildEmote(m_client, 597939529592078336);
            BoomkinEmoji = DiscordEmoji.FromGuildEmote(m_client, 597939529806249996);
            RestoDruidEmoji = DiscordEmoji.FromGuildEmote(m_client, 606343445803696157);
            DruidEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532029675438081);
            WarlockEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532029851336716);
            PriestEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532029901799437);
            ShadowEmoji = DiscordEmoji.FromGuildEmote(m_client, 597921879159734284);
            RogueEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532030086217748);
            HunterEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532029880827924);
            MageEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532030161977355);
            ProtEmoji = DiscordEmoji.FromGuildEmote(m_client, 580801859221192714);
            WarriorEmoji = DiscordEmoji.FromGuildEmote(m_client, 579532030153588739);

            var emojis = GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DiscordEmoji))
                .Select(x => (DiscordEmoji)x.GetValue(x));

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
