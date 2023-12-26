using DR.MainMenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DR.GameTutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if(scene.name != "MainLevelScene") return;
        
            gameObject.SetActive(LevelManager.Instance.isNewGame);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameObject.SetActive(false);
            }
        }
    }
}
