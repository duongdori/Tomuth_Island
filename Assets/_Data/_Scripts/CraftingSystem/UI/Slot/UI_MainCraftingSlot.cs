using UnityEngine;

namespace DR.Crafting
{
    public class UI_MainCraftingSlot : UI_CraftingSlot
    {
        [SerializeField] private CraftingRecipeGroup recipeGroup;
        [SerializeField] private UI_MainCrafting mainCraftingUI;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadMainCraftingUI();
            if (recipeGroup != null && recipeGroup.groupIcon != null)
                icon.sprite = recipeGroup.groupIcon;

            if (highlight)
                highlight.enabled = false;
            isVisible = false;
        }

        protected override void OnClick()
        {
            base.OnClick();
            mainCraftingUI.DisableCraftingSlotHighlight(this);
            SetHighlight(!isVisible);
            if (isVisible)
            {
                mainCraftingUI.subCraftingUI.gameObject.SetActive(true);
                mainCraftingUI.subCraftingUI.craftingInfoUI.gameObject.SetActive(false);
                mainCraftingUI.subCraftingUI.SetRecipeGroup(recipeGroup);
                mainCraftingUI.subCraftingUI.DisableCraftingSlotHighlight(null);
            }
            else
            {
                mainCraftingUI.subCraftingUI.SetVisible();
            }
            
        }

        private void LoadMainCraftingUI()
        {
            if(mainCraftingUI != null) return;
            mainCraftingUI = GetComponentInParent<UI_MainCrafting>();
            Debug.LogWarning(transform.name + ": LoadMainCraftingUI", gameObject);
        }
    }
}