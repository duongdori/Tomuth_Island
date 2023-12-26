using Animancer;
using DR.InputSystem;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerJumpingState : PlayerBaseState
    {
        [SerializeField] private LinearMixerTransitionAsset.UnShared anim;

        public override PlayerStatePriority Priority => PlayerStatePriority.Medium;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            player.animancer.Play(anim);
            player.forceReceiver.Jump(player.parameters.jumpForce);
            SoundManager.Instance.PlaySfx(Sound.Jump);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.playerStats.StaminaSystem.TrySpendMana(player.parameters.jumpCost);
        }

        protected override void Update()
        {
            base.Update();
            
            Vector3 movement = CalculateMovement();
            Move(movement * player.parameters.airBorneSpeed, Time.deltaTime);

            anim.State.Parameter = player.controller.velocity.y;
            
            if (player.IsGrounded() && player.controller.velocity.y <= 0f)
            {
                player.stateMachine.ForceSetState(player.brain.landingState);
            }
            
            FaceTarget();
        }
        
        private Vector3 CalculateMovement()
        {
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
    }
}