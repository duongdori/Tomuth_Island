using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AnimalController : MonoBehaviour
{
    [SerializeField]
    protected Transform player; // Reference to the player's Transform

    protected virtual void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = PlayerController.Instance.transform;
    }

    protected virtual void OnEnable()
    {
        if(player != null) return;
        player = PlayerController.Instance.transform;
    }

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(player != null) return;
        player = PlayerController.Instance.transform;
    }

    public virtual void TakeDamage(float damage)
    {
        
    }
}