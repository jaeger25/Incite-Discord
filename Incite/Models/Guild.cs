using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Guild : BaseModel
    {
        public const ulong InciteBotDiscordId = 637168144732913675;

        public UInt64 DiscordId { get; set; }

        public WowFaction? WowFaction { get; set; }

        public int? WowServerId { get; set; }
        public virtual WowServer? WowServer { get; set; }

        public int? OptionsId { get; set; }
        public virtual GuildOptions Options { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public virtual ICollection<WowCharacter> WowCharacters { get; set; } = new List<WowCharacter>();

        public virtual ICollection<EpgpSnapshot> EpgpSnapshots { get; set; } = new List<EpgpSnapshot>();
    }
}
