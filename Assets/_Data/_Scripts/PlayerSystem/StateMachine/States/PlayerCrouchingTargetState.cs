using Animancer;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerCrouchingTargetState : PlayerHasTargetState
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
            
            if (player.targeter.CurrentTarget == null)
            {
                player.stateMachine.ForceSetState(player.brain.crouchingState);
                return;
            }
            
            HandleMovement(player.parameters.crouchMoveSpeed);
        }

        protected override void OnTarget()
        {
            base.OnTarget();
            player.stateMachine.ForceSetState(player.brain.crouchingState);
        }

        protected override void OnCrouch()
        {
            base.OnCrouch();
            SoundManager.Instance.PlaySfx(Sound.EndCrouch);
            player.stateMachine.TrySetState(player.brain.targetState);
        }
    }
}