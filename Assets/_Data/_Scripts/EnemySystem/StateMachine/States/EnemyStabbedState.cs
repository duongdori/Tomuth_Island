using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyStabbedState : EnemyBaseState
    {
        [SerializeField] private ClipTransition anim;
        [SerializeField] private ClipTransition animDead;
        
        public override void OnEnterState()
        {
            base.OnEnterState();
            enemy.enemyStats.enabled = false;
            enemy.controller.enabled = false;
            enemy.agent.enabled = false;
            //enemy.backStabCollider.backStabBoxCollider.enabled = false;
        }

        private void OnEnable()
        {
            anim.Events.OnEnd = () => { enemy.animancer.Play(animDead); };
            enemy.animancer.Play(anim);
        }
    }
}