using DR.LoadSceneSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.MainMenuSystem
{
    public class UI_NewGameWindow : MyMonobehaviour
    {
        [SerializeField] private TMP_InputField worldNameInputField;
        [SerializeField] private Button playOfflineButton;
        [SerializeField] private Button playOnlineButton;
        [SerializeField] private Button closeButton;

        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener((() => {gameObject.SetActive(false);}));
            worldNameInputField.onValueChanged.AddListener((value =>
            {
                playOfflineButton.interactable = !string.IsNullOrEmpty(value);
                playOnlineButton.interactable = !string.IsNullOrEmpty(value);
            }));
        }

        protected override void Start()
        {
            base.Start();
            playOfflineButton.onClick.AddListener((() => {LoadSceneManager.Instance.LoadLevel("MainScene");}));
            playOnlineButton.onClick.AddListener((() =>
            {
                LoadSceneManager.Instance.LoadLevel("MainScene");
            }));
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadWorldNameInputField();
            LoadPlayOfflineButton();
            LoadPlayOnlineButton();
            LoadCloseButton();
        }

        #region Load Components

        private void LoadWorldNameInputField()
        {
            if(worldNameInputField != null) return;
            worldNameInputField = GetComponentInChildren<TMP_InputField>();
            Debug.LogWarning(transform.name + ": LoadWorldNameInputField", gameObject);
        }
        private void LoadPlayOfflineButton()
        {
            if(playOfflineButton != null) return;
            playOfflineButton = transform.Find("PlayOfflineButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadPlayOfflineButton", gameObject);
        }
        private void LoadPlayOnlineButton()
        {
            if(playOnlineButton != null) return;
            playOnlineButton = transform.Find("PlayOnlineButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadPlayOnlineButton", gameObject);
        }
        private void LoadCloseButton()
        {
            if(closeButton != null) return;
            closeButton = transform.Find("CloseButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadCloseButton", gameObject);
        }

        #endregion
        
    }

}
