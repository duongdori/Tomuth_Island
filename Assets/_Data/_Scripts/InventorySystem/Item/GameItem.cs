using System;
using HighlightPlus;
using UnityEngine;

namespace DR.InventorySystem
{
    public class GameItem : MonoBehaviour
    {
        public Rigidbody rb;
        [SerializeField] private Collider col;
        
        [SerializeField] private InventorySlot itemSlot;
        public InventorySlot ItemSlot => itemSlot;
        
        [SerializeField] float rotationSpeed = 30f;
        private bool _canRotate;

        [SerializeField] private HighlightEffect highlightEffect;
        private void Awake()
        {
            _canRotate = false;
            highlightEffect.highlighted = false;
        }

        private void Start()
        {
            SetupGameItem();
            UpdateGameObjectName();
        }

        private void OnValidate()
        {
            UpdateGameObjectName();
        }

        private void Update()
        {
            // if(!_canRotate) return;
            //
            // Quaternion deltaRotation = Quaternion.Euler(Vector3.up * (rotationSpeed * Time.deltaTime));
            // Quaternion currentRotation = transform.rotation;
            // transform.rotation = currentRotation * deltaRotation;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                rb.isKinematic = true;
                col.isTrigger = true;
                _canRotate = true;
                highlightEffect.highlighted = true;
            }
        }

        [ContextMenu("Setup Game Item")]
        private void SetupGameItem()
        {
            if(itemSlot.ItemData == null) return;
            
            SetItemVisual();
        }

        private void SetItemVisual()
        {
            if (transform.childCount > 0)
            {
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
            
            GameObject item = Instantiate(itemSlot.ItemData.itemVisual, transform);
            item.transform.localPosition = Vector3.up * 0.4f;
        }
        
        private void UpdateGameObjectName()
        {
            if(itemSlot.ItemData == null) return;
            
            string itemName = itemSlot.ItemData.itemName;
            string number = itemSlot.ItemData.isStackable ? itemSlot.StackSize.ToString() : "ns";
            gameObject.name = $"{itemName} ({number})";
        }
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void SetSlot(InventorySlot slot)
        {
            itemSlot = slot;
            SetItemVisual();
        }
    }
    
}

