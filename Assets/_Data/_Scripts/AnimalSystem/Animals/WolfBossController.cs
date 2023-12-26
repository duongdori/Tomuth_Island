using DR.PlayerSystem;
using UnityEngine;
using UnityEngine.AI;

public class WolfBossController : AnimalController
{
    [SerializeField]
    Transform[] patrolPoints;
    int currentPatrol;

    WolfManager animalManager;
    BaseCurrent baseCurrent;
    BaseFunction baseFunction;

    // Animal Info
    AnimalStatus animalStatus;
    EnemyWolf animalModel;

    // Animation states
    const string ANIMAL = "Wolf_Boss";
    string currentState = "";
    bool isIdle = true;

    // Animator
    Animator animator;

    NavMeshAgent agent;

    // Idle Behaviour
    int countStep = 0;
    float idleDuration = 10f;
    float idleTimer;
    bool isMovement;

    // Attack Behaviour
    float attackDuration = 3f;
    float attackTimer;
    bool isReadyAttack;
    bool isHit;

    // Recovery Behaviour
    float recoveryDuration = 7f;
    float recoveryTimer;
    bool isRecovery;

    // Instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    // Target Player
    float attackRange = 6f;
    float stoppingDistance = 5.5f; // Distance at which the enemy stops near the player
    
    bool isPlayerInRange = false; // Flag to track if the player is in attack range

    void Awake()
    {
        animalManager = FindAnyObjectByType<WolfManager>();
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

        animalModel = animalManager.GetBossModel();

        ChangeAnimationState(baseCurrent.GetIdle(ANIMAL));

        gameObject.name = animalModel.GetPrivateKey();
        gameObject.tag = animalModel.GetTag();

        currentPatrol = 0;
        isMovement = true;

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

                AnimalMovement();
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
                            isIdle = true;
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
                    // Vector3 nearestPlayerPosition = GetNearestPlayerPosition();
                    float nearestDistance = 50f;
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
            if (stateInfo.normalizedTime >= 2.3f)
            {
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
        // Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
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

    //Status Peace
    void TargetPlayer()
    {
        if (player != null)
        {
            // Check if the player GameObject associated with the Transform is active in the scene
            if (player.gameObject.activeInHierarchy)
            {
                float detectionRadius = 45;
                Vector3 toPlayer = player.position - transform.position;
                toPlayer.y = 0;
                if (toPlayer.magnitude <= detectionRadius)
                {
                    if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionRadius * 3f * Mathf.Deg2Rad))
                    {
                        animalStatus = AnimalStatus.Attack;
                        isIdle = true;
                        agent.isStopped = true;
                        MoveTowardsPlayer(player.transform.position);
                    }
                }
            }
        }
    }
    // Lead Movement
    void AnimalMovement()
    {
        // Done with path
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isMovement)
            {
                DirectionChangeIdle();
            }
            else
            {
                if (transform.position != patrolPoints[currentPatrol].position)
                {
                    if (countStep == 0)
                    {
                        idleTimer = 0f;
                        isMovement = true;
                        AnimalMovement(patrolPoints[currentPatrol].position);
                    }
                    else if (countStep == 1)
                    {
                        isMovement = false;
                        agent.isStopped = true;
                        isIdle = true;
                    }
                    countStep++;
                }
                else
                {
                    currentPatrol = (currentPatrol + 1) % patrolPoints.Length;
                }
            }
        }
    }
    void DirectionChangeIdle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            countStep = 0;
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
}
