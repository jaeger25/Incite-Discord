using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowItemSubclass : BaseModel
    {
        public string Name { get; set; }

        public int WowId { get; set; }

        public int WowItemClassId { get; set; }
        public virtual WowItemClass WowItemClass { get; set; }

        public virtual ICollection<WowItem> WowItems { get; set; } = new List<WowItem>();
    }
}
