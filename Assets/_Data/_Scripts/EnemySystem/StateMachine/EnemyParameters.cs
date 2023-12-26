using System;
using UnityEngine;

namespace DR.EnemySystem.StateMachine
{
    [Serializable]
    public class EnemyParameters
    {
        public float movementSpeed;
        public float attackRange;
        public float playerChasingRange;
        public float stoppingDistance = 2f;
        public int damageAmount = 10;
        
        [Header("Wander Variables")]
        public LayerMask wanderCheckLayer;
        public float wanderRadius = 15f;
        
        [Header("Ground Check Variables")] 
        public Transform groundCheckPosition;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundCheckLayerMask;
    }
}