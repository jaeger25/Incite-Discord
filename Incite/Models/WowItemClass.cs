using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowItemClass : BaseModel
    {
        public string Name { get; set; }

        public int WowId { get; set; }

        public virtual ICollection<WowItemSubclass> WowItemSubclasses { get; set; } = new List<WowItemSubclass>();
        public virtual ICollection<WowItem> WowItems { get; set; } = new List<WowItem>();
    }
}
