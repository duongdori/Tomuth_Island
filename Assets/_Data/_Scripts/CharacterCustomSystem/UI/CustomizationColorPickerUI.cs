using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.CharacterCustomSystem
{
    public class CustomizationColorPickerUI : MyMonobehaviour
    {
        [SerializeField] private CustomizationColorElement customizationColorElement;
        [SerializeField] private TMP_Text elementName;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Image colorElement;
        
        protected override void Start()
        {
            base.Start();
            customizationColorElement.OnColorElementChanged += UpdateColorElement;
            //UpdateColorElement();
            
            previousButton.onClick.AddListener((() =>
            {
                customizationColorElement.PreviousColor();
                UpdateColorElement();
            }));
            nextButton.onClick.AddListener((() =>
            {
                customizationColorElement.NextColor();
                UpdateColorElement();
            }));
        }
        
        private void UpdateElementName()
        {
            elementName.SetText(customizationColorElement.transform.name);
        }
        
        private void UpdateColorElement()
        {
            colorElement.color = customizationColorElement.GetCurrentColor();
        }
        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadElementName();
            LoadPreviousButton();
            LoadNextButton();
            LoadColorElement();
        }

        private void LoadElementName()
        {
            if(elementName != null) return;
            elementName = transform.Find("ElementName").GetComponent<TMP_Text>();
            Debug.LogWarning(transform.name + ": LoadElementName", gameObject);
        }
        private void LoadPreviousButton()
        {
            if(previousButton != null) return;
            previousButton = transform.Find("PickerButton").GetChild(0).GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadPreviousButton", gameObject);
        }
        private void LoadNextButton()
        {
            if(nextButton != null) return;
            nextButton = transform.Find("PickerButton").GetChild(1).GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadNextButton", gameObject);
        }
        private void LoadColorElement()
        {
            if(colorElement != null) return;
            colorElement = transform.Find("PickerButton").GetChild(2).GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadColorElement", gameObject);
        }
    }
}
