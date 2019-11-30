using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class WowCharacterRecipe : BaseModel
    {
        public int WowCharacterProfessionId { get; set; }
        public virtual WowCharacterProfession WowCharacterProfession { get; set; }

        public int RecipeId { get; set; }
        public virtual WowItem Recipe { get; set; }
    }
}
