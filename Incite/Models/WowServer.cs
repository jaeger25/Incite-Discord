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
            builder.Entity<WowServer>()
                .HasData(
                    new WowServer
                    {
                        Id = 1,
                        Name = "Kirtonos",
                        UtcOffset = TimeSpan.FromHours(-5)
                    });
        }
    }
}
