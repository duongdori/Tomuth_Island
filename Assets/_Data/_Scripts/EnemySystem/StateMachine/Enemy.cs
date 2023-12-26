using Animancer;
using DR.CombatSystem;
using DR.CombatSystem.Targeting;
using DR.CombatSystem.Weapons;
using DR.EnemySystem.StateMachine;
using DR.EnemySystem.StateMachine.State;
using DR.EnemySystem.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace DR.EnemySystem
{
    public class Enemy : MyMonobehaviour
    {
        public CharacterController controller;
        public NavMeshAgent agent;
        public AnimancerComponent animancer;
        public EnemyForceReceiver forceReceiver;
        public EnemyBrain brain;
        public EnemyStats enemyStats;
        public EnemyFieldOfView fieldOfView;
        public Target target;
        public BackStabCollider backStabCollider;
        public WeaponHandler weaponHandler;
        public EnemyWeaponDamage weaponDamage;
        public RedirectRootMotionToCharacterController redirectRootMotion;
        public EnemyTargeter targeter;
        
        public EnemyBaseState.StateMachine stateMachine;
        public EnemyParameters parameters;
        
        protected override void Awake()
        {
            base.Awake();
            stateMachine.InitializeAfterDeserialize();
        }

        protected override void Start()
        {
            base.Start();
            enemyStats.HealthSystem.OnTakeDamage += () =>
            {
                stateMachine.ForceSetState(brain.hurtState);
            };
            enemyStats.HealthSystem.OnDie += () => { stateMachine.TrySetState(brain.dieState); };
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadController();
            LoadAgent();
            LoadAnimancer();
            LoadForceReceiver();
            LoadBrain();
            LoadEnemyStats();
            LoadEnemyFieldOfView();
            LoadTarget();
            LoadBackStabCollider();
            LoadWeaponHandler();
            LoadWeaponDamage();
            LoadRedirectRootMotion();
            LoadGroundCheck();
            //stateMachine.DefaultState = GetComponent<EnemyIdleState>();
        }

        private void OnEnable()
        {
            enemyStats.enabled = true;
            controller.enabled = true;
            agent.enabled = true;
            redirectRootMotion.enabled = true;
            //backStabCollider.backStabBoxCollider.enabled = true;
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere(parameters.groundCheckPosition.position, parameters.groundCheckRadius, parameters.groundCheckLayerMask);
        }
        
        public void FacePlayer()
        {
            if(fieldOfView.PlayerRef == null) return;
        
            Vector3 lookPos = fieldOfView.PlayerRef.transform.position - transform.position;
            lookPos.y = 0f;
        
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(lookPos), 10f * Time.deltaTime);
        }

        public void FaceTarget()
        {
            if(fieldOfView.PlayerRef == null) return;
            Vector3 lookPos = fieldOfView.PlayerRef.transform.position - transform.position;
            lookPos.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }

        #region Load Components

        private void LoadController()
        {
            if(controller != null) return;
            controller = GetComponent<CharacterController>();
            Debug.LogWarning(transform.name + ": LoadController", gameObject);
        }
        private void LoadAgent()
        {
            if(agent != null) return;
            agent = GetComponent<NavMeshAgent>();
            Debug.LogWarning(transform.name + ": LoadAgent", gameObject);
        }
        private void LoadAnimancer()
        {
            if(animancer != null) return;
            animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.LogWarning(transform.name + ": LoadAnimancer", gameObject);
        }
        private void LoadForceReceiver()
        {
            if(forceReceiver != null) return;
            forceReceiver = GetComponent<EnemyForceReceiver>();
            Debug.LogWarning(transform.name + ": LoadForceReceiver", gameObject);
        }
        private void LoadBrain()
        {
            if(brain != null) return;
            brain = GetComponentInChildren<EnemyBrain>();
            Debug.LogWarning(transform.name + ": LoadBrain", gameObject);
        }
        private void LoadEnemyStats()
        {
            if(enemyStats != null) return;
            enemyStats = GetComponent<EnemyStats>();
            Debug.LogWarning(transform.name + ": LoadEnemyStats", gameObject);
        }
        private void LoadEnemyFieldOfView()
        {
            if(fieldOfView != null) return;
            fieldOfView = GetComponent<EnemyFieldOfView>();
            Debug.LogWarning(transform.name + ": LoadEnemyFieldOfView", gameObject);
        }
        private void LoadTarget()
        {
            if(target != null) return;
            target = GetComponent<Target>();
            Debug.LogWarning(transform.name + ": LoadTarget", gameObject);
        }
        private void LoadBackStabCollider()
        {
            if(backStabCollider != null) return;
            backStabCollider = GetComponentInChildren<BackStabCollider>();
            Debug.LogWarning(transform.name + ": LoadBackStabCollider", gameObject);
        }
        private void LoadWeaponHandler()
        {
            if(weaponHandler != null) return;
            weaponHandler = GetComponent<WeaponHandler>();
            Debug.LogWarning(transform.name + ": LoadWeaponHandler", gameObject);
        }
        private void LoadWeaponDamage()
        {
            if(weaponDamage != null) return;
            weaponDamage = GetComponentInChildren<EnemyWeaponDamage>();
            weaponDamage.gameObject.SetActive(false);
            Debug.LogWarning(transform.name + ": LoadWeaponDamage", gameObject);
        }
        
        private void LoadRedirectRootMotion()
        {
            if(redirectRootMotion != null) return;
            redirectRootMotion = GetComponentInChildren<RedirectRootMotionToCharacterController>();
            Debug.LogWarning(transform.name + ": LoadRedirectRootMotion", gameObject);
        }
        private void LoadGroundCheck()
        {
            if(parameters.groundCheckPosition != null) return;
            parameters.groundCheckPosition = transform.Find("GroundCheck");
            parameters.groundCheckLayerMask = LayerMask.GetMask("Ground");
            Debug.LogWarning(transform.name + ": LoadGroundCheck", gameObject);
        }

        #endregion
        
        
    }
}