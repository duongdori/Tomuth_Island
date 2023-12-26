using System.Linq;
using Animancer;
using DR.InventorySystem;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.CombatSystem.Weapons
{
    public class WeaponSlotManger : MyMonobehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private WeaponHolderSlot leftHandSlot;
        [SerializeField] private WeaponHolderSlot rightHandSlot;
    
        public ItemData currentWeapon;
        public InventorySlot slotActive;

        protected override void Start()
        {
            base.Start();
            PlayerController.Instance.hotbarInventory.OnInventorySlotActiveChanged += OnInventorySlotActiveChanged;
            PlayerController.Instance.player.playerStats.HealthSystem.OnDie += OnDie;
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadWeaponSlot();
        }

        public bool CanSpecialAttack()
        {
            return currentWeapon != null && currentWeapon.canSpecialAttack;
        }

        public bool HasWeaponOnHand()
        {
            return leftHandSlot.currentWeaponModel != null || rightHandSlot.currentWeaponModel != null;
        }
        public void OnInventorySlotActiveChanged(InventorySlot slot)
        {
            if(!slot.ItemData.canHoldOnHand) return;
        
            if (slot.IsActive)
            {
                slotActive = slot;
                slotActive.OnSlotClear += OnSlotClear;
                AnimancerState state = player.brain.actionLayer.Play(slot.ItemData.equipAnimation);
                state.Events.OnEnd = () => { player.brain.actionLayer.StartFade(0, player.brain.actionFadeOutDuration); };
                LoadWeaponOnSlot(slot.ItemData, slot.ItemData.isLeftHand);
            }
            else
            {
                slotActive.OnSlotClear -= OnSlotClear;
                slotActive = null;
                AnimancerState state = player.brain.actionLayer.Play(slot.ItemData.unequipAnimation);
                state.Events.OnEnd = () => { player.brain.actionLayer.StartFade(0, player.brain.actionFadeOutDuration); };
                LoadWeaponOnSlot(null, slot.ItemData.isLeftHand);
            }
        }

        public void LoadWeaponOnSlot(ItemData weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
            }
            else
            {
                currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
            }
        }

        private void OnDie()
        {
            rightHandSlot.UnloadWeaponAndDestroy();
            currentWeapon = null;
            slotActive = null;
        }
        public void OnSlotClear()
        {
            LoadWeaponOnSlot(null, slotActive.ItemData.isLeftHand);
            slotActive = null;
        }

        private void LoadWeaponSlot()
        {
            if(leftHandSlot != null && rightHandSlot != null) return;
            WeaponHolderSlot[] holderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            leftHandSlot = holderSlots.FirstOrDefault(s => s.isLeftHandSlot);
            rightHandSlot = holderSlots.FirstOrDefault(s => s.isRightHandSlot);
            Debug.LogWarning(transform.name + ": LoadWeaponSlot", gameObject);
        }
    }
}
