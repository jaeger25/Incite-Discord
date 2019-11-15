using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class Member : BaseModel
    {
        public UInt64 DiscordId { get; set; }

        public string PrimaryCharacterName { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
