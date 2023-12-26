using System;
using UnityEngine;

namespace DR.PlayerSystem.Stats
{
    [Serializable]
    public class StaminaSystem
    {
        [SerializeField] private int staminaMax = 150;
        [SerializeField] private float staminaAmount;
        [SerializeField] private float staminaRegenAmount;
        [SerializeField] private bool canRegen = true;
        public StaminaSystem()
        {
            staminaAmount = staminaMax;
            staminaRegenAmount = 10f;
        }

        public void Update()
        {
            if(!canRegen) return;
            
            staminaAmount += staminaRegenAmount * Time.deltaTime;
            staminaAmount = Mathf.Clamp(staminaAmount, 0f, staminaMax);
        }

        public void TrySpendMana(int amount)
        {
            if (staminaAmount >= amount)
            {
                staminaAmount -= amount;
            }
        }

        public void TrySpendManaPerSecond(float deltaTime, float cost)
        {
            if (staminaAmount > 0)
            {
                canRegen = false;
                staminaAmount -= deltaTime * cost;
            }
        }

        public void SetCanRegen(bool value)
        {
            canRegen = value;
        }

        public float GetStaminaNormalized()
        {
            return staminaAmount / staminaMax;
        }

        public int GetStaminaAmount()
        {
            return (int)staminaAmount;
        }
    }
    
}