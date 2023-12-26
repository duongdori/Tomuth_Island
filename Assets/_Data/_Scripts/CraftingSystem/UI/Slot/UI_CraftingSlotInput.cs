using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.Crafting
{
    public class UI_CraftingSlotInput : MyMonobehaviour
    {
        [SerializeField] private UI_CraftingInfo craftingInfoUI;
        [SerializeField] private Image filter;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadCraftingInfoUI();
            LoadFilter();
            LoadIcon();
            LoadAmountText();
        }

        public void UpdateSlotInput(ItemCraftingSlot craftingSlot)
        {
            filter.enabled = craftingSlot != null &&
                             !craftingInfoUI.inventory.CheckEnoughItems(craftingSlot.itemData, craftingSlot.amount);
            
            icon.enabled = craftingSlot != null;
            amountText.enabled = craftingSlot != null;
            
            if(craftingSlot == null) return;
            icon.sprite = craftingSlot.itemData.itemIcon;
            amountText.SetText(craftingSlot.amount.ToString());
        }

        #region Load Components

        private void LoadCraftingInfoUI()
        {
            if(craftingInfoUI != null) return;
            craftingInfoUI = GetComponentInParent<UI_CraftingInfo>();
            Debug.LogWarning(transform.name + ": LoadCraftingInfoUI", gameObject);
        }
        private void LoadFilter()
        {
            if(filter != null) return;
            filter = transform.Find("Filter").GetComponent<Image>();
            filter.enabled = false;
            Debug.LogWarning(transform.name + ": LoadFilter", gameObject);
        }
        private void LoadIcon()
        {
            if(icon != null) return;
            icon = transform.Find("Icon").GetComponent<Image>();
            icon.enabled = false;
            Debug.LogWarning(transform.name + ": LoadIcon", gameObject);
        }
        
        private void LoadAmountText()
        {
            if(amountText != null) return;
            amountText = GetComponentInChildren<TextMeshProUGUI>();
            amountText.enabled = false;
            Debug.LogWarning(transform.name + ": LoadAmountText", gameObject);
        }

        #endregion
        
    }
}