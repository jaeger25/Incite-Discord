using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.Services
{
    public class EmojiService
    {
        readonly DiscordClient m_client;

        public EmojiService(DiscordClient client)
        {
            m_client = client;

            AbsentEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 612343589070045200));
            LateEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 612373443551297689));
            RangedEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 592446395596931072));
            TankEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 598989638098747403));
            HealerEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 592438128057253898));
            MeleeEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 592440132129521664));
            RestoShamanEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 606343376497016854));
            EnhanceEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 597942199593730059));
            EleShamanEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 597939529394946068));
            ShamanEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532030056857600));
            BearEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 603627895956439079));
            CatEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 597939529592078336));
            BoomkinEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 597939529806249996));
            RestoDruidEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 606343445803696157));
            DruidEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532029675438081));
            WarlockEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532029851336716));
            PriestEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532029901799437));
            ShadowEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 597921879159734284));
            RougeEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532030086217748));
            HunterEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532029880827924));
            MageEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532030161977355));
            ProtEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 580801859221192714));
            WarriorEmoji = new Lazy<DiscordEmoji>(() => DiscordEmoji.FromGuildEmote(m_client, 579532030153588739));
        }

        public Lazy<DiscordEmoji> AbsentEmoji { get; }
        public Lazy<DiscordEmoji> LateEmoji { get; }
        public Lazy<DiscordEmoji> RangedEmoji { get; }
        public Lazy<DiscordEmoji> TankEmoji { get; }
        public Lazy<DiscordEmoji> HealerEmoji { get; }
        public Lazy<DiscordEmoji> MeleeEmoji { get; }
        public Lazy<DiscordEmoji> RestoShamanEmoji { get; }
        public Lazy<DiscordEmoji> EnhanceEmoji { get; }
        public Lazy<DiscordEmoji> EleShamanEmoji { get; }
        public Lazy<DiscordEmoji> ShamanEmoji { get; }
        public Lazy<DiscordEmoji> BearEmoji { get; }
        public Lazy<DiscordEmoji> CatEmoji { get; }
        public Lazy<DiscordEmoji> BoomkinEmoji { get; }
        public Lazy<DiscordEmoji> RestoDruidEmoji { get; }
        public Lazy<DiscordEmoji> DruidEmoji { get; }
        public Lazy<DiscordEmoji> WarlockEmoji { get; }
        public Lazy<DiscordEmoji> PriestEmoji { get; }
        public Lazy<DiscordEmoji> ShadowEmoji { get; }
        public Lazy<DiscordEmoji> RougeEmoji { get; }
        public Lazy<DiscordEmoji> HunterEmoji { get; }
        public Lazy<DiscordEmoji> MageEmoji { get; }
        public Lazy<DiscordEmoji> ProtEmoji { get; }
        public Lazy<DiscordEmoji> WarriorEmoji { get; }
    }
}
