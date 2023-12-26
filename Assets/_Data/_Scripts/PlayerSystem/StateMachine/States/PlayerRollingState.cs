using Animancer;
using DR.InputSystem;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerRollingState : PlayerBaseState
    {
        [SerializeField] private ClipTransition anim;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            anim.Events.OnEnd = () =>
            {
                player.stateMachine.TrySetDefaultState();
            };
            
            anim.Events.SetCallback("PlaySoundFX", (() =>
            {
                SoundManager.Instance.PlaySfx(Sound.Rolling);
            }));
            
            player.animancer.Play(anim);
            player.animancer.Animator.applyRootMotion = true;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.playerStats.StaminaSystem.TrySpendMana(player.parameters.rollCost);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            player.animancer.Animator.applyRootMotion = false;
            
            anim.Events.RemoveCallback("PlaySoundFX", (() =>
            {
                SoundManager.Instance.PlaySfx(Sound.Rolling);
            }));
        }

        protected override void Update()
        {
            base.Update();
            
            Vector3 movement = CalculateMovement();
            Move(movement * player.parameters.rollingForce, Time.deltaTime);
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