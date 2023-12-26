using DR.MainMenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private GameObject uiRight;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (!LevelManager.Instance.isNewGame)
        {
            uiRight.SetActive(false);
        }
    }
}
