using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.InventorySystem
{
    public class UI_InventorySlot : MyMonobehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private Image activeIndicator;
        [SerializeField] private Image durabilityBar;
        [SerializeField] private int inventorySlotIndex;
        [SerializeField] private InventorySlot assignedSlot;
        public InventorySlot AssignedSlot => assignedSlot;

        private InventorySwapping _inventorySwapping;

        [SerializeField] private Button button;
        [SerializeField] private InventorySystem inventory;

        protected override void Awake()
        {
            base.Awake();

            _inventorySwapping = GetComponentInParent<InventorySwapping>();
            
            button.onClick.AddListener(OnUISlotClick);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadItemIcon();
            LoadItemCount();
            LoadActiveIndicator();
            LoadDurabilityBar();
            LoadButton();
        }

        public void AssignSlot(int slotIndex)
        {
            if (assignedSlot != null) assignedSlot.OnInventorySlotChanged -= OnInventorySlotChanged;
            inventorySlotIndex = slotIndex;
            if (inventory == null) inventory = GetComponentInParent<UI_Inventory>().Inventory;
            assignedSlot = inventory.InventorySlots[inventorySlotIndex];
            assignedSlot.OnInventorySlotChanged += OnInventorySlotChanged;
            UpdateViewState(assignedSlot.ItemData, assignedSlot.StackSize, assignedSlot.CurrentDurability, assignedSlot.IsActive);
        }

        private void OnInventorySlotChanged(object sender, InventorySlotChangedArgs args)
        {
            UpdateViewState(args.ItemData, args.StackSize, args.CurrentDurability, args.IsActive);
        }

        private void UpdateViewState(ItemData itemData, int stackSize, float durability, bool active)
        {
            activeIndicator.enabled = active;
            bool hasItem = itemData != null;
            bool isStackable = hasItem && itemData.isStackable && stackSize > 1;
            itemIcon.enabled = hasItem;
            itemCount.enabled = isStackable;
            durabilityBar.enabled = hasItem && durability < itemData.durabilityMax;
            
            if(!hasItem) return;
            itemIcon.sprite = itemData.itemIcon;
            durabilityBar.fillAmount = durability / itemData.durabilityMax;
            if(isStackable) itemCount.SetText(stackSize.ToString());
        }

        private void OnUISlotClick()
        {
            _inventorySwapping.SlotClicked(this);
        }

        #region Load Components

        private void LoadItemIcon()
        {
            if(itemIcon != null) return;
            itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadItemIcon", gameObject);
        }
        
        private void LoadItemCount()
        {
            if(itemCount != null) return;
            itemCount = GetComponentInChildren<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadItemCount", gameObject);
        }
        
        private void LoadActiveIndicator()
        {
            if(activeIndicator != null) return;
            activeIndicator = transform.Find("Indicator").GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadActiveIndicator", gameObject);
        }
        private void LoadDurabilityBar()
        {
            if(durabilityBar != null) return;
            durabilityBar = transform.Find("DurabilityBar").GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadDurabilityBar", gameObject);
        }
        
        private void LoadButton()
        {
            if(button != null) return;
            button = GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadButton", gameObject);
        }

        #endregion
        
    }
}

