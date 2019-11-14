using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Guild : BaseModel
    {
        public UInt64 DiscordGuildId { get; set; }
    }
}
