using Animancer;
using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyHurtState : EnemyBaseState
    {
        [SerializeField] private CustomClipTransition anim;

        public override EnemyStatePriority Priority => EnemyStatePriority.Medium;
        public override bool CanInterruptSelf => true;

        private void OnEnable()
        {
            enemy.enemyStats.HealthSystem.OnTakeDamage += () => { enemy.stateMachine.ForceSetState(this); };
            anim.Events.OnEnd = () => { enemy.stateMachine.ForceSetState(enemy.brain.chasingState); };
            enemy.animancer.Play(anim);
            SoundManager.Instance.PlaySfx(anim.soundFX);
        }
    }
}