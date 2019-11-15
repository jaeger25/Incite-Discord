using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum ChannelKind
    {
        Unspecified,
        Admin,
    }

    public class Channel : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public ChannelKind Kind { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
