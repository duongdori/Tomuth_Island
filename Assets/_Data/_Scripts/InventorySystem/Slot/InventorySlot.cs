using System;
using UnityEngine;
using UnityEngine.Events;

namespace DR.InventorySystem
{
    public class InventorySlotChangedArgs
    {
        public ItemData ItemData { get; private set; }
        public int StackSize { get; private set; }
        public float CurrentDurability { get; private set; }
        public bool IsActive { get; private set; }

        public InventorySlotChangedArgs(ItemData itemData, int stackSize, float durability, bool isActive)
        {
            ItemData = itemData;
            StackSize = stackSize;
            CurrentDurability = durability;
            IsActive = isActive;
        }
    }
    
    [Serializable]
    public class InventorySlot
    {
        public event EventHandler<InventorySlotChangedArgs> OnInventorySlotChanged;
        public event UnityAction OnSlotClear; 
        
        [SerializeField] private ItemData itemData;
        public ItemData ItemData => itemData;
        
        [SerializeField] private int stackSize;
        public int StackSize
        {
            get => stackSize;
            set
            {
                value = value < 0 ? 0 : value;
                stackSize = value;
                NotifyAboutInventorySlotChanged();
            }
        }

        [SerializeField] private float currentDurability;

        public float CurrentDurability
        {
            get => currentDurability;
            set
            {
                currentDurability = value;
                NotifyAboutInventorySlotChanged();
            }
        }
        
        public bool IsStackable => itemData != null && itemData.isStackable;

        [SerializeField] private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                NotifyAboutInventorySlotChanged();
            }
        }

        private void NotifyAboutInventorySlotChanged()
        {
            OnInventorySlotChanged?.Invoke(this, new InventorySlotChangedArgs(itemData, stackSize, currentDurability, isActive));
        }
        
        public InventorySlot(ItemData itemData, int stackSize)
        {
            isActive = false;
            this.itemData = itemData;
            StackSize = stackSize;
        }
        
        public InventorySlot(ItemData itemData, int stackSize, float durability, bool active)
        {
            currentDurability = durability;
            isActive = active;
            this.itemData = itemData;
            StackSize = stackSize;
        }

        public InventorySlot()
        {
            ClearSlot();
        }
        
        public void ClearSlot()
        {
            currentDurability = -1;
            itemData = null;
            stackSize = -1;
            IsActive = false;
        }
        
        public void AssignItem(InventorySlot invSlot)
        {
            if (itemData == invSlot.ItemData)
            {
                AddToStack(invSlot.stackSize);
            }
            else
            {
                itemData = invSlot.ItemData;
                stackSize = 0;
                AddToStack(invSlot.stackSize);
            }
        }
        
        public void UpdateInventorySlot(ItemData data, int amount, bool active)
        {
            isActive = active;
            itemData = data;
            StackSize = amount;
        }
        
        public void UpdateInventorySlot(ItemData data, int amount, float durability, bool active)
        {
            currentDurability = durability;
            isActive = active;
            itemData = data;
            StackSize = amount;
        }

        public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
        {
            amountRemaining = itemData.maxStackSize - stackSize;

            return EnoughRoomLeftInStack(amountToAdd);
        }

        public bool EnoughRoomLeftInStack(int amountToAdd)
        {
            return stackSize + amountToAdd <= itemData.maxStackSize && IsStackable;
        }
        
        public bool CanAddAmountToStackSize()
        {
            return IsStackable && stackSize < itemData.maxStackSize;
        }
        
        public void AddToStack(int amount)
        {
            StackSize += amount;
        }

        public void RemoveFromStack(int amount)
        {
            StackSize -= amount;
            if (StackSize <= 0)
            {
                ClearSlot();
            }
        }

        public bool SplitStack(out InventorySlot splitStack)
        {
            if (stackSize <= 1)
            {
                splitStack = null;
                return false;
            }

            int halfStack = Mathf.RoundToInt((float)stackSize / 2);
            RemoveFromStack(halfStack);

            splitStack = new InventorySlot(itemData, halfStack);
            return true;
        }
        
        public bool IsEmptySlot()
        {
            return itemData == null;
        }
        
        public void UpdateGameItem(ItemData data, int amount, float durability)
        {
            itemData = data;
            stackSize = amount;
            currentDurability = durability;
        }

        public void DeductDurability()
        {
            CurrentDurability -= itemData.durabilityValue;
            if(currentDurability > 0) return;
            OnSlotClear?.Invoke();
            ClearSlot();
        }
    }
}
