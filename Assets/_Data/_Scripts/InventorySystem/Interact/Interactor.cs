using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DR.InventorySystem
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private GameObject interactKey;
        [SerializeField] private bool isInteract;

        public event UnityAction OnExitInteract;
        public bool IsInteract => isInteract;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                interactKey.SetActive(true);
                isInteract = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                interactKey.SetActive(false);
                isInteract = false;
                OnExitInteract?.Invoke();
            }
        }

        public void SetActiveInteractKey(bool value)
        {
            interactKey.SetActive(value);
        }
    }
}