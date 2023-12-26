using Animancer;
using DR.InputSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerLocomotionState : PlayerNonTargetState
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            player.animancer.Animator.SetBool("FreeLookState", true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            InputHandler.RollEvent += OnRoll;
            InputHandler.SprintEvent += OnSprintEvent;
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
            
            float speed = player.parameters.movementSpeed;
            float value = 1f;
            
            if (InputHandler.IsSprint && player.playerStats.StaminaSystem.GetStaminaAmount() > 0)
            {
                speed = player.parameters.sprintSpeed;
                value = 1.5f;
                player.playerStats.StaminaSystem.TrySpendManaPerSecond(Time.deltaTime, player.parameters.sprintCost);
            }
            else if (InputHandler.IsSprint && player.playerStats.StaminaSystem.GetStaminaAmount() <= 0)
            {
                InputHandler.SetIsSprint(false);
                OnSprintEvent();
            }
            
            HandleMovement(speed, value);
        }
        
        protected override void OnCrouch()
        {
            base.OnCrouch();
            player.stateMachine.TrySetState(player.brain.crouchingState);
        }

        protected override void OnTarget()
        {
            base.OnTarget();
            player.stateMachine.TrySetState(player.brain.targetState);
        }
        
        private void OnRoll()
        {
            if(player.playerStats.StaminaSystem.GetStaminaAmount() < player.parameters.rollCost) return;
            player.stateMachine.TrySetState(player.brain.rollingState);
        }
        
        private void OnSprintEvent()
        {
            player.playerStats.StaminaSystem.SetCanRegen(true);
        }
    }
}