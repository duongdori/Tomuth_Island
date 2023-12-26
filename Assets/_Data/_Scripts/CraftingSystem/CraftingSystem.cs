using System.Collections.Generic;
using DR.InventorySystem;
using UnityEngine;

namespace DR.Crafting
{
    public class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private HotbarInventory inventory;
        [SerializeField] private List<CraftingRecipeSO> craftingRecipes;

        public void Craft(CraftingRecipeSO craftingRecipe)
        {
            if(!CanCraftItem(craftingRecipe)) return;
            
            Crafting(craftingRecipe);
        }

        private void Crafting(CraftingRecipeSO craftingRecipe)
        {
            RemoveInputItem(craftingRecipe);
            
            inventory.AddToInventory(craftingRecipe.outputItem.itemData, craftingRecipe.outputItem.amount, craftingRecipe.outputItem.itemData.durabilityMax);
        }

        public bool CanCraftItem(CraftingRecipeSO craftingRecipe)
        {
            if (craftingRecipe == null) return false;
            if(!craftingRecipes.Contains(craftingRecipe)) return false;
            if(!CheckEnoughItemInput(craftingRecipe)) return false;
            if(!inventory.CanAddItemToInventory(craftingRecipe.outputItem.itemData, craftingRecipe.outputItem.amount)) return false;

            return true;
        }

        private bool CheckEnoughItemInput(CraftingRecipeSO craftingRecipe)
        {
            foreach (ItemCraftingSlot itemCraftingSlot in craftingRecipe.inputItemList)
            {
                if (!inventory.CheckEnoughItems(itemCraftingSlot.itemData, itemCraftingSlot.amount))
                {
                    return false;
                }
            }
            return true;
        }

        private void RemoveInputItem(CraftingRecipeSO craftingRecipe)
        {
            foreach (var itemCraftingSlot in craftingRecipe.inputItemList)
            {
                inventory.RemoveItemFromInventory(itemCraftingSlot.itemData, itemCraftingSlot.amount);
            }
        }
    }
}