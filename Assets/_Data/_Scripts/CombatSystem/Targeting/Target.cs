using UnityEngine;
using UnityEngine.Events;

namespace DR.CombatSystem.Targeting
{
    public class Target : MonoBehaviour
    {
        public event UnityAction<Target> OnDestroyed;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}
