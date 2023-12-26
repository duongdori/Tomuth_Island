using System.Threading.Tasks;
using Animancer;
using DR.CombatSystem.Targeting;
using DR.CombatSystem.Weapons;
using DR.InputSystem;
using DR.MainMenuSystem;
using DR.PlayerSystem.StateMachine;
using DR.PlayerSystem.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        public CharacterController controller;
        
        public AnimancerComponent animancer;
        
        public PlayerBrain brain;
        
        public PlayerBaseState.StateMachine stateMachine;

        public Transform mainCameraTransform;
        public InputHandler inputHandler;
        public ForceReceiver forceReceiver;
        public Targeter targeter;
        public WeaponDamage weaponDamage;
        public WeaponHandler weaponHandler;
        public Transform enemyPoint;
        public PlayerStats playerStats;
        public PlayerParameters parameters;
        
        private void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
            SceneManager.sceneLoaded += OnSceneLoaded;
            parameters.isDungeon = false;
        }
        
        private void Start()
        {

            playerStats.HealthSystem.OnTakeDamage += () => { stateMachine.ForceSetState(brain.hurtState); };
            
            playerStats.HealthSystem.OnDie += () =>
            {
                stateMachine.ForceSetState(brain.dieState);
            };
            
            playerStats.ThirstySystem.OnRunOutThirstyAmount += () =>
            {
                stateMachine.ForceSetState(brain.dieState);
            };
            playerStats.HungerSystem.OnRunOutHungerAmount += () =>
            {
                stateMachine.ForceSetState(brain.dieState);
            };
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            Debug.DrawRay(parameters.groundCheckPosition.position + Vector3.up * parameters.groundCheckRadius, Vector3.down * parameters.groundCheckMaxDistance, IsGrounded() ? Color.blue : Color.yellow);
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            mainCameraTransform = Camera.main.transform;

            // if (scene.name == "Dungeon01")
            // {
            //     Debug.Log(PlayerStartPoint.Instance.transform.position);
            //     parameters.isDungeon = true;
            //     transform.position = PlayerStartPoint.Instance.transform.position;
            //     return;
            // }
            //
            // if(scene.name != "MainLevelScene") return;
            //
            // if (parameters.isDungeon)
            // {
            //     parameters.isDungeon = false;
            //     transform.position = DungeonGate.Instance.transform.position + Vector3.forward * 2f;
            //     return;
            // }
            // if (LevelManager.Instance.isNewGame)
            // {
            //     transform.position = PlayerStartPoint.Instance.transform.position + Vector3.up * 10f;
            // }
            // else
            // {
            //     await LoadData();
            // }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(parameters.groundCheckPosition.position, Vector3.down,
                parameters.groundCheckMaxDistance, parameters.groundCheckLayerMask);

            // if (Physics.SphereCast(parameters.groundCheckPosition.position + Vector3.up * parameters.groundCheckRadius, parameters.groundCheckRadius, Vector3.down, out RaycastHit hitInfo, 
            //         parameters.groundCheckMaxDistance, parameters.groundCheckLayerMask))
            // {
            //     return true;
            // }
            // return false;
            //return Physics.CheckSphere(parameters.groundCheckPosition.position, parameters.groundCheckRadius, parameters.groundCheckLayerMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(parameters.groundCheckPosition.position, Vector3.down * parameters.groundCheckMaxDistance);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DungeonGate dungeonGate))
            {
                LevelManager.Instance.loadingScreen.SetActive(true);
                LevelManager.Instance.LoadLevelNoLoading(dungeonGate.sceneName);
            }
        }

        public void SetLockMovement()
        {
            parameters.SetTempMovement();
            
            parameters.movementSpeed = 0f;
            parameters.sprintSpeed = 0f;
            parameters.jumpForce = 0f;
            parameters.rollingForce = 0f;
            parameters.crouchMoveSpeed = 0f;
        }

        private void OnApplicationQuit()
        {
            //SaveData();
        }

        public async Task<PlayerPos> LoadData()
        {
            //bool hasValue = false;

            return await CloudSaveManager.RetrieveSpecificData<PlayerPos>("PlayerPosition");
            
            //transform.position = hasValue == true ? new Vector3(playerPos.xPos, playerPos.yPos + 5f, playerPos.zPos) : PlayerStartPoint.Instance.transform.position;
        }

        public async Task SaveData()
        {
            //if(SceneManager.GetActiveScene().name != "MainLevelScene") return;

            PlayerPos playerPos = new PlayerPos(transform.position.x, transform.position.y, transform.position.z);
            
            await CloudSaveManager.ForceSaveSingleData("PlayerPosition", playerPos);
        }
    }

    public struct PlayerPos
    {
        public float xPos;
        public float yPos;
        public float zPos;

        public PlayerPos(float x, float y, float z)
        {
            xPos = x;
            yPos = y;
            zPos = z;
        }
    }
    
}