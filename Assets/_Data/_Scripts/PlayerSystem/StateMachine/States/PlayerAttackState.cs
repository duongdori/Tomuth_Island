using Animancer;
using DR.InputSystem;
using DR.InventorySystem;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        private ClipTransition _anim;
        private int _attackIndex = int.MaxValue;
        private WeaponItemData Weapon => (WeaponItemData)PlayerController.Instance.weaponSlotManger.currentWeapon;
        public override bool CanEnterState =>
            Weapon != null && Weapon.attackAnimations.Count > 0;

        public override void OnExitState()
        {
            base.OnExitState();
            
            PlayerController.Instance.playerUI.SetActiveRightClickIcon(false);
            player.weaponHandler.DisableWeapon();

            _anim.Events.RemoveCallback("PlaySoundFX", (() =>
            {
                SoundManager.Instance.PlaySfx(Weapon.attackAnimations[_attackIndex].soundFX);
            }));
            
            _anim.Events.RemoveCallback("EnableWeapon", player.weaponHandler.EnableWeapon);
            _anim.Events.RemoveCallback("DisableWeapon", player.weaponHandler.DisableWeapon);
            
            InputHandler.BlockEvent -= OnBlockEvent;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.playerStats.StaminaSystem.TrySpendMana(player.parameters.attackCost);
            
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (ShouldRestartCombo())
            {
                _attackIndex = 0;
            }
            else
            {
                _attackIndex++;
            }

            _anim = Weapon.attackAnimations[_attackIndex];
            _anim.Events.OnEnd = OnEnd;
            InputHandler.BlockEvent += OnBlockEvent;
            _anim.Events.SetCallback("PlaySoundFX", (() =>
            {
                SoundManager.Instance.PlaySfx(Weapon.attackAnimations[_attackIndex].soundFX);
            }));
            
            _anim.Events.SetCallback("EnableWeapon", player.weaponHandler.EnableWeapon);
            _anim.Events.SetCallback("DisableWeapon", player.weaponHandler.DisableWeapon);
            player.weaponDamage.SetAttack(Weapon.attackList[_attackIndex].Damage, 0f);
            player.animancer.Play(_anim);
            
        }

        protected override void Update()
        {
            base.Update();
            Move(Time.deltaTime);
            
            if(player.weaponDamage.enemy == null) return;
            PlayerController.Instance.playerUI.SetActiveRightClickIcon(true);
        }

        private void OnEnd()
        {
            player.weaponDamage.enemy = null;
            ReturnToLocomotion();
        }
        
        private bool ShouldRestartCombo()
        {
            var attackAnimations = Weapon.attackAnimations;

            if (_attackIndex >= attackAnimations.Count - 1)
                return true;

            var state = attackAnimations[_attackIndex].State;
            if (state == null || state.Weight == 0)
                return true;

            return false;
        }
        
        private void OnBlockEvent()
        {
            if(!PlayerController.Instance.weaponSlotManger.currentWeapon.canSpecialAttack) return;
            if(player.weaponDamage.enemy == null) return;
            player.stateMachine.TrySetState(player.brain.specialAttackState);
        }
    }
}