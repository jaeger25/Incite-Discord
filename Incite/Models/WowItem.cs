using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum WowItemQuality
    {
        Poor,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public class WowItem : BaseModel
    {
        public string Name { get; set; }

        public int WowId { get; set; }

        public WowItemQuality ItemQuality { get; set; }

        public string WowHeadIcon { get; set; }

        public int WowItemClassId { get; set; }
        public virtual WowItemClass WowItemClass { get; set; }

        public int WowItemSubclassId { get; set; }
        public virtual WowItemSubclass WowItemSubclass { get; set; }

        public virtual ICollection<WowSpell> CreatedBy { get; set; } = new List<WowSpell>();
    }
}