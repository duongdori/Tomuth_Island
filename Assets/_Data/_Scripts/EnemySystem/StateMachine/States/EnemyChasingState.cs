using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyChasingState : EnemyBaseState
    {
        [SerializeField] protected float parameterFadeSpeed = 3;
        [SerializeField] private LinearMixerTransitionAsset.UnShared anim;
        private void OnEnable()
        {
            enemy.animancer.Play(anim);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            enemy.agent.ResetPath();
        }

        private void Update()
        {
            if (!enemy.fieldOfView.CanSeePlayer || enemy.fieldOfView.PlayerRef == null)
            {
                enemy.stateMachine.TrySetDefaultState();
                return;
            }

            if (CheckDistance() && !enemy.agent.pathPending)
            {
                enemy.stateMachine.TrySetState(enemy.brain.attackState);
                return;
            }
            
            if (enemy.fieldOfView.CanSeePlayer && enemy.fieldOfView.PlayerRef != null)
            {
                enemy.agent.SetDestination(enemy.fieldOfView.PlayerRef.transform.position);
            }

            anim.State.Parameter = Mathf.MoveTowards(anim.State.Parameter, enemy.agent.desiredVelocity.normalized.magnitude,
                parameterFadeSpeed * Time.deltaTime);
        }

        private bool CheckDistance()
        {
            if (enemy.fieldOfView.PlayerRef == null) return false;
            
            return Vector3.Distance(enemy.transform.position, enemy.fieldOfView.PlayerRef.transform.position) <=
                   enemy.parameters.stoppingDistance;
        }
    }
}