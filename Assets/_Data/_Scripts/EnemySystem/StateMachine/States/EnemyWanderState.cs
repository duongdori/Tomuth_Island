using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyWanderState : EnemyBaseState
    {
        public float acceptableDistance = 0.2f;
        [SerializeField] protected float parameterFadeSpeed = 3;
        [SerializeField] private LinearMixerTransitionAsset.UnShared anim;

        private void OnEnable()
        {
            enemy.agent.destination = GenerateRandomPoint();
            enemy.animancer.Play(anim);
        }

        private void Update()
        {
            if (enemy.fieldOfView.CanSeePlayer)
            {
                enemy.stateMachine.TrySetState(enemy.brain.chasingState);
                return;
            }
            
            if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= acceptableDistance)
            {
                enemy.stateMachine.TrySetDefaultState();
                return;
            }

            anim.State.Parameter = Mathf.MoveTowards(anim.State.Parameter, enemy.agent.desiredVelocity.normalized.magnitude,
                parameterFadeSpeed * Time.deltaTime);
        }

        private Vector3 GenerateRandomPoint()
        {
            Vector3 randomPoint = enemy.transform.position + Random.insideUnitSphere * enemy.parameters.wanderRadius;
            randomPoint.y = enemy.transform.position.y;
            
            //GameObject test = new GameObject("RandomPoint") { transform = { position = randomPoint } };

            return randomPoint;
            
        }
    }
}