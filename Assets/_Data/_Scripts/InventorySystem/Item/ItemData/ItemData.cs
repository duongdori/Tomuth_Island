using Animancer;
using UnityEngine;

namespace DR.InventorySystem
{
    [CreateAssetMenu(menuName = "SO/Items/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Item Information")]
        public int itemID;
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;
        public GameObject itemPrefab;
        public GameObject itemVisual;
        public bool isStackable;
        public int maxStackSize;
        public bool isLeftHand;
        public bool canHoldOnHand;
        public bool canSpecialAttack;
        public ClipTransition equipAnimation;
        public ClipTransition unequipAnimation;
        public float durabilityMax = 100f;
        public float durabilityValue;
        public AudioClip soundFX;
    }
}