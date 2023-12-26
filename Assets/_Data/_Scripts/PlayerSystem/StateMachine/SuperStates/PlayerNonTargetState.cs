using Animancer;
using DR.CombatSystem.Weapons;
using DR.InputSystem;
using DR.MainMenuSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DR.PlayerSystem.StateMachine
{
    public class PlayerNonTargetState : PlayerBaseState
    {
        [SerializeField] protected float parameterFadeSpeed = 3;
        [SerializeField] protected LinearMixerTransitionAsset.UnShared anim;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InputHandler.Instance.JumpEvent += OnJump;
            InputHandler.TargetEvent += OnTarget;
            InputHandler.Instance.CrouchEvent += OnCrouch;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            InputHandler.Instance.JumpEvent -= OnJump;
            InputHandler.TargetEvent -= OnTarget;
            InputHandler.Instance.CrouchEvent -= OnCrouch;
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

        protected void HandleMovement(float speed, float value)
        {
            Vector3 movement = CalculateMovement();
            Move(movement * speed, Time.deltaTime);

            anim.State.Parameter = Mathf.MoveTowards(anim.State.Parameter, movement.magnitude * value,
                parameterFadeSpeed * Time.deltaTime);
            
            if(InputHandler.MovementValue == Vector2.zero) return;
        
            FaceMovementDirection(movement, Time.deltaTime);
        }
        
        protected void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            Quaternion targetDirection = Quaternion.LookRotation(movement);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetDirection,
                deltaTime * player.parameters.rotationDamping);
        }
        
        protected Vector3 CalculateMovement()
        {
            if (player.mainCameraTransform == null) return Vector3.zero;
            
            Vector3 forward = player.mainCameraTransform.forward;
            Vector3 right = player.mainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
        
            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = forward * InputHandler.MovementValue.y +
                              right * InputHandler.MovementValue.x;

            return moveDir;
        }
        
        protected virtual void OnJump()
        {
            if(!player.IsGrounded() || player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.jumpCost) return;
            player.stateMachine.TrySetState(player.brain.jumpState);
        }
        
        protected virtual void OnTarget()
        {
            if(!player.targeter.SelectTarget()) return;
            player.animancer.Animator.SetBool("FreeLookState", false);
        }

        protected virtual void OnCrouch()
        {
        }
    }
}