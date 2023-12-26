using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderManager : MonoBehaviour
{
    BaseFunction baseFunction;
    [SerializeField]
    List<GameObject> animals;
    [SerializeField]
    GameObject[] waypointArea;
    int positionArea = 0;
    bool isTargetLayer;
    [SerializeField]
    Transform player; // Reference to the player's Transform
    [SerializeField]
    float rangeMovement;
    int countActive;

    // Recovery Behaviour
    float resetDuration = 9f;
    float resetTimer;
    bool isReset;

    // Start is called before the first frame update
    void Start()
    {
        baseFunction = new();
        isTargetLayer = false;
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
            SpiderController animalController = animals[i].GetComponent<SpiderController>();
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
    public EnemySpider GetModel(int index)
    {
        AnimalPositions animalPositions = index == 0 ? AnimalPositions.Lead : AnimalPositions.None;
        EnemySpider enemyAnimal = new();
        if (enemyAnimal != null)
        {
            enemyAnimal.SetAnimalPositions(animalPositions);
            int level = enemyAnimal.GetAnimalPositions().Equals(AnimalPositions.Lead) ? 7 * (positionArea + 1) : Random.Range(1, 3) * (positionArea + 1);

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

            enemyAnimal.SetModel(baseFunction.GenerateRandomKey(), nameof(EnemySpider), level, STR, INT, VIT, AGI, DEX);
        }
        return enemyAnimal;
    }

    // Change Status
    public void ChangeStatusAnimalGroup(AnimalStatus animalStatus)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            SpiderController animalController = animals[i].GetComponent<SpiderController>();
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
        for (int i = 0; i < animals.Count; i++)
        {
            SpiderController animalController = animals[i].GetComponent<SpiderController>();
            if (animalController.GetModelAnimal().GetPrivateKey().Equals(privateKey))
            {
                animalController.TakeDamage(damage);
            }
        }
    }
}
