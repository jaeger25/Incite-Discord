using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum RoleKind
    {
        Everyone,
        Member,
        Officer,
        Leader,
    }

    public class Role : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public RoleKind Kind { get; set; }
    }
}
