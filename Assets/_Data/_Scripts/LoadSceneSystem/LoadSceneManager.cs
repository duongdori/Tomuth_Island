using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DR.LoadSceneSystem
{
    public class LoadSceneManager : MonoBehaviour
    {
        public static LoadSceneManager Instance { get; private set; }
    
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject mainMenuScreen;
        [SerializeField] private Image loadingBar;
    
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
    
        public void LoadLevel(string levelToLoad)
        {
            mainMenuScreen.SetActive(!mainMenuScreen.activeSelf);
            loadingScreen.SetActive(true);

            StartCoroutine(LoadLevelAsync(levelToLoad));

        }
        private IEnumerator LoadLevelAsync(string levelToLoad)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                loadingBar.fillAmount = progressValue;
                yield return null;
            }
            loadingScreen.SetActive(false);
        }
    }
}