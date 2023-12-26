using System.Collections.Generic;
using UnityEngine;

namespace DR.InventorySystem
{
    public class UI_Inventory : MonoBehaviour
    {
        [SerializeField] private UI_InventorySlot slotUIPrefab;
        [SerializeField] private InventorySystem inventory;
        public InventorySystem Inventory => inventory;

        [SerializeField] private List<UI_InventorySlot> slots;
        public List<UI_InventorySlot> Slots => slots;

        private void Awake()
        {
            InitializeInventoryUI();
        }

        [ContextMenu("Initialize Inventory UI")]
        private void InitializeInventoryUI()
        {
            if(inventory == null || slotUIPrefab == null) return;

            slots = new List<UI_InventorySlot>(inventory.Size);
            
            RemoveChildren();

            for (int i = 0; i < inventory.Size; i++)
            {
                UI_InventorySlot uiSlot = Instantiate(slotUIPrefab, transform);
                uiSlot.AssignSlot(i);
                slots.Add(uiSlot);
            }
        }

        private void RemoveChildren()
        {
            if (transform.childCount <= 0) return;
            
            Transform[] children = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            foreach (Transform child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}