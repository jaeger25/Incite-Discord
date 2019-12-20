using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class EpgpSnapshot : BaseModel
    {
        public DateTimeOffset DateTime { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public virtual ICollection<EpgpStanding> Standings { get; set; } = new List<EpgpStanding>();
    }
}
