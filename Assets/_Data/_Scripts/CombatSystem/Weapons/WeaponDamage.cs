using System.Collections.Generic;
using DR.EnemySystem;
using DR.PlayerSystem;
using DR.PlayerSystem.Stats;
using DR.ResourceSystem;
using UnityEngine;

namespace DR.CombatSystem.Weapons
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private int currentDamage;
        [SerializeField] private float currentKnockBack;
        [SerializeField] private WeaponSlotManger weaponSlotManager;
        [SerializeField] private List<Collider> alreadyCollidedWith = new List<Collider>();

        public Enemy enemy;
        public bool canSpecialAttack;
        private void OnEnable()
        {
            alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other == myCollider) return;
        
            if(alreadyCollidedWith.Contains(other)) return;
            alreadyCollidedWith.Add(other);
        
            if (other.TryGetComponent(out CharacterStats characterStats))
            {
                if(!characterStats.enabled) return;
            
                if (other.TryGetComponent(out ResourceHolder holder))
                {
                    if (!holder.toolsNeeded.Contains(weaponSlotManager.currentWeapon))
                    {
                        currentDamage = 0;
                    }

                    ParticleSystem effect = Instantiate(holder.ResourceManager.effectFX, other.ClosestPoint(transform.position), Quaternion.identity);
                    effect.Play();
                }

                if (other.TryGetComponent(out Enemy enemy))
                {
                    this.enemy = enemy;
                }
            
                DamagePopup.Create(PlayerController.Instance.damagePopupPrefab.transform, other.ClosestPoint(transform.position),
                    currentDamage);

                weaponSlotManager.slotActive.DeductDurability();
                Debug.Log(other.name + ": Damaged " + currentDamage);

                characterStats.HealthSystem.DealDamage(currentDamage);
                
            }

            // if (other.TryGetComponent(out AnimalController animalController))
            // {
            //     DamagePopup.Create(PlayerController.Instance.damagePopupPrefab.transform, other.ClosestPoint(transform.position),
            //         currentDamage);
            //
            //     weaponSlotManager.slotActive.DeductDurability();
            //     Debug.Log(other.name + ": Damaged " + currentDamage);
            //
            //     animalController.TakeDamage(currentDamage);
            // }
        }

        public void SetAttack(int damage, float knockBack)
        {
            currentDamage = damage;
            currentKnockBack = knockBack;
        }

        public int GetCurrentDamage()
        {
            return currentDamage;
        }
    }
}
