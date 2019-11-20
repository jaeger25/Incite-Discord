using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Guild : BaseModel
    {
        public const ulong InciteDiscordId = 407066938078920705;
        public const ulong InciteTestDiscordId = 637168144732913675;

        public UInt64 DiscordId { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

        public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
