using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class EventMessage : BaseModel
    {
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}
