using DR.InputSystem;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.InventorySystem
{
    public class PlayerInteract : MonoBehaviour
    {
        private HotbarInventory _hotbarInventory;
        private BackpackInventory _backpackInventory;
        private GameItem _item;
        
        [SerializeField] private GameObject eKey;
        [SerializeField] private bool canInteract;
        private void Start()
        {
            InputHandler.InteractEvent += OnInteractEvent;
            _hotbarInventory = PlayerController.Instance.hotbarInventory;
            _backpackInventory = PlayerController.Instance.backpackInventory;
        }

        private void OnInteractEvent()
        {
            if(!canInteract) return;
            
            if (_hotbarInventory.CanAddItem(_item.ItemSlot.ItemData))
            {
                _hotbarInventory.AddToInventory(_item.ItemSlot.ItemData, _item.ItemSlot.StackSize, _item.ItemSlot.CurrentDurability);
                _item.DestroySelf();
                
                _item = null;
                eKey.SetActive(false);
                canInteract = false;
                return;
            }
            
            if (_backpackInventory.CanAddItem(_item.ItemSlot.ItemData))
            {
                _backpackInventory.AddToInventory(_item.ItemSlot.ItemData, _item.ItemSlot.StackSize, _item.ItemSlot.CurrentDurability);
                _item.DestroySelf();
                
                _item = null;
                eKey.SetActive(false);
                canInteract = false;
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _item = other.gameObject.GetComponent<GameItem>();
            
            if(_item == null) return;
            eKey.SetActive(true);
            canInteract = true;
            
        }

        private void OnTriggerExit(Collider other)
        {
            _item = null;
            eKey.SetActive(false);
            canInteract = false;
        }
    }
}

