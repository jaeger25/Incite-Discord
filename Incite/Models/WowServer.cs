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
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Nethergarde Keep",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Pyrewood Village",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Auberdine",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Everlook",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Razorfen",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Lakeshire",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Chromie",
                        UtcOffset = TimeSpan.FromHours(1)
                    },

                    // Australian PVE
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Remulos",
                        UtcOffset = TimeSpan.FromHours(11)
                    },

                    // America PVP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Anathema",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Arcanite Reaper",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Bigglesworth",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Benediction",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Blaumeux",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Earthfury",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Faerlina",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Fairbanks",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Heartseeker",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Herod",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Incendius",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Kurinaxx",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Kromcrush",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Kurinnaxx",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Loatheb",
                        UtcOffset = TimeSpan.FromHours(-4) // TODO: Mexico
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Netherwind",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Rattlegore",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Skeram",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Smolderweb",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Stalagg",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Sul'thraze",
                        UtcOffset = TimeSpan.FromHours(-8) // TODO: Brazil
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Sulfuras",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Thalnos",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Thunderfury",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Whitemane",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },

                    // EU PVP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Ashbringer",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Bloodfang",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Dreadmist",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Earthshaker",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Firemaw",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Flamelash",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Gandling",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Gehennas",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Golemagg",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Judgement",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Mograine",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Noggenfogger",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Razorgore",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Shazzrah",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Skullflame",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Stonespine",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Ten Storms",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Amnennar",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Sulfuron",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Finkle",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Heartstriker",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Lucifron",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Venoxis",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Patchwerk",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Dragon's Call",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Transcendence",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Harbinger of Doom",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Flamegor",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Wyrmthalak",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Rhok’delar",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Mandokir",
                        UtcOffset = TimeSpan.FromHours(1)
                    },

                    // Australian PVP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Arugal",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Felstriker",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Yojamba",
                        UtcOffset = TimeSpan.FromHours(11)
                    },

                    // Asia PvP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Hillsbrad",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Iceblood",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Lokholar",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Ragnaros",
                        UtcOffset = TimeSpan.FromHours(11)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Ivas",
                        UtcOffset = TimeSpan.FromHours(11)
                    },

                    // America RP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Bloodsail Buccaneers",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Deviate Delight",
                        UtcOffset = TimeSpan.FromHours(-5)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Grobbulus",
                        UtcOffset = TimeSpan.FromHours(-8)
                    },

                    // Europe RP
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Hydraxian Waterlords",
                        UtcOffset = TimeSpan.FromHours(1)
                    },
                    new WowServer
                    {
                        Id = nextId++,
                        Name = "Zandalar Tribe",
                        UtcOffset = TimeSpan.FromHours(1)
                    });
        }
    }
}
