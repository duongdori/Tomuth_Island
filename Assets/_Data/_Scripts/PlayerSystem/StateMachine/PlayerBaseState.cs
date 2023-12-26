using Animancer;
using Animancer.FSM;
using DR.CombatSystem.Targeting;
using DR.EnemySystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine
{
    public class PlayerBaseState : StateBehaviour
    {
        [System.Serializable]
        public class StateMachine : StateMachine<PlayerBaseState>.WithDefault{ }

        public Player player;

        public virtual PlayerStatePriority Priority => PlayerStatePriority.Low;
        
        public virtual bool CanInterruptSelf => false;
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            gameObject.GetComponentInParentOrChildren(ref player);
        }
#endif
        
        public override bool CanExitState
        {
            get
            {
                var nextState = player.stateMachine.NextState;
                if (nextState == this)
                    return CanInterruptSelf;
                else if (Priority == PlayerStatePriority.Low)
                    return true;
                else
                    return nextState.Priority > Priority;
            }
        }

        protected virtual void OnEnable()
        {
        }

        

        protected virtual void Update()
        {
            
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 movement, float deltaTime)
        {
            player.controller.Move((movement + player.forceReceiver.Movement) * deltaTime);
        }
        
        protected void FaceTarget()
        {
            Target currentTarget = player.targeter.CurrentTarget;
            if(currentTarget == null) return;

            Vector3 lookPos = currentTarget.transform.position - player.transform.position;
            lookPos.y = 0f;

            player.transform.rotation = Quaternion.LookRotation(lookPos);
        }
        
        protected void ReturnToLocomotion()
        {
            if (player.targeter.CurrentTarget != null)
            {
                player.stateMachine.ForceSetState(player.brain.targetState);
            }
            else
            {
                player.stateMachine.ForceSetState(player.stateMachine.DefaultState);
            }
        }

        protected virtual bool AttemptSpecialAttack()
        {
            if (Physics.Raycast(player.parameters.attackRaycastStartPoint.position,
                    player.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1f,
                    player.parameters.enemyLayer))

            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if(enemy == null) return false;

                Vector3 dirFromEnemyToPlayer = (player.transform.position - hit.transform.position).normalized;
                float dot = Vector3.Dot(hit.transform.forward, dirFromEnemyToPlayer);
                float dotOffset = 0.2f;
                if (dot > 1 - dotOffset)
                {
                    player.parameters.enemy = enemy;
                    return true;
                }
            }
            return false;
        }
        
        protected virtual bool AttemptBackStab()
        {
            if (Physics.Raycast(player.parameters.attackRaycastStartPoint.position,
                    player.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1f,
                    player.parameters.enemyLayer))
            {
                Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                if (enemy == null) return false;
                
                Vector3 dirFromEnemyToPlayer = (player.transform.position - hit.transform.position).normalized;
                float dot = Vector3.Dot(hit.transform.forward, dirFromEnemyToPlayer);
                float dotOffset = 0.2f;
                if (dot < -1 + dotOffset)
                {
                    player.parameters.enemy = enemy;
                    player.transform.position = enemy.backStabCollider.backStabberStandPoint.position;
                    Vector3 rotationDirection = player.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - player.transform.position;
                    rotationDirection.y = 0f;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
                    player.transform.rotation = targetRotation;
                    return true;
                }
            }

            return false;
        }
        // protected virtual bool AttemptBackStab()
        // {
        //     if (Physics.Raycast(player.parameters.attackRaycastStartPoint.position,
        //             player.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 0.5f,
        //             player.parameters.backStabLayer))
        //     {
        //         Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
        //         if (enemy == null) return false;
        //         player.parameters.enemy = enemy;
        //         player.transform.position = enemy.backStabCollider.backStabberStandPoint.position;
        //
        //         Vector3 rotationDirection = player.transform.root.eulerAngles;
        //         rotationDirection = hit.transform.position - player.transform.position;
        //         rotationDirection.y = 0f;
        //         rotationDirection.Normalize();
        //         Quaternion tr = Quaternion.LookRotation(rotationDirection);
        //         Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 500 * Time.deltaTime);
        //         player.transform.rotation = targetRotation;
        //         return true;
        //     }
        //
        //     return false;
        // }
    }
}