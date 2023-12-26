using Animancer;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerCrouchingState : PlayerNonTargetState
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            player.animancer.Play(anim);
            SoundManager.Instance.PlaySfx(Sound.StartCrouch);
        }

        protected override void Update()
        {
            base.Update();
            
            HandleMovement(player.parameters.crouchMoveSpeed, 1f);
        }
        
        protected override void OnCrouch()
        {
            base.OnCrouch();
            SoundManager.Instance.PlaySfx(Sound.EndCrouch);
            player.stateMachine.TrySetDefaultState();
        }

        protected override void OnTarget()
        {
            base.OnTarget();
            player.stateMachine.TrySetState(player.brain.crouchingTargetState);
        }
    }
}