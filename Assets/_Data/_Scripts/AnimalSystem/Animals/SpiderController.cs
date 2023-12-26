using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : AnimalController
{
    [SerializeField]
    Transform targetAnimalLead;
    SpiderManager animalManager;
    BaseCurrent baseCurrent;
    BaseFunction baseFunction;

    // Animal Info
    AnimalStatus animalStatus;
    int indexAnimal;
    EnemySpider animalModel;
    int indexControll = 5;

    // Animation states
    const string ANIMAL = "Spider";
    string currentState = "";
    bool isIdle = true;

    // Animator
    Animator animator;

    NavMeshAgent agent;
    bool isRange = true;
    float range;

    // Idle Behaviour
    int countStep = 0;
    int countMove = 0;
    float idleDuration = 10f;
    float idleTimer;
    bool isMovement;

    // Attack Behaviour
    float attackDuration = 5f;
    float attackTimer;
    bool isReadyAttack;
    bool isHit;

    // Recovery Behaviour
    float recoveryDuration = 3f;
    float recoveryTimer;
    bool isRecovery;

    // Center of the area the agent wants to move around in
    Vector3 centerArea;
    float rangeMovement;
    Transform endPoint;
    // Instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    // Target Player
    float attackRange = 3f;
    float stoppingDistance = 2f; // Distance at which the enemy stops near the player
    
    bool isPlayerInRange = false; // Flag to track if the player is in attack range

    void Awake()
    {
        // animalManager = FindAnyObjectByType<SpiderManager>();
        animalManager = GetComponentInParent<SpiderManager>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        baseCurrent = new();
        baseFunction = new();
        animalModel = new();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        animalModel = animalManager.GetModel(indexAnimal);
        endPoint = transform;
        ChangeAnimationState(baseCurrent.GetIdle(ANIMAL));

        gameObject.name = animalModel.GetPrivateKey();
        gameObject.tag = animalModel.GetTag();

        animalStatus = AnimalStatus.Peace;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState != baseCurrent.GetDeath(ANIMAL))
        {
            if (currentState != baseCurrent.GetIdle(ANIMAL) && isIdle)
            {
                ChangeAnimationState(baseCurrent.GetIdle(ANIMAL));
            }

            if (animalStatus.Equals(AnimalStatus.Peace))
            {
                agent.stoppingDistance = 0;
                TargetPlayer();

                if (animalModel.CheckHealth() && isRecovery)
                {
                    recoveryTimer = 0f;
                    animalModel.SetHealing(10);
                    isRecovery = false;
                }
                else
                {
                    DirectionHealing();
                }

                if (animalModel.GetAnimalPositions().Equals(AnimalPositions.Lead))
                {
                    LeadMovement();
                }

                if (animalModel.GetAnimalPositions().Equals(AnimalPositions.None) && targetAnimalLead != null)
                {
                    FowardLead();
                }
            }
            else if (animalStatus.Equals(AnimalStatus.Attack))
            {
                // Adjust stopping distance to stop near the player but not too close
                agent.stoppingDistance = stoppingDistance;

                if (!isReadyAttack)
                {
                    DirectionChangeAttack();
                }

                if (stateInfo.IsName(baseCurrent.GetAttack(ANIMAL)) && stateInfo.normalizedTime >= 1.0f && !isHit)
                {
                    if (player != null)
                    {
                        PlayerController playerController = player.GetComponent<PlayerController>();
                        if (playerController != null)
                        {
                            playerController.player.playerStats.HealthSystem.DealDamage(animalModel.GetSTR());
                            isHit = true;
                            Vector3 randomDestination = SearchForDest(3f);
                            if (baseFunction.IsValidDestination(randomDestination))
                            {
                                MoveTowardsPlayer(randomDestination);
                            }
                        }
                    }
                }

                DetectPlayer(); // Check if the player is in range
                if (isPlayerInRange && isReadyAttack)
                {
                    AttackPlayer(); // Initiate attack if the player is in range
                }
                else if (!isPlayerInRange && isReadyAttack)
                {
                    if (animalModel.GetAnimalPositions().Equals(AnimalPositions.Lead))
                    {
                        if (Vector3.Distance(transform.position, centerArea) >= rangeMovement)
                        {
                            animalManager.ChangeStatusAnimalGroup(AnimalStatus.Peace);
                            AnimalMovement(endPoint.position);
                        }
                    }

                    float nearestDistance = 25f;
                    float dictanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    if (dictanceToPlayer > agent.stoppingDistance && dictanceToPlayer < nearestDistance && !agent.pathPending)
                    {
                        MoveTowardsPlayer(player.transform.position); // Move towards the player
                    }
                    else
                    {
                        agent.isStopped = true;
                        isIdle = true;
                    }
                }
            }
        }
        else
        {
            if (stateInfo.normalizedTime >= 2f)
            {
                animalModel = animalManager.GetModel(indexAnimal);
                animalManager.CheckCountAnimalActive();
                gameObject.SetActive(false);
            }

        }
    }

    //Status Attack
    void MoveTowardsPlayer(Vector3 nearestPlayerPosition)
    {
        // Move the NavMeshAgent towards the player
        agent.isStopped = false;
        agent.SetDestination(nearestPlayerPosition);
        ChangeAnimationState(baseCurrent.GetRunForward(ANIMAL));
        isIdle = false;
    }

    void DetectPlayer()
    {
        if (player != null)
        {
            // Check if the player GameObject associated with the Transform is active in the scene
            if (player.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance <= attackRange)
                {
                    isPlayerInRange = true;
                }
                else
                {
                    isPlayerInRange = false;
                }
            }
        }
    }
    void AttackPlayer()
    {
        // Your attack logic goes here...
        // For example, reducing player's health or triggering attack animations.
        agent.isStopped = true;
        attackTimer = 0f;
        ChangeAnimationState(baseCurrent.GetAttack(ANIMAL));
        isReadyAttack = false;
        isHit = false;
    }
    void DirectionChangeAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackDuration)
            isReadyAttack = true;
    }
    Vector3 RandomLeaveDestination(Vector3 point)
    {
        // Generate random coordinates within the spawn area
        float randomX = Random.Range(-point.x / 21f, point.x / 21f);
        float randomZ = Random.Range(-point.z / 21f, point.z / 21f);
        // Create a random position within the spawn area
        Vector3 randomPosition = new Vector3(randomX, point.y, randomZ) + point;
        return randomPosition;
    }

    //Status Peace
    void TargetPlayer()
    {
        if (player != null)
        {
            // Check if the player GameObject associated with the Transform is active in the scene
            if (player.gameObject.activeInHierarchy)
            {
                float detectionRadius = 15f;
                Vector3 toPlayer = player.position - transform.position;
                toPlayer.y = 0;
                if (toPlayer.magnitude <= detectionRadius)
                {
                    if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionRadius * 3f * Mathf.Deg2Rad))
                    {
                        animalManager.ChangeStatusAnimalGroup(AnimalStatus.Attack);
                    }
                }
            }
        }
    }
    // Lead Movement
    void LeadMovement()
    {
        // Done with path
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isMovement)
            {
                DirectionChangeIdle();
            }

            if (range != rangeMovement)
            {
                if (rangeMovement > Vector3.Distance(transform.position, centerArea))
                {
                    range = rangeMovement - Vector3.Distance(transform.position, centerArea);
                }
                else
                {
                    range = Vector3.Distance(transform.position, centerArea) - rangeMovement;
                }

                if (range >= 40f)
                {
                    idleDuration = 7f;
                }
                else
                {
                    idleDuration = 5f;
                }
            }
            Vector3 point;
            // Pass in our center point and radius of area
            if (baseFunction.RandomPoint(centerArea, range, out point))
            {
                if (countStep == 0)
                {
                    // Check if the destination position is valid
                    if (baseFunction.IsValidDestination(point))
                    {
                        idleTimer = 0f;
                        isMovement = true;
                        AnimalMovement(point);
                        countMove++;
                    }
                }
                else if (countStep == 1)
                {
                    isMovement = false;
                    agent.isStopped = true;
                    isIdle = true;
                    AddControlAnimal();
                }
                countStep++;
            }
        }
    }
    void DirectionChangeIdle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            countStep = 0;
    }

    // Create Enemy
    void AddControlAnimal()
    {
        if (countMove == 3)
        {
            if (isRange && indexControll < 5)
            {
                PositionActiveAnimal();
            }

            isRange = !isRange;
            countMove = 0;
        }
    }
    void PositionActiveAnimal()
    {
        Vector3 positionActiveAnimal = RandomPositionAnimal();
        if (baseFunction.IsValidDestination(positionActiveAnimal))
        {
            animalManager.ActiveAnimal(indexControll, positionActiveAnimal);
            indexControll++;
        }
        else
        {
            PositionActiveAnimal();
        }
    }
    Vector3 RandomPositionAnimal()
    {
        // Generate random coordinates within the spawn area
        Vector3 animalVector = transform.position;
        float randomX = Random.Range(-animalVector.x / 2.5f, animalVector.x / 2.5f);
        float randomZ = Random.Range(-animalVector.z / 2.5f, animalVector.z / 2.5f);
        // Create a random position within the spawn area
        Vector3 randomPosition = new Vector3(randomX, animalVector.y, randomZ) + animalVector;
        return randomPosition;
    }

    // Enemy Movement
    void FowardLead()
    {
        float walkPointSet = Vector3.Distance(transform.position, targetAnimalLead.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (walkPointSet > 10f)
            {
                Vector3 randomDestination = RandomNextDestination(targetAnimalLead.position);
                if (baseFunction.IsValidDestination(randomDestination))
                {
                    AnimalMovement(randomDestination);
                }
            }
            else if (walkPointSet <= 3f)
            {
                Vector3 randomDestination = SearchForDest(7f);
                if (baseFunction.IsValidDestination(randomDestination))
                {
                    AnimalMovement(randomDestination);
                }
            }
            else if (walkPointSet > 3f && walkPointSet < 7f)
            {
                agent.isStopped = true;
                isIdle = true;
            }
            else
            {
                Vector3 randomDestination = SearchForDest(3f);
                if (baseFunction.IsValidDestination(randomDestination))
                {
                    AnimalMovement(randomDestination);
                }
            }
        }
    }
    Vector3 RandomNextDestination(Vector3 point)
    {
        // Generate random coordinates within the spawn area
        float randomX = Random.Range(-point.x / 3f, point.x / 3f);
        float randomZ = Random.Range(-point.z / 3f, point.z / 3f);
        // Create a random position within the spawn area
        Vector3 randomPosition = new Vector3(randomX, point.y, randomZ) + point;
        return randomPosition;
    }
    Vector3 SearchForDest(float rangeEnemy)
    {
        float randomX = Random.Range(-rangeEnemy, rangeEnemy);
        float randomZ = Random.Range(-rangeEnemy, rangeEnemy);
        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    void DirectionHealing()
    {
        recoveryTimer += Time.deltaTime;

        if (recoveryTimer > recoveryDuration)
            isRecovery = true;
    }

    void AnimalMovement(Vector3 positionMovement)
    {
        // So you can see with gizmos
        agent.isStopped = false;
        agent.SetDestination(positionMovement);
        ChangeAnimationState(baseCurrent.GetWalkForward(ANIMAL));
        isIdle = false;
        // Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
    }
    //--------------------------------------------------------------------------------------------------------//
    // Function Animation
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    //--------------------------------------------------------------------------------------------------------//
    // Event Public
    public void SetIndexAnimal(int index)
    {
        indexAnimal = index;
    }
    public EnemySpider GetModelAnimal()
    {
        return animalModel;
    }
    public void SetModelAnimal(EnemySpider animalModel)
    {
        this.animalModel = animalModel;
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        if (player != null)
        {
            // Check if the player GameObject associated with the Transform is active in the scene
            if (player.gameObject.activeInHierarchy)
            {
                float detectionRadius = 15f;
                Vector3 toPlayer = player.position - transform.position;
                toPlayer.y = 0;
                if (toPlayer.magnitude <= detectionRadius)
                {
                    if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionRadius * 3f * Mathf.Deg2Rad))
                    {
                        if (animalModel.TakeDamge(damage))
                        {
                            ChangeAnimationState(baseCurrent.GetHitFront(ANIMAL));
                        }
                        else
                        {
                            ChangeAnimationState(baseCurrent.GetDeath(ANIMAL));
                        }
                    }
                    else
                    {
                        if (animalModel.TakeDamge(damage * 1.5f))
                        {
                            ChangeAnimationState(baseCurrent.GetHitBack(ANIMAL));
                        }
                        else
                        {
                            ChangeAnimationState(baseCurrent.GetDeath(ANIMAL));
                        }
                    }
                }
            }
        }
    }
    public void SetAnimalStatus(AnimalStatus animalStatus)
    {
        if (animalStatus.Equals(AnimalStatus.Attack))
        {
            isReadyAttack = true;
            agent.isStopped = true;
            isIdle = true;
            endPoint = transform;
        }
        this.animalStatus = animalStatus;
    }
    public void SetCenterArea(Vector3 centerArea, float rangeMovement)
    {
        this.centerArea = centerArea;
        this.rangeMovement = rangeMovement;
        range = rangeMovement;
    }
}
