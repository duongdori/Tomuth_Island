using UnityEngine;
using UnityEngine.UI;

namespace DR.PlayerSystem.Stats
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image damageBarImage;

        private float damageHealthShrinkTimerMax = 1f;
        private float damageHealthShrinkTimer;
        public PlayerStats playerStats;

        private void Awake()
        {
            healthBar = transform.Find("HealthBar").GetComponent<Image>();
            damageBarImage = transform.Find("DamageBarImage").GetComponent<Image>();
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            SetHealthSystemBarSize(playerStats.HealthSystem.GetHealthPercent());

            damageBarImage.fillAmount = healthBar.fillAmount;
            
            playerStats.HealthSystem.OnHealthPercentChanged += SetHealthSystemBarSize;
            playerStats.HealthSystem.OnTakeDamage += HealthSystemSystemOnDamaged;
            playerStats.HealthSystem.OnHealed += HealthSystemSystemOnHealed;
        }

        private void HealthSystemSystemOnDamaged()
        {
            damageHealthShrinkTimer = damageHealthShrinkTimerMax;
        }
        private void HealthSystemSystemOnHealed()
        {
            damageBarImage.fillAmount = playerStats.HealthSystem.GetHealthPercent();
        }

        private void Update()
        {
            damageHealthShrinkTimer -= Time.deltaTime;
            if (damageHealthShrinkTimer < 0)
            {
                if (healthBar.fillAmount < damageBarImage.fillAmount)
                {
                    float shrinkSpeed = 1f;
                    damageBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
                }
            }
        }

        private void SetHealthSystemBarSize(float healthPercent)
        {
            healthBar.fillAmount = healthPercent;
        }
    }
}