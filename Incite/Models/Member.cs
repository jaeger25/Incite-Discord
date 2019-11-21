using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Member : BaseModel
    {
        public string PrimaryCharacterName { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
        public virtual ICollection<MemberEvent> MemberEvents { get; set; } = new List<MemberEvent>();
    }
}
