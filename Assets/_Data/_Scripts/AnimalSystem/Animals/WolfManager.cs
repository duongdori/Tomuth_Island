using System.Collections.Generic;
using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WolfManager : MonoBehaviour
{
    BaseFunction baseFunction;
    [SerializeField]
    List<GameObject> animals;
    [SerializeField]
    GameObject[] waypointArea;
    [SerializeField]
    GameObject wolfBoss;
    int positionArea = 0;
    bool isTargetLayer;
    // [SerializeField]
    // LayerMask playerLayer; // Layer mask for identifying the player
    [SerializeField]
    Transform player; // Reference to the player's Transform
    [SerializeField]
    float rangeMovement;
    int countActive;

    // Recovery Behaviour
    float resetDuration = 9f;
    float resetTimer;
    bool isReset;

    // Reset Boss Behaviour
    float resetBossDuration = 30f;
    float resetBossTimer;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = PlayerController.Instance.transform;
        
        baseFunction = new();
        isTargetLayer = false;
        //wolfBoss.SetActive(true);
        resetBossTimer = 0f;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(player != null) return;
        player = PlayerController.Instance.transform;
    }

    void Update()
    {
        if (countActive == 0 && !isTargetLayer)
        {
            for (int i = 0; i < waypointArea.Length; i++)
            {
                CheckLeaveColliders(i);
            }
        }
        else
        {
            DirectionReset();
            if (countActive == 0 && isReset)
            {
                ListAnimalControllerActive(positionArea);
            }
            CheckInSideColliders(positionArea);
        }
    }

    void CheckLeaveColliders(int index)
    {
        // Check colliders within a sphere around the specified position
        // Collider[] hitColliders = Physics.OverlapSphere(waypointArea[index].transform.position, rangeMovement * 1.5f, playerLayer);

        // if (hitColliders.Length > 0)
        // {
        //     positionArea = index;
        //     ListAnimalControllerActive(positionArea);
        //     isTargetLayer = true;
        // }
        if (player != null)
        {
            if (player.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(waypointArea[index].transform.position, player.position);
                if (distance < rangeMovement * 1.5f)
                {
                    positionArea = index;
                    ListAnimalControllerActive(positionArea);
                    isTargetLayer = true;
                }
            }
        }
    }

    void CheckInSideColliders(int index)
    {
        // Check colliders within a sphere around the specified position
        // Collider[] hitColliders = Physics.OverlapSphere(waypointArea[index].transform.position, rangeMovement * 1.5f, playerLayer);

        // if (hitColliders.Length <= 0)
        // {
        //     ListAnimalControllerUnActive();
        //     isTargetLayer = false;
        // }
        if (player != null)
        {
            if (player.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(waypointArea[index].transform.position, player.position);
                if (distance >= rangeMovement * 1.5f)
                {
                    ListAnimalControllerUnActive();
                    isTargetLayer = false;
                }
            }
        }

    }

    void ListAnimalControllerActive(int indexPosition)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            WolfController animalController = animals[i].GetComponent<WolfController>();
            animalController.SetIndexAnimal(i);
            animalController.SetCenterArea(waypointArea[indexPosition].transform.position, rangeMovement);
            ActiveAnimalClinent(indexPosition, i);
        }
        isReset = false;
    }
    void ActiveAnimalClinent(int indexPosition, int indexAnimal)
    {
        Vector3 positionArea = PositionActiveAnimal(waypointArea[indexPosition].transform.position);
        if (baseFunction.IsValidDestination(positionArea))
        {
            animals[indexAnimal].transform.position = positionArea;
            animals[indexAnimal].SetActive(true);
            countActive++;
        }
        else
        {
            ActiveAnimalClinent(indexPosition, indexAnimal);
        }
    }
    Vector3 PositionActiveAnimal(Vector3 createPositionAnimal)
    {
        // Generate random coordinates within the spawn area
        float randomX = Random.Range(-createPositionAnimal.x / 3.5f, createPositionAnimal.x / 3.5f);
        float randomZ = Random.Range(-createPositionAnimal.z / 3.5f, createPositionAnimal.z / 3.5f);
        // Create a random position within the spawn area
        Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ) + createPositionAnimal;
        return randomPosition;
    }

    void CheckResetBoss()
    {
        if (!wolfBoss.activeInHierarchy)
        {
            resetBossTimer += Time.deltaTime;

            if (resetBossTimer > resetBossDuration)
            {
                wolfBoss.SetActive(true);
                resetBossTimer = 0f;
            }
        }
    }
    void ListAnimalControllerUnActive()
    {
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i].SetActive(false);
            countActive--;
        }
    }

    void DirectionReset()
    {
        resetTimer += Time.deltaTime;

        if (resetTimer > resetDuration)
            isReset = true;
    }
    //--------------------------------------------------------------------------------------------------------//
    //Event Public
    public void ActiveAnimal(int index, Vector3 positionActiveAnimal)
    {
        animals[index].transform.position = positionActiveAnimal;
        animals[index].SetActive(true);
        countActive++;
    }

    // Set Model for Enemy
    public EnemyWolf GetModel(int index)
    {
        AnimalPositions animalPositions = index == 0 ? AnimalPositions.Lead : AnimalPositions.None;
        EnemyWolf enemyAnimal = new();
        if (enemyAnimal != null)
        {
            enemyAnimal.SetAnimalPositions(animalPositions);
            int level = enemyAnimal.GetAnimalPositions().Equals(AnimalPositions.Lead) ? 7 * (positionArea + 1) : Random.Range(3, 5) * (positionArea + 1);

            int point = level - 1;
            int count = 0;

            count = point - count;
            int STR = Random.Range(0, count);

            count = count - STR;
            int INT = Random.Range(0, count);

            count = count - INT;
            int VIT = Random.Range(0, count);

            count = count - VIT;
            int AGI = Random.Range(0, count);

            count = count - AGI;
            int DEX = count;

            // int wolfLayer = LayerMask.NameToLayer(nameof(EnemyWolf));
            enemyAnimal.SetModel(baseFunction.GenerateRandomKey(), nameof(EnemyWolf), level, STR, INT, VIT, AGI, DEX);
        }
        return enemyAnimal;
    }

    public EnemyWolf GetBossModel()
    {
        EnemyWolf enemyAnimal = new();
        if (enemyAnimal != null)
        {
            enemyAnimal.SetAnimalPositions(AnimalPositions.Boss);
            int level = 50;

            int point = level - 1;
            int count = 0;

            count = point - count;
            int STR = Random.Range(0, count);

            count = count - STR;
            int INT = Random.Range(0, count);

            count = count - INT;
            int VIT = Random.Range(0, count);

            count = count - VIT;
            int AGI = Random.Range(0, count);

            count = count - AGI;
            int DEX = count;

            string privateKey = baseFunction.GenerateRandomKey() + "*";
            enemyAnimal.SetModel(privateKey, nameof(EnemyWolf), level, STR, INT, VIT, AGI, DEX);
        }
        return enemyAnimal;
    }

    // Change Status
    public void ChangeStatusAnimalGroup(AnimalStatus animalStatus)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            WolfController animalController = animals[i].GetComponent<WolfController>();
            animalController.SetAnimalStatus(animalStatus);
        }
    }

    // Check Count Active In List Animal
    public void CheckCountAnimalActive()
    {
        countActive--;
        if (countActive == 0)
        {
            resetTimer = 0f;
            isReset = false;
        }
    }

    // Take Damage
    public void AnimalTakeDamge(string privateKey, float damage)
    {
        if (privateKey.Contains("*"))
        {
            // The string contains an asterisk ("*")
            WolfController animalController = wolfBoss.GetComponent<WolfController>();
            if (animalController.GetModelAnimal().GetPrivateKey().Equals(privateKey))
            {
                animalController.TakeDamage(damage);
            }
        }
        else
        {
            // The string does not contain an asterisk
            for (int i = 0; i < animals.Count; i++)
            {
                WolfController animalController = animals[i].GetComponent<WolfController>();
                if (animalController.GetModelAnimal().GetPrivateKey().Equals(privateKey))
                {
                    animalController.TakeDamage(damage);
                }
            }
        }

    }
}
