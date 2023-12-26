using System.Threading.Tasks;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.InventorySystem
{
    public class BackpackInventory : InventorySystem
    {
        [SerializeField] private GameItem gameItemPrefab;
        [SerializeField] private float dropOffset = 3f;

        protected override void Start()
        {
            base.Start();
            PlayerController.Instance.player.playerStats.HealthSystem.OnDie += OnDie;
        }
        
        private void OnDie()
        {
            foreach (InventorySlot slotHasItem in PlayerController.Instance.hotbarInventory.SlotHasItems())
            {
                DropItemOnGround(slotHasItem.ItemData, slotHasItem.StackSize, slotHasItem.CurrentDurability);
                slotHasItem.ClearSlot();
            }
            foreach (InventorySlot slotHasItem in SlotHasItems())
            {
                DropItemOnGround(slotHasItem.ItemData, slotHasItem.StackSize, slotHasItem.CurrentDurability);
                slotHasItem.ClearSlot();
            }
        }

        protected override void InventoryItemRemaining(ItemData itemData, int amount, float durability)
        {
            base.InventoryItemRemaining(itemData, amount, durability);
            DropItemOnGround(itemData,amount, durability);
        }

        private void DropItemOnGround(ItemData itemData, int amount, float durability)
        {
            Vector3 pos = transform.parent.position + transform.parent.forward * dropOffset;
            GameItem item = Instantiate(gameItemPrefab, pos, Quaternion.identity);
            item.ItemSlot.UpdateGameItem(itemData, amount, durability);
        }

        public int GetEmptySlots()
        {
            return GetEmptySlotCount();
        }

        public void RemoveItems(ItemData itemData, int amount)
        {
            RemoveItem(itemData, amount);
        }
        
        public async Task SaveData()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemSlot itemSlot = new ItemSlot(inventorySlots[i].ItemData, inventorySlots[i].StackSize, inventorySlots[i].CurrentDurability);
                string itemSlotJson = JsonUtility.ToJson(itemSlot);
                await CloudSaveManager.ForceSaveSingleData("BackpackItemSlot" + i, itemSlotJson);
            }
        }

        public async Task LoadData()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                var itemSlotString = await CloudSaveManager.RetrieveSpecificData<string>("BackpackItemSlot" + i);
                if(itemSlotString == null) return;
                ItemSlot itemSlot = JsonUtility.FromJson<ItemSlot>(itemSlotString);
                inventorySlots[i].UpdateInventorySlot(itemSlot.ItemData, itemSlot.StackSize, itemSlot.Durability, false);
            }
        }
    }
}