using System.Linq;
using System.Threading.Tasks;
using DR.InputSystem;
using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DR.InventorySystem
{
    public class HotbarInventory : InventorySystem
    {
        public event UnityAction<InventorySlot> OnInventorySlotActiveChanged;

        protected override void Start()
        {
            base.Start();
            InputHandler.OnSlotActive += OnSlotActive;
        }

        private void OnDestroy()
        {
            InputHandler.OnSlotActive -= OnSlotActive;
        }

        private void OnSlotActive(int index)
        {
            if(inventorySlots[index].IsEmptySlot()) return;

            InventorySlot[] slotActives = inventorySlots.Where(s => s.IsActive).ToArray();
            
            foreach (InventorySlot slotActive in slotActives)
            {
                if (slotActive.ItemData.isLeftHand == inventorySlots[index].ItemData.isLeftHand)
                {
                    if (slotActive != inventorySlots[index])
                    {
                        slotActive.IsActive = false;
                        break;
                    }
                    else
                    {
                        slotActive.IsActive = !slotActive.IsActive;
                        OnInventorySlotActiveChanged?.Invoke(slotActive);
                        return;
                    }
                }
            }
            
            inventorySlots[index].IsActive = !inventorySlots[index].IsActive;
            OnInventorySlotActiveChanged?.Invoke(inventorySlots[index]);
        }

        protected override void InventoryItemRemaining(ItemData itemData, int amount, float durability)
        {
            base.InventoryItemRemaining(itemData, amount, durability);
            PlayerController.Instance.backpackInventory.AddToInventory(itemData, amount, durability);
        }

        public bool CheckEnoughItems(ItemData itemData, int amount)
        {
            if (IsEnoughItems(itemData, amount, out int count)) return true;

            return PlayerController.Instance.backpackInventory.IsEnoughItems(itemData, amount - count, out int c);
        }

        public bool RemoveItemFromInventory(ItemData itemData, int amount)
        {
            if (IsEnoughItems(itemData, amount, out int count))
            {
                RemoveItem(itemData, amount);
                return true;
            }
            
            if (PlayerController.Instance.backpackInventory.IsEnoughItems(itemData, amount - count, out int c))
            {
                RemoveItem(itemData, count);
                PlayerController.Instance.backpackInventory.RemoveItems(itemData, amount - count);
                return true;
            }
            
            Debug.LogWarning("Cannot Remove " + itemData.itemName);
            
            return false;
        }

        public bool CanAddItemToInventory(ItemData itemData, int amount)
        {
            return CanAddItem(itemData, amount) || PlayerController.Instance.backpackInventory.CanAddItem(itemData, amount);
        }

        public bool IsEnoughSlotToAddItems(ItemData itemData, int amount)
        {
            int slotCount = Mathf.CeilToInt((float)amount / itemData.maxStackSize);
            int totalEmptySlot = GetEmptySlotCount() + PlayerController.Instance.backpackInventory.GetEmptySlots();

            if (totalEmptySlot < slotCount)
            {
                Debug.LogWarning("Cannot Add");
                return false;
            }
            
            Debug.Log("Can add");
            return true;
        }

        public async Task SaveData()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemSlot itemSlot = new ItemSlot(inventorySlots[i].ItemData, inventorySlots[i].StackSize, inventorySlots[i].CurrentDurability);
                string itemSlotJson = JsonUtility.ToJson(itemSlot);
                await CloudSaveManager.ForceSaveSingleData("HotBarItemSlot" + i, itemSlotJson);
            }
        }

        public async Task LoadData()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                var itemSlotString = await CloudSaveManager.RetrieveSpecificData<string>("HotBarItemSlot" + i);
                if(itemSlotString == null) return;
                ItemSlot itemSlot = JsonUtility.FromJson<ItemSlot>(itemSlotString);
                inventorySlots[i].UpdateInventorySlot(itemSlot.ItemData, itemSlot.StackSize, itemSlot.Durability, false);
            }
        }
    }

    
}