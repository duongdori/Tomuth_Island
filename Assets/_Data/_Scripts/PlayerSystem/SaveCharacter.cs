using DR.CharacterCustomSystem;
using DR.InputSystem;
using DR.MainMenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem
{
    public class SaveCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterCustomization characterCustomization;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject customChar;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if(characterCustomization == null) return;
            if (scene.name == "MainLevelScene")
            {
                InputHandler.Instance.enabled = true;
                canvas.SetActive(true);
            }
            else if (scene.name == "CharacterCustomScene")
            {
                InputHandler.Instance.enabled = false;
                canvas.SetActive(false);
            }
        }

        public void SaveChar()
        {
            characterCustomization.SaveData();
            LevelManager.Instance.LoadLevel("MainLevelScene");
        }
    }
}
