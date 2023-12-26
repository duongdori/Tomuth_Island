using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace DR.BossSystem.StateMachine
{
    public class BossBaseState : StateBehaviour
    {
        [System.Serializable]
        public class StateMachine : StateMachine<BossBaseState>.WithDefault{ }

        public BossController boss;

        public virtual BossStatePriority Priority => BossStatePriority.Low;
        
        public virtual bool CanInterruptSelf => false;
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            gameObject.GetComponentInParentOrChildren(ref boss);
        }
#endif
        
        public override bool CanExitState
        {
            get
            {
                var nextState = boss.stateMachine.NextState;
                if (nextState == this)
                    return CanInterruptSelf;
                else if (Priority == BossStatePriority.Low)
                    return true;
                else
                    return nextState.Priority > Priority;
            }
        }
        
        protected void HandleMovement(Transform target, float speed)
        {
            Vector3 direction = target.position - boss.transform.position;
            direction.y = 0f;
            direction.Normalize();

            boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation, Quaternion.LookRotation(direction),
                10f * Time.deltaTime);
            
            boss.rigid.velocity = direction * speed;
        }
        
        protected bool CheckDistance(Transform target, float maxDistance)
        {
            float distance = Vector3.Distance(boss.transform.position, target.position);

            return distance <= maxDistance;
        }
    }
}