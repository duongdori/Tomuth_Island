using UnityEngine;
using UnityEngine.InputSystem;

namespace DR.InventorySystem
{
    public class InventorySwapping : MonoBehaviour
    {
        public void SlotClicked(UI_InventorySlot clickedSlot)
        {
            ItemData clickedSlotItemData = clickedSlot.AssignedSlot.ItemData;
            ItemData mouseSlotItemData = MouseItemData.Instance.AssignedSlot.ItemData;
            
            // Clicked slot has an item - Mouse doesn't have an item - pick up that item
            if (clickedSlotItemData != null && mouseSlotItemData == null)
            {
                PickUpItemToMouseSlot(clickedSlot);
                return;
            }
            
            // Clicked slot doesn't have an item - Mouse does have an item - place the mouse item into the empty slot
            if (clickedSlotItemData == null && mouseSlotItemData != null)
            {
                PlaceMouseItemIntoEmptySlot(clickedSlot);
                return;
            }
            
            // Clicked slot has an item - Mouse does have an item - swap slots and combine slots
            if (clickedSlotItemData != null && mouseSlotItemData != null)
            {
                if (clickedSlotItemData != mouseSlotItemData)
                {
                    SwapSlots(clickedSlot);
                    return;
                }
                
                CombineSlots(clickedSlot, mouseSlotItemData);
            }
        }

        private void PickUpItemToMouseSlot(UI_InventorySlot clickedSlot)
        {
            bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

            // If player is holding shift key? Split the stack.
            if (isShiftPressed && clickedSlot.AssignedSlot.SplitStack(out InventorySlot halfStackSlot))
            {
                MouseItemData.Instance.AssignedSlot.UpdateInventorySlot(halfStackSlot.ItemData, halfStackSlot.StackSize, halfStackSlot.CurrentDurability, halfStackSlot.IsActive);
            }
            else
            {
                MouseItemData.Instance.AssignedSlot.UpdateInventorySlot(clickedSlot.AssignedSlot.ItemData,
                    clickedSlot.AssignedSlot.StackSize, clickedSlot.AssignedSlot.CurrentDurability, clickedSlot.AssignedSlot.IsActive);
                clickedSlot.AssignedSlot.ClearSlot();
            }
        }
        private void PlaceMouseItemIntoEmptySlot(UI_InventorySlot clickedSlot)
        {
            InventorySlot slot = MouseItemData.Instance.AssignedSlot;
            clickedSlot.AssignedSlot.UpdateInventorySlot(slot.ItemData, slot.StackSize, slot.CurrentDurability, slot.IsActive);
                
            MouseItemData.Instance.AssignedSlot.ClearSlot();
        }
        private void SwapSlots(UI_InventorySlot clickedSlot)
        {
            InventorySlot clonedSlot = new InventorySlot(MouseItemData.Instance.AssignedSlot.ItemData,
                MouseItemData.Instance.AssignedSlot.StackSize, MouseItemData.Instance.AssignedSlot.CurrentDurability, MouseItemData.Instance.AssignedSlot.IsActive);

            MouseItemData.Instance.AssignedSlot.UpdateInventorySlot(clickedSlot.AssignedSlot.ItemData,
                clickedSlot.AssignedSlot.StackSize, clickedSlot.AssignedSlot.CurrentDurability, clickedSlot.AssignedSlot.IsActive);
            
            clickedSlot.AssignedSlot.UpdateInventorySlot(clonedSlot.ItemData, clonedSlot.StackSize, clonedSlot.CurrentDurability, clonedSlot.IsActive);
        }
        private void CombineSlots(UI_InventorySlot clickedSlot, ItemData mouseSlotItemData)
        {
            int mouseSlotStackSize = MouseItemData.Instance.AssignedSlot.StackSize;
                    
            if (clickedSlot.AssignedSlot.EnoughRoomLeftInStack(mouseSlotStackSize, out int leftInStack))
            {
                clickedSlot.AssignedSlot.AddToStack(mouseSlotStackSize);
                        
                MouseItemData.Instance.AssignedSlot.ClearSlot();
                return;
            }

            if (leftInStack < 1)
            {
                SwapSlots(clickedSlot);
                return;
            }
            
            clickedSlot.AssignedSlot.AddToStack(leftInStack);

            MouseItemData.Instance.AssignedSlot.RemoveFromStack(leftInStack);
        }
    }
}