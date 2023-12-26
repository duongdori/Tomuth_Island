using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DR.MainMenuSystem
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
    
        public GameObject loadingScreen;

        [SerializeField] private GameObject mainMenuScreen;

        [SerializeField] private Image loadingBar;

        public bool isNewGame;
        [SerializeField] private bool hasUIEnable;
        public bool HasUIEnable => hasUIEnable;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void NewGame()
        {
            isNewGame = true;
            
            SetHasUIEnable(false);
            LoadLevel("IntroScene");
        }

        public void ContinuesGame(UnityAction onSceneLoaded = null)
        {
            isNewGame = false;
            SetHasUIEnable(false);
            LoadLevel("MainLevelScene", onSceneLoaded);
        }
    
        public void LoadLevel(string levelToLoad, UnityAction onSceneLoaded = null)
        {
            mainMenuScreen.SetActive(false);
            loadingScreen.SetActive(true);

            StartCoroutine(LoadLevelAsync(levelToLoad, onSceneLoaded));
        }

        public void LoadLevelNoLoading(string levelToLoad)
        {
            mainMenuScreen.SetActive(false);
            loadingScreen.SetActive(true);
            StartCoroutine(LoadLevelNoLoadingAsync(levelToLoad));

        }
    
        private IEnumerator LoadLevelAsync(string levelToLoad, UnityAction onSceneLoaded = null)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            if (loadOperation != null)
            {
                while (!loadOperation.isDone)
                {
                    float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    loadingBar.fillAmount = progressValue;
                    yield return null;
                }
                
                onSceneLoaded?.Invoke();
                loadingScreen.SetActive(false);
            }
        }
        
        private IEnumerator LoadLevelNoLoadingAsync(string levelToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            if (loadOperation != null)
            {
                while (!loadOperation.isDone)
                {
                    float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    loadingBar.fillAmount = progressValue;
                    yield return null;
                }
            }
        }
    
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void SetHasUIEnable(bool value)
        {
            hasUIEnable = value;
        }
    }
}
