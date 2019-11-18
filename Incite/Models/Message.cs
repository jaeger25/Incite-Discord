using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class Message : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
