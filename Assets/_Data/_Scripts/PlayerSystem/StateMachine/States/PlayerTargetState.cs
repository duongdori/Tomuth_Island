using Animancer;
using DR.InputSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerTargetState : PlayerHasTargetState
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            InputHandler.RollEvent += OnRoll;
            player.animancer.Play(anim);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            InputHandler.RollEvent -= OnRoll;
        }

        protected override void Update()
        {
            base.Update();
            
            if (player.targeter.CurrentTarget == null)
            {
                player.stateMachine.ForceSetState(player.stateMachine.DefaultState);
                return;
            }

            HandleMovement(player.parameters.targetMovementSpeed);
        }

        protected override void OnTarget()
        {
            base.OnTarget();
            player.stateMachine.TrySetDefaultState();
        }

        protected override void OnCrouch()
        {
            base.OnCrouch();
            player.stateMachine.TrySetState(player.brain.crouchingTargetState);
        }
        
        private void OnRoll()
        {
            if(player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.rollCost) return;
            
            player.stateMachine.TrySetState(player.brain.rollingTargetState);
        }
    }
}