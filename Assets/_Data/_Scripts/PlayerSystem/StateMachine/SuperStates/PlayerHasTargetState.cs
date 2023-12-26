using Animancer;
using DR.CombatSystem.Weapons;
using DR.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DR.PlayerSystem.StateMachine
{
    public class PlayerHasTargetState : PlayerBaseState
    {
        [SerializeField] protected float parameterFadeSpeed = 3;
        [SerializeField] protected MixerTransition2DAsset.UnShared anim;

        protected override void OnEnable()
        {
            base.OnEnable();
            InputHandler.TargetEvent += OnTarget;
            InputHandler.Instance.JumpEvent += OnJump;
            InputHandler.Instance.CrouchEvent += OnCrouch;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.animancer.Animator.SetBool("TargetState", true);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            InputHandler.TargetEvent -= OnTarget;
            InputHandler.Instance.JumpEvent -= OnJump;
            InputHandler.Instance.CrouchEvent -= OnCrouch;
            
            if(player.stateMachine.NextState == player.brain.jumpState 
               || player.stateMachine.NextState == player.brain.attackState) return;
            
            player.animancer.Animator.SetBool("TargetState", false);
        }

        protected override void Update()
        {
            base.Update();
            
            WeaponSlotManger weaponSlotManager = PlayerController.Instance.weaponSlotManger;
            
            if (InputHandler.Instance.IsAttacking && !EventSystem.current.IsPointerOverGameObject() &&
                weaponSlotManager.HasWeaponOnHand() && !AttemptBackStab())
            {
                if(player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.attackCost) return;

                player.stateMachine.TrySetState(player.brain.attackState);
                return;
            }
            
            if (InputHandler.Instance.IsAttacking && !EventSystem.current.IsPointerOverGameObject() &&
                weaponSlotManager.HasWeaponOnHand() && AttemptBackStab())
            {
                if(player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.attackCost) return;

                if (weaponSlotManager.CanSpecialAttack())
                {
                    player.stateMachine.TrySetState(player.brain.backStabState);
                }
                else
                {
                    player.stateMachine.TrySetState(player.brain.attackState);
                }
            }
        }

        protected void HandleMovement(float speed)
        {
            Vector3 movement = CalculateMovement(Time.deltaTime);
            Move(movement * speed, Time.deltaTime);

            anim.State.Parameter = Vector2.MoveTowards(anim.State.Parameter, InputHandler.MovementValue,
                parameterFadeSpeed * Time.deltaTime);
            
            FaceTarget();
        }
        private Vector3 CalculateMovement(float deltaTime)
        {
            Vector3 movement = new Vector3();
            var playerTransform = player.transform;
        
            movement += playerTransform.right * InputHandler.MovementValue.x;
            movement += playerTransform.forward * InputHandler.MovementValue.y;
        
            return movement;
        }
        
        protected virtual void OnTarget()
        {
            player.targeter.Cancel();
        }
        
        private void OnJump()
        {
            if(!player.IsGrounded() || player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.jumpCost) return;

            player.stateMachine.TrySetState(player.brain.jumpState);
        }
        
        protected virtual void OnCrouch()
        {
        }
    }
}