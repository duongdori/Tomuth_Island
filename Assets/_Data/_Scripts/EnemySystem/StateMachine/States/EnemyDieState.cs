using Animancer;
using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyDieState : EnemyBaseState
    {
        [SerializeField] private CustomClipTransition anim;

        public override EnemyStatePriority Priority => EnemyStatePriority.High;

        private void OnEnable()
        {
            enemy.enemyStats.enabled = false;
            enemy.controller.enabled = false;
            enemy.agent.enabled = false;
            enemy.redirectRootMotion.enabled = false;
            enemy.animancer.Play(anim);
            SoundManager.Instance.PlaySfx(anim.soundFX);
        }
    }
}