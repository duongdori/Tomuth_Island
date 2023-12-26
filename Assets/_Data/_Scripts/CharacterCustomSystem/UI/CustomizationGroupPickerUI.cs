using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.CharacterCustomSystem
{
    public class CustomizationGroupPickerUI : MyMonobehaviour
    {
        [SerializeField] private CharacterCustomizationUI characterCustomizationUI;
        [SerializeField] private List<CustomizationElement> customizationElements;
        [SerializeField] private TextMeshProUGUI elementName;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI elementID;

        protected override void Start()
        {
            base.Start();
            
            customizationElements[0].OnElementIDChanged += UpdateElementID;
            
            UpdateElementID();
            previousButton.onClick.AddListener((() =>
            {
                foreach (var element in customizationElements)
                {
                    element.PreviousElement();
                }
                UpdateElementID();
                characterCustomizationUI.EnableBackPreviewButton();
            }));
            nextButton.onClick.AddListener((() =>
            {
                foreach (var element in customizationElements)
                {
                    element.NextElement();
                }
                UpdateElementID();
                characterCustomizationUI.EnableBackPreviewButton();
            }));
        }

        public void Randomize()
        {
            int id = Random.Range(0, customizationElements[0].Elements.Count);
            foreach (var element in customizationElements)
            {
                element.Randomize(id);
            }
            
            UpdateElementID();
        }
        private void UpdateElementID()
        {
            elementID.SetText(customizationElements[0].ElementID.ToString());
        }
        private void UpdateElementID(int value)
        {
            elementID.SetText(value.ToString());
        }

        public void UpdateElementName(string newName)
        {
            elementName.SetText(newName);
        }

        public void SetCustomizationElements(CustomizationElement element)
        {
            customizationElements.Add(element);
        }
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
            Debug.LogWarning(transform.name + ": LoadElementIDText", gameObject);
        }
    }
}