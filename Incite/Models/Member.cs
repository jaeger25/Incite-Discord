using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Member : BaseModel
    {
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Event> OwnedEvents { get; set; } = new List<Event>();
    }
}
