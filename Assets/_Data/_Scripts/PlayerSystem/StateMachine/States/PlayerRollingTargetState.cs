using Animancer;
using DR.InputSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerRollingTargetState : PlayerBaseState
    {
        [SerializeField] private MixerTransition2DAsset.UnShared anim;

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.animancer.Animator.SetBool("TargetState", true);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            anim.Events.OnEnd = () => { player.stateMachine.TrySetState(player.brain.targetState); };
            player.animancer.Play(anim);
            player.animancer.Animator.applyRootMotion = true;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            player.animancer.Animator.applyRootMotion = false;
        }

        protected override void Update()
        {
            base.Update();
            
            //Vector3 movement = CalculateMovement();
            //Move(movement * 5f, Time.deltaTime);
            anim.State.Parameter = InputHandler.MovementValue;
            FaceTarget();
        }
        
        private Vector3 CalculateMovement()
        {
            Vector3 movement = new Vector3();
            var playerTransform = player.transform;
        
            movement += playerTransform.right * InputHandler.MovementValue.x;
            movement += playerTransform.forward * InputHandler.MovementValue.y;
        
            return movement;
        }
    }
}