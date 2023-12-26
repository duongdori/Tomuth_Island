using System.Collections.Generic;
using DR.CombatSystem;
using DR.PlayerSystem;
using DR.PlayerSystem.Stats;
using UnityEngine;

namespace DR.EnemySystem
{
    public class EnemyWeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private int currentDamage;
        [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();
    
        private void OnEnable()
        {
            alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other == myCollider) return;
        
            if(alreadyCollidedWith.Contains(other)) return;
            alreadyCollidedWith.Add(other);

            if (other.TryGetComponent(out PlayerStats playerStats))
            {
                if(!playerStats.enabled) return;
            
                DamagePopup.Create(PlayerController.Instance.damagePopupPrefab.transform, other.ClosestPoint(transform.position),
                    currentDamage);
            
                playerStats.HealthSystem.DealDamage(currentDamage);
            }
        }

        public void SetAttack(int damage)
        {
            currentDamage = damage;
        }
    }
}
