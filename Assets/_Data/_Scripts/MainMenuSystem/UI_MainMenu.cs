using System;
using DR.PlayerSystem;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DR.MainMenuSystem
{
    public class UI_MainMenu : MyMonobehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continuesGameButton;
        [SerializeField] private Button settingGameButton;
        [SerializeField] private Button exitButton;

        public GameObject menuWindow;
        public GameObject buttonMenu;
        [SerializeField] private UI_Setting settingWindow;
        [SerializeField] private GameObject loadingScreen;

        [SerializeField] private GameObject playerPrefab;

        protected override void Start()
        {
            base.Start();
            AuthenticationService.Instance.SignedIn += LoadData;
            
            newGameButton.onClick.AddListener(PlayerNewGame);
            
            continuesGameButton.onClick.AddListener(PlayerContinueGame);
            
            settingGameButton.onClick.AddListener((() =>
            {
                LevelManager.Instance.SetHasUIEnable(true);
                settingWindow.gameObject.SetActive(true);
                buttonMenu.SetActive(false);
            }));
            
            exitButton.onClick.AddListener(LevelManager.Instance.ExitGame);
        }

        private void OnDestroy()
        {
            AuthenticationService.Instance.SignedIn -= LoadData;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                OnEscapeEvent();
            }
        }

        private void OnEscapeEvent()
        {
            if (SceneManager.GetActiveScene().name == "MainMenuScene") return;
            
            if (settingWindow.gameObject.activeSelf)
            {
                if (SceneManager.GetActiveScene().name == "MainLevelScene")
                {
                    if(PlayerController.Instance == null) return;
                    PlayerController.Instance.cameraHolder.SetDefaultSpeed();
                }
                
                LevelManager.Instance.SetHasUIEnable(false);
                settingWindow. gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                if(IntroController.Instance == null) return;
                IntroController.Instance.videoPlayer.Play();
            }
            else
            {
                if(LevelManager.Instance.HasUIEnable) return;
                
                if (SceneManager.GetActiveScene().name == "MainLevelScene")
                {
                    if(PlayerController.Instance == null) return;
                    PlayerController.Instance.cameraHolder.SetCameraSpeed(0f, 0f);
                }
                
                LevelManager.Instance.SetHasUIEnable(true);
                settingWindow.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                if(IntroController.Instance == null) return;
                IntroController.Instance.videoPlayer.Pause();
            }
        }

        private async void PlayerNewGame()
        {
            if (PlayerController.Instance != null)
            {
                Destroy(PlayerController.Instance.gameObject);
            }
            
            ES3.DeleteFile(ES3Settings.defaultSettings);
            await CloudSaveManager.ForceDeleteAllData();
            LevelManager.Instance.NewGame();
        }

        private async void PlayerContinueGame()
        {
            loadingScreen.SetActive(true);
            Instantiate(playerPrefab, Vector3.up * 51f, Quaternion.identity);
            await PlayerController.Instance.characterCustomization.LoadData();
            await PlayerController.Instance.hotbarInventory.LoadData();
            await PlayerController.Instance.backpackInventory.LoadData();
            await PlayerController.Instance.player.playerStats.LoadData();
            
            PlayerController.Instance.playerCanvas.SetActive(true);
            LevelManager.Instance.ContinuesGame();
            //await PlayerController.Instance.player.LoadData();
        }

        public async void LoadData()
        {
            await CloudSaveManager.RetrieveSpecificData<Gender>("PlayerGender", value =>
            {
                continuesGameButton.gameObject.SetActive(value);
            });
        }

        private void OnApplicationQuit()
        {
            if(settingWindow.isExit) return;
            //SaveGameBeforeQuit();
        }

        private async void SaveGameBeforeQuit()
        {
            if (SceneManager.GetActiveScene().name == "MainLevelScene")
            {
                loadingScreen.SetActive(true);
                
                ES3.Save("PlayerPosition", PlayerController.Instance.transform.position);

                await PlayerController.Instance.player.SaveData();
                await PlayerController.Instance.player.playerStats.SaveData();
                await PlayerController.Instance.hotbarInventory.SaveData();
                await PlayerController.Instance.backpackInventory.SaveData();
            }
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadNewGameButton();
            LoadContinuesGameButton();
            LoadSettingGameButton();
            LoadExitButton();
        }

        #region Load Components

        private void LoadNewGameButton()
        {
            if(newGameButton != null) return;
            newGameButton = transform.Find("ButtonMenu").Find("NewGameButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadNewGameButton", gameObject);
        }
        private void LoadContinuesGameButton()
        {
            if(continuesGameButton != null) return;
            continuesGameButton = transform.Find("ButtonMenu").Find("ContinuesGameButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadContinuesGameButton", gameObject);
        }
        private void LoadSettingGameButton()
        {
            if(settingGameButton != null) return;
            settingGameButton = transform.Find("ButtonMenu").Find("SettingGameButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadSettingGameButton", gameObject);
        }
        private void LoadExitButton()
        {
            if(exitButton != null) return;
            exitButton = transform.Find("ButtonMenu").Find("ExitButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadExitButton", gameObject);
        }

        #endregion
        
    }
}

