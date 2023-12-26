using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float timer = 3f;

    private void Start()
    {
        Invoke(nameof(DestroyFX), timer);
    }

    private void DestroyFX()
    {
        Destroy(gameObject);
    }
}
