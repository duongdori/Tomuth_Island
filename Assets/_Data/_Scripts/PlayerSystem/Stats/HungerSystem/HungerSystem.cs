using System;
using UnityEngine;
using UnityEngine.Events;

namespace DR.PlayerSystem.Stats
{
    [Serializable]
    public class HungerSystem
    {
        [SerializeField] private float hungerMax;
        [SerializeField] private float currentHunger;
        [SerializeField] private float decreaseRate;

        private PlayerStats _playerStats;
        public float CurrentHunger => currentHunger;
        
        public event UnityAction OnIncreaseHungerAmount;
        public event UnityAction OnRunOutHungerAmount;
        
        private bool _tick;
        
        public HungerSystem(float max, float rate, PlayerStats playerStats)
        {
            hungerMax = max;
            decreaseRate = rate;
            currentHunger = hungerMax;
            _playerStats = playerStats;
            _tick = false;
        }
        public void Update()
        {
            if(!_playerStats.isInGame) return;
            
            if (currentHunger <= 0 && !_tick)
            {
                _tick = true;
                OnRunOutHungerAmount?.Invoke();
                return;
            }
            
            currentHunger -= decreaseRate * Time.deltaTime;
            currentHunger = Mathf.Clamp(currentHunger, 0f, hungerMax);
        }
        
        public void ResetHunger()
        {
            currentHunger = hungerMax;
            _tick = false;
        }

        public void IncreaseHungerAmount(float amount)
        {
            currentHunger += amount;
            if (currentHunger >= hungerMax)
            {
                currentHunger = hungerMax;
            }
            
            OnIncreaseHungerAmount?.Invoke();
        }

        public void SetCurrentHunger(float amount)
        {
            currentHunger = amount;
        }
        
        public float GetHungerAmountPercent()
        {
            return currentHunger / hungerMax;
        }
    }
}