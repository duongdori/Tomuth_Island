using UnityEngine;
using UnityEngine.AI;

namespace DR.PlayerSystem
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private CharacterController controller;
        [SerializeField] private NavMeshAgent agent;
    
        [SerializeField] private float drag = 0.1f;

        private Vector3 _dampingVelocity;
        private Vector3 _currentVelocity;
        private float _verticalVelocity;

        public Vector3 Movement => _currentVelocity + Vector3.up * _verticalVelocity;
        private void FixedUpdate()
        {
            if (_verticalVelocity < 0f && player.IsGrounded())
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, Vector3.zero, ref _dampingVelocity, drag);

            if(agent == null) return;
        
            if (_currentVelocity.sqrMagnitude <= Mathf.Pow(0.2f, 2f))
            {
                _currentVelocity = Vector3.zero;
                agent.enabled = true;
            }
        }

        public void AddForce(Vector3 force)
        {
            _currentVelocity += force;
        
            if(agent == null) return;
            agent.enabled = false;
        }

        public void Jump(float jumpForce)
        {
            _verticalVelocity += jumpForce;
        }
    }
}