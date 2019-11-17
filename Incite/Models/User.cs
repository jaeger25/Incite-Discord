using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class User : BaseModel
    {
        public UInt64 DiscordId { get; set; }
    }
}
