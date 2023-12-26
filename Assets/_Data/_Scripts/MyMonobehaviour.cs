using UnityEngine;

public class MyMonobehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void Start()
    {
        //For override
    }

    protected virtual void Reset()
    {
        ResetValue();
        this.LoadComponents();
    }

    protected virtual void ResetValue()
    {
        //For override
    }

    protected virtual void LoadComponents()
    {
        //For override   
    }
}
