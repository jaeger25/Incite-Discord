using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowServer : BaseModel
    {
        public string Name { get; set; }

        public TimeSpan UtcOffset { get; set; }

        public virtual ICollection<WowCharacter> WowCharacters { get; set; } = new List<WowCharacter>();

        public override string ToString()
        {
            return Name;
        }

        public static void Seed(ModelBuilder builder)
        {
            int nextId = 1;

            builder.Entity<WowServer>()
                .HasData(
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Kirtonos",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },

                    // America PVE
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Ashkandi",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Atiesh",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Azuresong",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Mankrik",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Myzrael",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Old Blanchy",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Pagle",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Westfall",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Windseeker",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },

                    // Europe PVE
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Mirage Raceway",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Nethergarde Keep",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Pyrewood Village",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Auberdine",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Everlook",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Razorfen",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Lakeshire",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Chromie",
                        UtcOffset = TimeSpan.FromHours(-5)
                    }
                    );
        }
    }
}
