using DR.PlayerSystem.Stats;
using UnityEngine;

namespace DR.ResourceSystem
{
    public class ResourceStats : CharacterStats
    {
        private void Awake()
        {
            healthSystem = new HealthSystem(50);
        }
    }
}

