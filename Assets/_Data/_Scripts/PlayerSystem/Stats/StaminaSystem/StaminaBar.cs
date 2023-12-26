using UnityEngine;
using UnityEngine.UI;

namespace DR.PlayerSystem.Stats
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private Image staminaBar;
        [SerializeField] private PlayerStats playerStats;
        private void Awake()
        {
            staminaBar = transform.Find("StaminaBar").GetComponent<Image>();
        }

        private void Update()
        {
            playerStats.StaminaSystem.Update();

            staminaBar.fillAmount = playerStats.StaminaSystem.GetStaminaNormalized();
        }
    }
}
