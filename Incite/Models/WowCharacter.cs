﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public enum WowFaction
    {
        Horde,
        Alliance,
    }

    public class WowCharacter : BaseModel
    {
        public string Name { get; set; }

        public WowFaction WowFaction { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int WowClassId { get; set; }
        public virtual WowClass WowClass { get; set; }

        public int WowServerId { get; set; }
        public virtual WowServer WowServer { get; set; }

        public virtual ICollection<WowCharacterProfession> WowCharacterProfessions { get; set; } = new List<WowCharacterProfession>();

        public override string ToString()
        {
            return $"{Name}-{WowServer.Name}";
        }
    }
}
