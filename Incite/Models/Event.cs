using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class Event : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public int EventMessageId { get; set; }
        public virtual Message EventMessage { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int OwnerId { get; set; }
        public virtual Member Owner { get; set; }

        public virtual ICollection<EventMember> EventMembers { get; set; } = new List<EventMember>();
    }
}
