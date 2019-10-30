using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord
{
    public static class InciteEmoji
    {
        public const string Absent = ":Absent:";
        public const string Late = ":Late:";
        public const string Ranged = ":Ranged:";
        public const string Tank = ":Tank:";
        public const string Healer = ":Healer:";
        public const string Melee = ":Melee:";
        public const string RestoShaman = ":RestoShaman:";
        public const string Bear = ":Bear:";
        public const string Enhance = ":Enhance:";
        public const string Cat = ":Cat:";
        public const string Boomkin = ":Boomkin:";
        public const string EleShaman = ":EleShaman:";
        public const string RestoDruid = ":RestoDruid:";
        public const string Warlock = ":Warlock:";
        public const string Shadow = ":Shadow:";
        public const string Rouge = ":Rouge:";
        public const string Hunter = ":Hunter:";
        public const string Mage = ":Mage:";
        public const string Shaman = ":Shaman:";
        public const string Bench = ":Bench:";
        public const string Priest = ":Priest:";
        public const string Druid = ":Druid:";
        public const string Prot = ":Prot:";
        public const string Warrior = ":Warrior:";

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
