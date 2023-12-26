using DR.PlayerSystem;
using UnityEngine;

namespace DR.EnemySystem.StateMachine
{
    public class EnemyTargeter : MonoBehaviour
    {
        public Player player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                this.player = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                this.player = null;
            }
        }
    }
}