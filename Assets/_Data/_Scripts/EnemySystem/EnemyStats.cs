using DR.PlayerSystem.Stats;
using UnityEngine;

namespace DR.EnemySystem.Stats
{
    public class EnemyStats : CharacterStats
    {
        private void Awake()
        {
            healthSystem = new HealthSystem(100);
        }
    }
}