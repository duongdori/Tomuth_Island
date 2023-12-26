using UnityEngine;
using UnityEngine.AI;

namespace DR.EnemySystem.StateMachine
{
    public class EnemyForceReceiver : MyMonobehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private CharacterController controller;
        //[SerializeField] private NavMeshAgent agent;
    
        [SerializeField] private float drag = 0.1f;

        private Vector3 _dampingVelocity;
        private Vector3 _currentVelocity;
        private float _verticalVelocity;

        public Vector3 Movement => _currentVelocity + Vector3.up * _verticalVelocity;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadEnemy();
            LoadController();
            LoadAgent();
        }

        private void FixedUpdate()
        {
            if (_verticalVelocity < 0f && enemy.IsGrounded())
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, Vector3.zero, ref _dampingVelocity, drag);

        
            if (_currentVelocity.sqrMagnitude <= Mathf.Pow(0.2f, 2f))
            {
                _currentVelocity = Vector3.zero;
            }
        }

        public void AddForce(Vector3 force)
        {
            _currentVelocity += force;
        
            //if(agent == null) return;
            //agent.enabled = false;
        }

        public void Jump(float jumpForce)
        {
            _verticalVelocity += jumpForce;
        }

        private void LoadEnemy()
        {
            if(enemy != null) return;
            enemy = GetComponent<Enemy>();
            Debug.LogWarning(transform.name + ": LoadEnemy", gameObject);
        }
        private void LoadController()
        {
            if(controller != null) return;
            controller = GetComponent<CharacterController>();
            Debug.LogWarning(transform.name + ": LoadController", gameObject);
        }
        private void LoadAgent()
        {
            // if(agent != null) return;
            // agent = GetComponent<NavMeshAgent>();
            // Debug.LogWarning(transform.name + ": LoadAgent", gameObject);
        }
    }
}