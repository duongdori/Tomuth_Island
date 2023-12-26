using Animancer;
using DR.SoundSystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        [SerializeField] private ClipTransition anim;

        public override PlayerStatePriority Priority => PlayerStatePriority.Medium;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            anim.Events.OnEnd = OnEndState;
            player.animancer.Play(anim);
            SoundManager.Instance.PlaySfx(Sound.Landing);
        }

        protected override void Update()
        {
            base.Update();
            Move(Time.deltaTime);
            FaceTarget();
        }

        private void OnEndState()
        {
            ReturnToLocomotion();
        }
    }
}