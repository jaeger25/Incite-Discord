using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum RoleKind
    {
        Everyone,
        Member,
        Officer
    }

    public class Role : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public RoleKind Kind { get; set; }
    }
}
