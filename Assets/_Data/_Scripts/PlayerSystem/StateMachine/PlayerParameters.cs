using DR.EnemySystem;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine
{
    [System.Serializable]
    public class PlayerParameters
    {
        [Header("Locomotion Variables")] 
        public float movementSpeed;
        public float sprintSpeed;
        public float rotationDamping;
        public float jumpForce;
        public float airBorneSpeed;
        public float crouchMoveSpeed;
        public float targetMovementSpeed;
        public float rollingForce;
        public bool isDungeon;
        public bool isDie;
        
        [Header("Stamina Cost")] 
        public int jumpCost;
        public int rollCost;
        public int attackCost;
        public int sprintCost;
        
        [Header("BackStab")] 
        public Transform attackRaycastStartPoint;
        public LayerMask backStabLayer;
        public LayerMask enemyLayer;
        public Enemy enemy;
        
        [Header("Ground Check Variables")] 
        public Transform groundCheckPosition;
        public float groundCheckRadius = 0.3f;
        public float groundCheckMaxDistance = 0.5f;
        public LayerMask groundCheckLayerMask;


        private float _tempMovementSpeed;
        private float _tempSprintSpeed;
        private float _tempJumpForce;
        private float _tempRollingForce;
        private float _tempCrouchMoveSpeed;
        
        
        public void SetDefaultMovement()
        {
            movementSpeed = _tempMovementSpeed;
            sprintSpeed = _tempSprintSpeed;
            jumpForce = _tempJumpForce;
            rollingForce = _tempRollingForce;
            crouchMoveSpeed = _tempCrouchMoveSpeed;
        }

        public void SetTempMovement()
        {
            _tempMovementSpeed = movementSpeed;
            _tempSprintSpeed = sprintSpeed;
            _tempJumpForce = jumpForce;
            _tempRollingForce = rollingForce;
            _tempCrouchMoveSpeed = crouchMoveSpeed;
        }
    }
}