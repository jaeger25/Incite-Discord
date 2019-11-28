using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowReagent : BaseModel
    {
        public int Count { get; set; }

        public int WowItemId { get; set; }
        public virtual WowItem WowItem { get; set; }

        public int WowSpellId { get; set; }
        public virtual WowSpell WowSpell { get; set; }
    }
}
