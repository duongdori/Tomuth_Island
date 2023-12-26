using UnityEngine;

namespace DR.PlayerSystem
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private GameObject rightClickIcon;

        public void SetActiveRightClickIcon(bool active)
        {
            rightClickIcon.SetActive(active);
        }
    }
}