using UnityEngine;

namespace DR.PlayerSystem.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField] protected HealthSystem healthSystem;

        public HealthSystem HealthSystem => healthSystem;
    }
}