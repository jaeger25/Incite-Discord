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
            builder.Entity<WowClass>()
                .HasData(
                    new WowClass
                    {
                        Name = "Warrior",
                    },
                    new WowClass
                    {
                        Name = "Rogue",
                    },
                    new WowClass
                    {
                        Name = "Hunter",
                    },
                    new WowClass
                    {
                        Name = "Mage",
                    },
                    new WowClass
                    {
                        Name = "Warlock",
                    },
                    new WowClass
                    {
                        Name = "Priest",
                    },
                    new WowClass
                    {
                        Name = "Druid",
                    },
                    new WowClass
                    {
                        Name = "Shaman",
                    },
                    new WowClass
                    {
                        Name = "Paladin",
                    }
                );
        }
    }
}
