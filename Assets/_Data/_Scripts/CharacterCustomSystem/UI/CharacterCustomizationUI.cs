using DR.MainMenuSystem;
using UnityEngine;
using UnityEngine.UI;

namespace DR.CharacterCustomSystem
{
    public class CharacterCustomizationUI : MyMonobehaviour
    {
        [SerializeField] private CharacterCustomization characterCustomization;
        
        [SerializeField] private GameObject selectGenderButton;
        [SerializeField] private GameObject characterCustomPopup;

        [SerializeField] private Button selectMaleButton;
        [SerializeField] private Button selectFemaleButton;
        
        [SerializeField] private Button randomButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button backPreviewButton;
        [SerializeField] private Button saveCharacterButton;
        
        
        [SerializeField] private CustomizationPickerUI facialHairPickerUI;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            characterCustomPopup.SetActive(false);

            selectMaleButton.onClick.AddListener((() => SelectGender(Gender.Male)));
            selectFemaleButton.onClick.AddListener((() => SelectGender(Gender.Female)));
            backButton.onClick.AddListener(BackToSelectGender);
            backPreviewButton.onClick.AddListener(BackDefaultPreview);
            saveCharacterButton.onClick.AddListener(SaveChar);
        }
        
        private async void SaveChar()
        {
            LevelManager.Instance.loadingScreen.SetActive(true);
            await characterCustomization.SaveData();
            LevelManager.Instance.LoadLevel("MainLevelScene");
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadSelectGenderButton();
            LoadCharacterCustomPopup();
            LoadSelectMaleButton();
            LoadSelectFemaleButton();
            LoadRandomButton();
            LoadBackButton();
            LoadBackPreviewButton();
            LoadSaveCharacterButton();
        }

        private void SelectGender(Gender gender)
        {
            facialHairPickerUI.gameObject.SetActive(gender != Gender.Female);
            
            selectGenderButton.SetActive(false);
            characterCustomPopup.SetActive(true);
            characterCustomization.SetGender(gender);
            characterCustomization.InitBaseCharacter();
            characterCustomization.UpdateElements();
            backPreviewButton.interactable = false;
        }

        public void BackDefaultPreview()
        {
            characterCustomization.SetElementGroup(ElementGroup.FullBody);
            backPreviewButton.interactable = false;
        }

        public void EnableBackPreviewButton()
        {
            if(backPreviewButton.interactable) return;
            backPreviewButton.interactable = true;
        }

        public void BackToSelectGender()
        {
            selectGenderButton.SetActive(true);
            characterCustomPopup.SetActive(false);
            characterCustomization.SetDefault();
            characterCustomization.SetElementGroup(ElementGroup.FullBody);
        }

        #region Load Components

        private void LoadSelectGenderButton()
        {
            if(selectGenderButton != null) return;
            selectGenderButton = transform.Find("SelectGenderButtons").gameObject;
            Debug.LogWarning(transform.name + ": LoadSelectGenderButton", gameObject);
        }
        private void LoadCharacterCustomPopup()
        {
            if(characterCustomPopup != null) return;
            characterCustomPopup = transform.Find("CharacterCustomPopup").gameObject;
            Debug.LogWarning(transform.name + ": LoadCharacterCustomPopup", gameObject);
        }
        private void LoadSelectMaleButton()
        {
            if(selectMaleButton != null) return;
            selectMaleButton = selectGenderButton.transform.Find("SelectMaleButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadSelectMaleButton", gameObject);
        }
        private void LoadSelectFemaleButton()
        {
            if(selectFemaleButton != null) return;
            selectFemaleButton = selectGenderButton.transform.Find("SelectFemaleButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadSelectFemaleButton", gameObject);
        }
        private void LoadRandomButton()
        {
            if(randomButton != null) return;
            randomButton = characterCustomPopup.transform.Find("OptionButtons").Find("RandomButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadRandomButton", gameObject);
        }
        private void LoadBackButton()
        {
            if(backButton != null) return;
            backButton = characterCustomPopup.transform.Find("OptionButtons").Find("BackButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadRandomButton", gameObject);
        }
        private void LoadBackPreviewButton()
        {
            if(backPreviewButton != null) return;
            backPreviewButton = characterCustomPopup.transform.Find("OptionButtons").Find("BackPreviewButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadBackPreviewButton", gameObject);
        }
        private void LoadSaveCharacterButton()
        {
            if(saveCharacterButton != null) return;
            saveCharacterButton = characterCustomPopup.transform.Find("OptionButtons").Find("SaveCharacterButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadSaveCharacterButton", gameObject);
        }

        #endregion
        
    }
}
