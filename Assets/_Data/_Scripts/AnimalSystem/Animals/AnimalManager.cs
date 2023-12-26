using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    BaseFunction baseFunction;
    WolfManager wolfManager;
    BoarManager boarManager;
    SpiderManager spiderManager;

    void Awake()
    {
        Application.targetFrameRate = 60;
        wolfManager = FindAnyObjectByType<WolfManager>();
        boarManager = FindAnyObjectByType<BoarManager>();
        spiderManager = FindAnyObjectByType<SpiderManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        baseFunction = new();
        baseFunction.AddEnemyTag(nameof(EnemyWolf));
        baseFunction.AddEnemyTag(nameof(EnemyBoar));
        baseFunction.AddEnemyTag(nameof(EnemySpider));
    }

    public void TakeDamgeAnimal(string tagKey, string privateKey, float damage)
    {
        switch (tagKey)
        {
            case nameof(EnemyWolf):
                wolfManager.AnimalTakeDamge(privateKey, damage);
                break;
            case nameof(EnemyBoar):
                boarManager.AnimalTakeDamge(privateKey, damage);
                break;
            case nameof(EnemySpider):
                spiderManager.AnimalTakeDamge(privateKey, damage);
                break;
        }
    }
}
