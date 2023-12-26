using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DR.InventorySystem
{
    public abstract class InventorySystem : MonoBehaviour
    {
        [SerializeField] private int size;
        public int Size => size;

        [SerializeField] protected List<InventorySlot> inventorySlots;
        public List<InventorySlot> InventorySlots => inventorySlots;

        protected virtual void Start()
        {
            
        }
        
        [ContextMenu("Adjust Size")]
        private void AdjustSize()
        {
            if (inventorySlots == null) inventorySlots = new List<InventorySlot>();
            if (inventorySlots.Count > size) inventorySlots.RemoveRange(size, inventorySlots.Count - size);
            if(inventorySlots.Count < size) inventorySlots.AddRange(new InventorySlot[size - inventorySlots.Count]);
        }

        public bool CanAddItem(ItemData itemData, int amount)
        {
            InventorySlot slot =
                inventorySlots.FirstOrDefault(s => s.ItemData == itemData && s.EnoughRoomLeftInStack(amount));
            return HasFreeSlot(out InventorySlot freeSlot) || slot != null;
        }
        public bool CanAddItem(ItemData itemData)
        {
            InventorySlot slotWithStackableItem = FindSlot(itemData);
            return HasFreeSlot(out InventorySlot freeSlot) || slotWithStackableItem != null;
        }
        private InventorySlot FindSlot(ItemData itemData)
        {
            return inventorySlots.FirstOrDefault(slot => slot.ItemData == itemData && slot.CanAddAmountToStackSize());
        }
        private bool HasFreeSlot(out InventorySlot freeSlot)
        {
            freeSlot = inventorySlots.FirstOrDefault(slot => slot.IsEmptySlot());

            return freeSlot != null;
        }
        
        public void AddToInventory(ItemData itemToAdd, int amountToAdd, float durability)
        {
            InventorySlot slotContainsItem = FindSlot(itemToAdd);

            if (slotContainsItem != null)
            {
                AddToSlotContainsItem(slotContainsItem, itemToAdd, amountToAdd, durability);
            }
            else
            {
                AddItemToEmptySlot(itemToAdd, amountToAdd, durability);
            }
        }

        private void AddToSlotContainsItem(InventorySlot slot, ItemData itemToAdd, int amountToAdd, float durability)
        {
            if (slot.EnoughRoomLeftInStack(amountToAdd, out int remaining))
            {
                slot.AddToStack(amountToAdd);
            }
            else
            {
                slot.AddToStack(remaining);
                amountToAdd -= remaining;
                AddToInventory(itemToAdd, amountToAdd, durability);
            }
        }
        
        private void AddItemToEmptySlot(ItemData itemToAdd, int amountToAdd, float durability)
        {
            if (HasFreeSlot(out InventorySlot freeSlot))
            {
                if (amountToAdd <= itemToAdd.maxStackSize)
                {
                    freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd, durability, false);
                }
                else
                {
                    freeSlot.UpdateInventorySlot(itemToAdd, itemToAdd.maxStackSize, durability, false);
                    amountToAdd -= itemToAdd.maxStackSize;
                    AddToInventory(itemToAdd, amountToAdd, durability);
                }
            }
            else
            {
                if (amountToAdd > 0)
                {
                    InventoryItemRemaining(itemToAdd, amountToAdd, durability);
                    return;
                }
            }
        }

        protected int GetEmptySlotCount()
        {
            return inventorySlots.Count(slot => slot.IsEmptySlot());
        }
        
        public bool IsEnoughItems(ItemData itemData, int amount, out int count)
        {
            var slots = inventorySlots.Where(slot => slot.ItemData == itemData).ToList();
            if (slots.Count == 0)
            {
                count = 0;
                return false;
            }

            count = 0;
            foreach (InventorySlot slot in slots)
            {
                count += slot.StackSize;
                if (count >= amount) return true;
            }
        
            return count >= amount;
        }
        
        protected void RemoveItem(ItemData itemData, int amount)
        {
            if(amount <= 0) return;
        
            InventorySlot slot = inventorySlots.FirstOrDefault(slot => slot.ItemData == itemData);
            if(slot == null) return;
            
            if (slot.StackSize > amount)
            {
                slot.RemoveFromStack(amount);
            }
            else
            {
                amount -= slot.StackSize;
                slot.RemoveFromStack(slot.StackSize);
                RemoveItem(itemData, amount);
            }
        }
        
        protected virtual void InventoryItemRemaining(ItemData itemData, int amount, float durability)
        {
            
        }

        public List<InventorySlot> SlotHasItems()
        {
            return inventorySlots.Where(slot => !slot.IsEmptySlot()).ToList();
        }
    }

    public class ListItemSlot
    {
        public List<ItemSlot> ItemSlots;

        public ListItemSlot(List<ItemSlot> slots)
        {
            ItemSlots = slots;
        }
    }
    public class ItemSlot
    {
        public ItemData ItemData;
        public int StackSize;
        public float Durability;

        public ItemSlot(ItemData item, int amount, float durability)
        {
            ItemData = item;
            StackSize = amount;
            Durability = durability;
        }
    }
}
