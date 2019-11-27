using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum WowItemQuality
    {
        Common = 1,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public class WowItem : BaseModel
    {
        public string Name { get; set; }

        public int WowItemId { get; set; }

        public WowItemQuality ItemQuality { get; set; }

        public string IconUrl { get; set; }

        public string WowHeadUrl { get; set; }
    }
}
