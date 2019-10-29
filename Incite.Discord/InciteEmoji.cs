using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord
{
    public static class InciteEmoji
    {
        public static readonly string Absent = ":Absent:";
        public static readonly string Late = ":Late:";
        public static readonly string Ranged = ":Ranged:";
        public static readonly string Tank = ":Tank:";
        public static readonly string Healer = ":Healer:";
        public static readonly string Melee = ":Melee:";
        public static readonly string RestoShaman = ":RestoShaman:";
        public static readonly string Bear = ":Bear:";
        public static readonly string Enhance = ":Enhance:";
        public static readonly string Cat = ":Cat:";
        public static readonly string Boomkin = ":Boomkin:";
        public static readonly string EleShaman = ":EleShaman:";
        public static readonly string RestoDruid = ":RestoDruid:";
        public static readonly string Warlock = ":Warlock:";
        public static readonly string Shadow = ":Shadow:";
        public static readonly string Rouge = ":Rouge:";
        public static readonly string Hunter = ":Hunter:";
        public static readonly string Mage = ":Mage:";
        public static readonly string Shaman = ":Shaman:";
        public static readonly string Bench = ":Bench:";
        public static readonly string Priest = ":Priest:";
        public static readonly string Druid = ":Druid:";
        public static readonly string Prot = ":Prot:";
        public static readonly string Warrior = ":Warrior:";

        public static IEnumerable<string> GetEventReactionEmojis()
        {
            return new List<string>()
            {
                Prot,
                Warrior,
                Rouge,
                Hunter,
                Mage,
                Warlock,
                Priest,
                Enhance,
                EleShaman,
                RestoShaman,
                Bear,
                Cat,
                RestoDruid,
                Late,
                Bench,
                Absent,
            };
        }
    }
}
