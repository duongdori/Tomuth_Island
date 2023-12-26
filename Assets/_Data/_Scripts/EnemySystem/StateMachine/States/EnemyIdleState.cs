using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyIdleState : EnemyBaseState
    {
        [SerializeField] private ClipTransition anim;

        [SerializeField] private float timer;
        private void OnEnable()
        {
            timer = Random.Range(6f, 8f);
            enemy.animancer.Play(anim);
        }

        private void Update()
        {
            if (enemy.fieldOfView.CanSeePlayer)
            {
                enemy.stateMachine.TrySetState(enemy.brain.chasingState);
                return;
            }

            if (timer <= 0)
            {
                enemy.stateMachine.TrySetState(enemy.brain.wanderState);
            }
            
            timer -= Time.deltaTime;
        }
    }
}