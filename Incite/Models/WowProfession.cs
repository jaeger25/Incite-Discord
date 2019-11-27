using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowProfession : BaseModel
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static void Seed(ModelBuilder builder)
        {
            int nextId = 1;

            builder.Entity<WowProfession>()
                .HasData(
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "FirstAid"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Fishing"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Cooking"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Alchemy"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Blacksmithing"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Enchanting"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Engineering"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Leatherworking"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Tailoring"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Herbalism"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Mining"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Skinning"
                    },
                    new WowProfession()
                    {
                        Id = nextId++,
                        Name = "Lockpicking"
                    }
                );
        }
    }
}
