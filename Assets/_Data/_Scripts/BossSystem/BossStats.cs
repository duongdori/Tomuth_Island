using System;
using DR.PlayerSystem.Stats;
using UnityEngine;

namespace DR.BossSystem
{
    public class BossStats : CharacterStats
    {
        private void Awake()
        {
            healthSystem = new HealthSystem(200);
        }
    }
}