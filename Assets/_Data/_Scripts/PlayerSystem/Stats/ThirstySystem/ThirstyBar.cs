using UnityEngine;
using UnityEngine.UI;

namespace DR.PlayerSystem.Stats
{
    public class ThirstyBar : MonoBehaviour
    {
        [SerializeField] private Image thirstyBar;
        [SerializeField] private PlayerStats playerStats;
        
        private void Awake()
        {
            thirstyBar = transform.Find("ThirstyBar").GetComponent<Image>();
        }
        
        private void Update()
        {
            playerStats.ThirstySystem.Update();

            thirstyBar.fillAmount = playerStats.ThirstySystem.GetThirstyAmountPercent();
        }
    }
}