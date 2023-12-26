using UnityEngine;
using UnityEngine.UI;

namespace DR.BossSystem
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image damageBarImage;

        private float damageHealthShrinkTimerMax = 1f;
        private float damageHealthShrinkTimer;

        public BossStats bossStats;
        
        private void Awake()
        {
            healthBar = transform.Find("HealthBar").GetComponent<Image>();
            damageBarImage = transform.Find("DamageBar").GetComponent<Image>();
        }
        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            SetHealthSystemBarSize(bossStats.HealthSystem.GetHealthPercent());

            damageBarImage.fillAmount = healthBar.fillAmount;
            
            bossStats.HealthSystem.OnHealthPercentChanged += SetHealthSystemBarSize;
            bossStats.HealthSystem.OnTakeDamage += HealthSystemSystemOnDamaged;
            bossStats.HealthSystem.OnHealed += HealthSystemSystemOnHealed;
        }
        
        private void HealthSystemSystemOnDamaged()
        {
            damageHealthShrinkTimer = damageHealthShrinkTimerMax;
        }
        private void HealthSystemSystemOnHealed()
        {
            damageBarImage.fillAmount = bossStats.HealthSystem.GetHealthPercent();
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