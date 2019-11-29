using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowSpell : BaseModel
    {
        public string Name { get; set; }

        public int WowId { get; set; }

        public int? CreatedItemId { get; set; }
        public virtual WowItem? CreatedItem { get; set; }

        public virtual ICollection<WowReagent> WowReagents { get; set; } = new List<WowReagent>();
    }
}
