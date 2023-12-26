using System;
using UnityEngine;
using UnityEngine.Events;

namespace DR.PlayerSystem.Stats
{
    [Serializable]
    public class ThirstySystem
    {
        [SerializeField] private float thirstyMax;
        [SerializeField] private float currentThirsty;
        [SerializeField] private float decreaseRate;

        private PlayerStats _playerStats;
        public float CurrentThirsty => currentThirsty;
        public event UnityAction OnIncreaseThirstyAmount;
        public event UnityAction OnRunOutThirstyAmount;

        private bool _tick;
        public ThirstySystem(float max, float rate, PlayerStats playerStats)
        {
            thirstyMax = max;
            decreaseRate = rate;
            currentThirsty = thirstyMax;
            _playerStats = playerStats;
            _tick = false;
        }
        public void Update()
        {
            if(!_playerStats.isInGame) return;
            
            if (currentThirsty <= 0 && !_tick)
            {
                _tick = true;
                OnRunOutThirstyAmount?.Invoke();
                return;
            }
            
            currentThirsty -= decreaseRate * Time.deltaTime;
            currentThirsty = Mathf.Clamp(currentThirsty, 0f, thirstyMax);
        }

        public void IncreaseThirstyAmount(float amount)
        {
            currentThirsty += amount;
            if (currentThirsty >= thirstyMax)
            {
                currentThirsty = thirstyMax;
            }
            
            OnIncreaseThirstyAmount?.Invoke();
        }

        public void ResetThirsty()
        {
            currentThirsty = thirstyMax;
            _tick = false;
        }

        public void SetCurrentThirsty(float amount)
        {
            currentThirsty = amount;
        }
        
        public float GetThirstyAmountPercent()
        {
            return currentThirsty / thirstyMax;
        }
    }
}