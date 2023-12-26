using DR.PlayerSystem;
using DR.SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DR.MainMenuSystem
{
    public class UI_Setting : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private UI_MainMenu mainMenuUI;
        [SerializeField] private GameObject loadingScreen;

        [SerializeField] private Slider musicVolumeSetting;
        [SerializeField] private Slider soundFXVolumeSetting;

        public bool isExit;
        private void Awake()
        {
            closeButton.onClick.AddListener(CloseSettingWindow);
            exitGameButton.onClick.AddListener(ExitGame);
            isExit = false;
        }
        

        private void OnEnable()
        {
            exitGameButton.gameObject.SetActive(SceneManager.GetActiveScene().name != "MainMenuScene");
        }

        private void Start()
        {
            SoundManager.Instance.ChangeMusicVolume(musicVolumeSetting.value);
            SoundManager.Instance.ChangeSfxVolume(soundFXVolumeSetting.value);
            
            musicVolumeSetting.onValueChanged.AddListener((value => SoundManager.Instance.ChangeMusicVolume(value)));
            soundFXVolumeSetting.onValueChanged.AddListener((value => SoundManager.Instance.ChangeSfxVolume(value)));
        }

        private void CloseSettingWindow()
        {
            if (SceneManager.GetActiveScene().name != "MainMenuScene" && SceneManager.GetActiveScene().name != "CharacterCustomScene")
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.cameraHolder.SetDefaultSpeed();
                }
                
            }
            
            if (IntroController.Instance != null)
            {
                IntroController.Instance.videoPlayer.Play();
            }
            
            LevelManager.Instance.SetHasUIEnable(false);
            mainMenuUI.buttonMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        private async void ExitGame()
        {
            // PlayerController.Instance.cameraHolder.SetDefaultSpeed();
            LevelManager.Instance.SetHasUIEnable(false);
            loadingScreen.SetActive(true);
            isExit = true;
            if (SceneManager.GetActiveScene().name == "MainLevelScene")
            {
                await PlayerController.Instance.player.SaveData();
                await PlayerController.Instance.player.playerStats.SaveData();
                await PlayerController.Instance.hotbarInventory.SaveData();
                await PlayerController.Instance.backpackInventory.SaveData();
                
                ES3.Save("PlayerPosition", PlayerController.Instance.transform.position);
            }
            
            LevelManager.Instance.ExitGame();
            //LevelManager.Instance.LoadLevel("MainMenuScene", OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            if (PlayerController.Instance != null)
            {
                Destroy(PlayerController.Instance.gameObject);
            }
            
            mainMenuUI.menuWindow.SetActive(true);
            mainMenuUI.LoadData();
            gameObject.SetActive(false);
        }
    }
}
