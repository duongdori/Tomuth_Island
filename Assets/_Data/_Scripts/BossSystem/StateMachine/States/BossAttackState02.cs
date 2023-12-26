using Animancer;
using UnityEngine;

namespace DR.BossSystem.StateMachine.States
{
    public class BossAttackState02 : BossBaseState
    {
        [SerializeField] private ClipTransition anim;

        private void OnEnable()
        {
            anim.Events.OnEnd = OnEnd;
            boss.animancer.Play(anim);
            boss.animancer.Animator.applyRootMotion = true;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            boss.animancer.Animator.applyRootMotion = false;
            anim.Events.OnEnd = null;
        }

        private void OnEnd()
        {
            boss.stateMachine.TrySetState(boss.brain.attack02State);
        }
    }
}