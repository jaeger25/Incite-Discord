﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class Member : BaseModel
    {
        public UInt64 DiscordUserId { get; set; }

        public string PrimaryCharacterName { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
