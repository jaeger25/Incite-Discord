using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class MemberEvent : BaseModel
    {
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
