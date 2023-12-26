using System.Collections.Generic;
using UnityEngine;

namespace DR.Crafting
{
    [CreateAssetMenu(menuName = "SO/Crafting/Recipe Group", fileName = "New Recipe Group")]
    public class CraftingRecipeGroup : ScriptableObject
    {
        public string groupTitle;
        public Sprite groupIcon;
        public List<CraftingRecipeSO> craftingRecipeList;
    }
}