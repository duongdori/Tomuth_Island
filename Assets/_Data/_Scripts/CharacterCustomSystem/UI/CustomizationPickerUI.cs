using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.CharacterCustomSystem
{
    public class CustomizationPickerUI : MyMonobehaviour
    {
        [SerializeField] private CharacterCustomizationUI characterCustomizationUI;
        [SerializeField] private CustomizationElement customizationElement;
        [SerializeField] private TextMeshProUGUI elementName;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI elementID;

        protected override void Start()
        {
            base.Start();
            customizationElement.OnElementIDChanged += UpdateElementID;
            
            //UpdateElementID(customizationElement.ElementID);
            previousButton.onClick.AddListener((() =>
            {
                customizationElement.PreviousElement();
                UpdateElementID(customizationElement.ElementID);
                characterCustomizationUI.EnableBackPreviewButton();
            }));
            nextButton.onClick.AddListener((() =>
            {
                customizationElement.NextElement();
                UpdateElementID(customizationElement.ElementID);
                characterCustomizationUI.EnableBackPreviewButton();
            }));
        }

        public void Randomize()
        {
            customizationElement.Randomize();
            UpdateElementID(customizationElement.ElementID);
        }

        private void UpdateElementID(int value)
        {
            elementID.SetText(value.ToString());
        }

        public void UpdateElementName(string newName)
        {
            elementName.SetText(newName);
        }

        public void SetCustomizationElement(CustomizationElement element)
        {
            customizationElement = element;
        }

        #region Load Components

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadCharacterCustomUI();
            LoadElementName();
            LoadPreviousButton();
            LoadNextButton();
            LoadElementIDText();
        }
        private void LoadCharacterCustomUI()
        {
            if(characterCustomizationUI != null) return;
            characterCustomizationUI = GetComponentInParent<CharacterCustomizationUI>();
            Debug.LogWarning(transform.name + ": LoadCharacterCustomUI", gameObject);
        }
        private void LoadElementName()
        {
            if(elementName != null) return;
            elementName = transform.Find("ElementName").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadElementName", gameObject);
        }
        private void LoadPreviousButton()
        {
            if(previousButton != null) return;
            previousButton = transform.Find("PickerButton").Find("PreviousButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadPreviousButton", gameObject);
        }
        private void LoadNextButton()
        {
            if(nextButton != null) return;
            nextButton = transform.Find("PickerButton").Find("NextButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadNextButton", gameObject);
        }
        private void LoadElementIDText()
        {
            if(elementID != null) return;
            elementID = transform.Find("PickerButton").Find("ElementID").GetComponentInChildren<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadElementID", gameObject);
        }

        #endregion
        
    }
}
