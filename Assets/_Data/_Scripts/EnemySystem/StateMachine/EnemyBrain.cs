using DR.EnemySystem.StateMachine.State;
using UnityEngine;

namespace DR.EnemySystem.StateMachine
{
    public class EnemyBrain : MyMonobehaviour
    {
        public Enemy enemy;
        public EnemyBaseState chasingState;
        public EnemyBaseState attackState;
        public EnemyBaseState hurtState;
        public EnemyBaseState stabbedState;
        public EnemyBaseState dieState;
        public EnemyBaseState wanderState;
        public EnemySpecialDieState specialDieState;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadEnemy();
            LoadChasingState();
            LoadAttackState();
            LoadHurtState();
            LoadStabbedState();
            LoadDieState();
            LoadWanderState();
            LoadSpecialDieState();
        }

        public void SetSpecialDieState(int index)
        {
            specialDieState.SetIndex(index);
            enemy.stateMachine.ForceSetState(specialDieState);
        }

        #region Load Components

        private void LoadEnemy()
        {
            if(enemy != null) return;
            enemy = GetComponentInParent<Enemy>();
            Debug.LogWarning(transform.name + ": LoadEnemy", gameObject);
        }
        private void LoadChasingState()
        {
            if(chasingState != null) return;
            chasingState = GetComponent<EnemyChasingState>();
            Debug.LogWarning(transform.name + ": LoadChasingState", gameObject);
        }
        private void LoadAttackState()
        {
            if(attackState != null) return;
            attackState = GetComponent<EnemyAttackState>();
            Debug.LogWarning(transform.name + ": LoadAttackState", gameObject);
        }
        private void LoadHurtState()
        {
            if(hurtState != null) return;
            hurtState = GetComponent<EnemyHurtState>();
            Debug.LogWarning(transform.name + ": LoadHurtState", gameObject);
        }
        private void LoadStabbedState()
        {
            if(stabbedState != null) return;
            stabbedState = GetComponent<EnemyStabbedState>();
            Debug.LogWarning(transform.name + ": LoadStabbedState", gameObject);
        }
        private void LoadDieState()
        {
            if(dieState != null) return;
            dieState = GetComponent<EnemyDieState>();
            Debug.LogWarning(transform.name + ": LoadDieState", gameObject);
        }
        private void LoadWanderState()
        {
            if(wanderState != null) return;
            wanderState = GetComponent<EnemyWanderState>();
            Debug.LogWarning(transform.name + ": LoadWanderState", gameObject);
        }
        private void LoadSpecialDieState()
        {
            if(specialDieState != null) return;
            specialDieState = GetComponent<EnemySpecialDieState>();
            Debug.LogWarning(transform.name + ": LoadSpecialDieState", gameObject);
        }

        #endregion
        
    }
}