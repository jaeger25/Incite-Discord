using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public enum MemberStatus
    {
        Unconfirmed,
        Pending,
        Confirmed
    }

    public class Member : BaseModel
    {
        public MemberStatus Status { get; set; }

        public int? PrimaryWowCharacterId { get; set; }
        public virtual WowCharacter? PrimaryWowCharacter { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
        public virtual ICollection<MemberEvent> MemberEvents { get; set; } = new List<MemberEvent>();
    }
}
