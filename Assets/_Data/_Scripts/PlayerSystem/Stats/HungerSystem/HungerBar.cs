using UnityEngine;
using UnityEngine.UI;

namespace DR.PlayerSystem.Stats
{
    public class HungerBar : MonoBehaviour
    {
        [SerializeField] private Image hungerBar;
        [SerializeField] private PlayerStats playerStats;
        
        private void Awake()
        {
            hungerBar = transform.Find("HungerBar").GetComponent<Image>();
        }
        
        private void Update()
        {
            playerStats.HungerSystem.Update();

            hungerBar.fillAmount = playerStats.HungerSystem.GetHungerAmountPercent();
        }
    }
}