using UnityEngine;

namespace DR.BossSystem
{
    [System.Serializable]
    public class BossParameters
    {
        public Transform startPoint;
        public float moveSpeed;
        public float walkSpeed;
        public float distanceAttack01;
        public float distanceAttack02;
    }
}