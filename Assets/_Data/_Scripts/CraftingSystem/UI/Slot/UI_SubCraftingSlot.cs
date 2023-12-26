using UnityEngine;

namespace DR.Crafting
{
    public class UI_SubCraftingSlot : UI_CraftingSlot
    {
        [SerializeField] private UI_SubCrafting subCraftingUI;
        [SerializeField] private CraftingRecipeSO recipeSO;
        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadSubCraftingUI();
            if (highlight)
                highlight.enabled = false;
            isVisible = false;
        }

        protected override void OnClick()
        {
            base.OnClick();
            subCraftingUI.DisableCraftingSlotHighlight(this);
            SetHighlight(!isVisible);
            
            if (isVisible)
            {
                subCraftingUI.craftingInfoUI.gameObject.SetActive(recipeSO != null);
                if(recipeSO == null) return;
                subCraftingUI.craftingInfoUI.UpdateCraftingInfo(recipeSO);
            }
            else
            {
                subCraftingUI.craftingInfoUI.gameObject.SetActive(false);
            }
        }
        
        public void UpdateCraftingSlot(CraftingRecipeSO craftingRecipeSO)
        {
            recipeSO = craftingRecipeSO;
            icon.enabled = craftingRecipeSO != null;
            if(craftingRecipeSO == null) return;
            icon.sprite = craftingRecipeSO.outputItem.itemData.itemIcon;
        }
        
        private void LoadSubCraftingUI()
        {
            if(subCraftingUI != null) return;
            subCraftingUI = GetComponentInParent<UI_SubCrafting>();
            Debug.LogWarning(transform.name + ": LoadSubCraftingUI", gameObject);
        }
    }
}