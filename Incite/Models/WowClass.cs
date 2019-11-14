using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowClass : BaseModel
    {
        public string Name { get; set; }

        public static void Seed(ModelBuilder builder)
        {
            int nextId = 1;

            builder.Entity<WowClass>()
                .HasData(
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Warrior",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Rogue",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Hunter",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Mage",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Warlock",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Priest",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Druid",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Shaman",
                    },
                    new WowClass
                    {
                        Id = nextId++,
                        Name = "Paladin",
                    }
                );
        }
    }
}
