using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class Event : BaseModel
    {
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public virtual ICollection<EventMember> EventMembers { get; set; }
    }
}
