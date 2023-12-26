using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace DR.EnemySystem.StateMachine
{
    public class EnemyBaseState : StateBehaviour
    {
        [System.Serializable]
        public class StateMachine : StateMachine<EnemyBaseState>.WithDefault{ }

        public Enemy enemy;

        public virtual EnemyStatePriority Priority => EnemyStatePriority.Low;
        
        public virtual bool CanInterruptSelf => false;
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            gameObject.GetComponentInParentOrChildren(ref enemy);
        }
#endif
        
        public override bool CanExitState
        {
            get
            {
                var nextState = enemy.stateMachine.NextState;
                if (nextState == this)
                    return CanInterruptSelf;
                else if (Priority == EnemyStatePriority.Low)
                    return true;
                else
                    return nextState.Priority > Priority;
            }
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 movement, float deltaTime)
        {
            enemy.controller.Move((movement + enemy.forceReceiver.Movement) * deltaTime);
        }
        
        // protected virtual void FacePlayer()
        // {
        //     if(character.playerTarget == null) return;
        //
        //     Vector3 lookPos = character.playerTarget.position - character.transform.position;
        //     lookPos.y = 0f;
        //
        //     character.transform.rotation = Quaternion.Lerp(character.transform.rotation,
        //         Quaternion.LookRotation(lookPos), 10f * Time.deltaTime);
        // }
        //
        // protected virtual bool IsInChaseRange()
        // {
        //     //if (stateMachine.Player.IsDead) return false;
        //
        //     Vector3 toPlayer = character.playerTarget.position - character.transform.position;
        //
        //     float playerDistanceSqr = toPlayer.sqrMagnitude;
        //
        //     return playerDistanceSqr <= Mathf.Pow(character.parameters.playerChasingRange, 2f);
        // }
        
    }
}