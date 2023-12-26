using System;
using UnityEngine;
using UnityEngine.Events;

namespace DR.PlayerSystem.Stats
{
    [Serializable]
    public class HealthSystem
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        public int CurrentHealth => currentHealth;
        
        private bool _isInvulnerable;

        public event UnityAction OnTakeDamage;
        public event UnityAction OnDie;
        
        public event UnityAction<float> OnHealthPercentChanged;
        public event UnityAction<int> OnHealthChanged;
        public event UnityAction OnHealed;

        public bool IsDead => currentHealth == 0;

        public HealthSystem()
        {
            currentHealth = maxHealth;
        }
        
        public HealthSystem(int healthMax)
        {
            maxHealth = healthMax;
            currentHealth = maxHealth;
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            OnHealthPercentChanged?.Invoke(GetHealthPercent());
        }
        public void SetInvulnerable(bool isInvulnerable)
        {
            _isInvulnerable = isInvulnerable;
        }
    
        public void DealDamage(int damage)
        {
            if(currentHealth == 0) return;
            if(_isInvulnerable) return;
        
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            
            OnHealthChanged?.Invoke(currentHealth);
            OnHealthPercentChanged?.Invoke(GetHealthPercent());
            OnTakeDamage?.Invoke();

            if (currentHealth == 0)
            {
                OnDie?.Invoke();
            }
        }
        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            OnHealed?.Invoke();
            OnHealthChanged?.Invoke(currentHealth);
            OnHealthPercentChanged?.Invoke(GetHealthPercent());
        }
        
        public float GetHealthPercent()
        {
            return (float)currentHealth / maxHealth;
        }

        public void SetCurrentHealth(int amount)
        {
            currentHealth = amount;
            OnHealed?.Invoke();
            OnHealthPercentChanged?.Invoke(GetHealthPercent());
        }
    }
}
