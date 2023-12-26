using TMPro;
using UnityEngine;

namespace DR.CombatSystem
{
    public class DamagePopup : MonoBehaviour
    {
        public static DamagePopup Create(Transform damagePopupPrefab, Vector3 position, int damageAmount)
        {
            Transform damagePopupTransform = Instantiate(damagePopupPrefab, position, Quaternion.identity);

            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            if (damagePopup != null)
            {
                damagePopup.Setup(damageAmount);
            }

            return damagePopup;
        }

        private static int sortingOrder;
        
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private float moveYSpeed = 2f;
        [SerializeField] private float disappearTimer;
        [SerializeField] private float disappearSpeed = 3f;
        [SerializeField] private float increaseScaleAmount = 1f;
        [SerializeField] private float decreaseScaleAmount = 1f;
        
        private float disappearTimerMax = 1f;
        private Color textColor;
        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
            transform.forward = Camera.main.transform.forward;
        }

        private void Update()
        {
            transform.position += new Vector3(0f, moveYSpeed, 0f) * Time.deltaTime;

            if (disappearTimer > disappearTimerMax * 0.5f)
            {
                //First half of the popup lifetime
                transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
            }
            else
            {
                //Second half of the popup lifetime
                transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
            }
            
            disappearTimer -= Time.deltaTime;
            
            if (disappearTimer < 0)
            {
                //Start disappearing
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
                if (textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void Setup(int damageAmount)
        {
            textMesh.SetText(damageAmount.ToString());
            textColor = textMesh.color;
            disappearTimer = disappearTimerMax;
            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;
        }
    }
}