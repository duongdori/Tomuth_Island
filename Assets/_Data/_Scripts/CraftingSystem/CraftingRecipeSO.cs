using System;
using System.Collections.Generic;
using DR.InventorySystem;
using UnityEngine;

namespace DR.Crafting
{
    [Serializable]
    public class ItemCraftingSlot
    {
        public ItemData itemData;
        public int amount;
    }
    [CreateAssetMenu(menuName = "SO/Crafting/Crafting Recipe", fileName = "New Crafting Recipe")]
    public class CraftingRecipeSO : ScriptableObject
    {
        public List<ItemCraftingSlot> inputItemList;
        public ItemCraftingSlot outputItem;

        public float craftingTime;
    }
}
