using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class Channel : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
