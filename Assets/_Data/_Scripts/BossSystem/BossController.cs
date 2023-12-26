using Animancer;
using DR.BossSystem.StateMachine;
using DR.CombatSystem.Weapons;
using UnityEngine;

namespace DR.BossSystem
{
    public class BossController : MonoBehaviour
    {
        public Rigidbody rigid;
        public AnimancerComponent animancer;
        public BossBrain brain;
        public BossWeaponDamage weaponDamage;
        public WeaponHandler weaponHandler;
        public BossFieldOfView fieldOfView;
        public AnimationEventEffects animationEventEffects;
        public BossBaseState.StateMachine stateMachine;
        public BossParameters parameters;
        
        private void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
        }
    }
}
