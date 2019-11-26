using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowClass : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<WowCharacter> WowCharacters { get; set; } = new List<WowCharacter>();

        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<WowClass>()
                .HasData(
                    new WowClass
                    {
                        Id = 1,
                        Name = "Warrior",
                    },
                    new WowClass
                    {
                        Id = 2,
                        Name = "Rogue",
                    },
                    new WowClass
                    {
                        Id = 3,
                        Name = "Hunter",
                    },
                    new WowClass
                    {
                        Id = 4,
                        Name = "Mage",
                    },
                    new WowClass
                    {
                        Id = 5,
                        Name = "Warlock",
                    },
                    new WowClass
                    {
                        Id = 6,
                        Name = "Priest",
                    },
                    new WowClass
                    {
                        Id = 7,
                        Name = "Druid",
                    },
                    new WowClass
                    {
                        Id = 8,
                        Name = "Shaman",
                    },
                    new WowClass
                    {
                        Id = 9,
                        Name = "Paladin",
                    }
                );
        }
    }
}
