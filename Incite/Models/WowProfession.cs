using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowProfession : BaseModel
    {
        public string Name { get; set; }

        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<WowProfession>()
                .HasData(
                    new WowProfession()
                    {
                        Name = "First Aid"
                    },
                    new WowProfession()
                    {
                        Name = "Fishing"
                    },
                    new WowProfession()
                    {
                        Name = "Cooking"
                    },
                    new WowProfession()
                    {
                        Name = "Alchemy"
                    },
                    new WowProfession()
                    {
                        Name = "Blacksmithing"
                    },
                    new WowProfession()
                    {
                        Name = "Enchanting"
                    },
                    new WowProfession()
                    {
                        Name = "Engineering"
                    },
                    new WowProfession()
                    {
                        Name = "Leatherworking"
                    },
                    new WowProfession()
                    {
                        Name = "Tailoring"
                    },
                    new WowProfession()
                    {
                        Name = "Herbalism"
                    },
                    new WowProfession()
                    {
                        Name = "Mining"
                    },
                    new WowProfession()
                    {
                        Name = "Skinning"
                    },
                    new WowProfession()
                    {
                        Name = "Lockpicking"
                    }
                );
        }
    }
}
