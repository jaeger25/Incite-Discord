using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowCharacterProfession : BaseModel
    {
        public int WowCharacterId { get; set; }
        public virtual WowCharacter WowCharacter { get; set; }

        public int WowProfessionId { get; set; }
        public virtual WowProfession WowProfession { get; set; }

        public virtual ICollection<WowCharacterRecipe> WowCharacterRecipes { get; set; } = new List<WowCharacterRecipe>();
    }
}
