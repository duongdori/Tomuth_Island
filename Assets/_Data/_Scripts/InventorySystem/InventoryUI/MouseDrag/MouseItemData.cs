using System.Collections.Generic;
using DR.PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DR.InventorySystem
{
    public class MouseItemData : MyMonobehaviour
    {
        public static MouseItemData Instance { get; private set; }
        
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private GameObject gameItemPrefab;
        [SerializeField] private float dropOffset = 2f;
        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private InventorySlot assignedSlot;
        public InventorySlot AssignedSlot => assignedSlot;
        
        private bool _activeIndicator = false;
        protected override void Awake()
        {
            base.Awake();
            
            if (Instance == null)
            {
                Instance = this;
            }
            
            UpdateViewState(assignedSlot.ItemData, assignedSlot.StackSize, assignedSlot.IsActive);
            assignedSlot.OnInventorySlotChanged += OnInventorySlotChanged;
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadItemIcon();
            LoadItemCount();
            LoadGameItemPrefab();
            LoadPlayerTransform();
        }

        private void Update()
        {
            if (!assignedSlot.IsEmptySlot())
            {
                transform.position = Mouse.current.position.ReadValue();

                if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
                {
                    if (assignedSlot.IsActive)
                    {
                        assignedSlot.IsActive = false;
                        PlayerController.Instance.weaponSlotManger.OnInventorySlotActiveChanged(assignedSlot);
                    }
                    LoadPlayerTransform();
                    Vector3 pos = playerTransform.position + playerTransform.forward;
                    GameObject item = Instantiate(gameItemPrefab, pos, Quaternion.identity);
                    
                    GameItem itemPickUp = item.GetComponent<GameItem>();
                    itemPickUp.rb.AddForce(playerTransform.forward * dropOffset, ForceMode.Impulse);
                    itemPickUp.rb.AddForce(playerTransform.up * 6, ForceMode.Impulse);
                    if (itemPickUp != null)
                    {
                        itemPickUp.ItemSlot.UpdateInventorySlot(assignedSlot.ItemData, assignedSlot.StackSize, assignedSlot.CurrentDurability, false);
                    }
                    assignedSlot.ClearSlot();
                }
            }
        }
        
        private void OnInventorySlotChanged(object sender, InventorySlotChangedArgs args)
        {
            UpdateViewState(args.ItemData, args.StackSize, args.IsActive);
        }
        
        private void UpdateViewState(ItemData itemData, int stackSize, bool active)
        {
            _activeIndicator = active;
            bool hasItem = itemData != null;
            bool isStackable = hasItem && itemData.isStackable && stackSize > 1;
            itemIcon.enabled = hasItem;
            itemCount.enabled = isStackable;
            
            if(!hasItem) return;
            itemIcon.sprite = itemData.itemIcon;
            if(isStackable) itemCount.SetText(stackSize.ToString());
        }

        private static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        #region Load Components

        private void LoadItemIcon()
        {
            if(itemIcon != null) return;
            itemIcon = GetComponentInChildren<Image>();
            Debug.LogWarning(transform.name + ": LoadItemIcon", gameObject);
        }
        
        private void LoadItemCount()
        {
            if(itemCount != null) return;
            itemCount = GetComponentInChildren<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadItemCount", gameObject);
        }
        
        private void LoadGameItemPrefab()
        {
            if(gameItemPrefab != null) return;
            gameItemPrefab = Resources.Load<GameObject>("Prefabs/Items/GameItem");
            Debug.LogWarning(transform.name + ": LoadGameItemPrefab", gameObject);
        }
        
        private void LoadPlayerTransform()
        {
            if(playerTransform != null) return;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.LogWarning(transform.name + ": LoadPlayerTransform", gameObject);
        }

        #endregion
        
    }
}
