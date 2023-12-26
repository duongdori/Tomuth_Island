using System;
using UnityEngine;

public class DungeonGate : MonoBehaviour
{
    public static DungeonGate Instance;
    public string sceneName;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
