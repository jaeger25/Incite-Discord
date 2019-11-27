using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowItem : BaseModel
    {
        public string Name { get; set; }

        public int WowItemId { get; set; }
    }
}
