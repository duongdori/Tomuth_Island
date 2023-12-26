using System;
using System.Collections;
using System.Threading.Tasks;
using DR.MainMenuSystem;
using DR.SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem
{
    public class PlayerStartPoint : MonoBehaviour
    {
        public static PlayerStartPoint Instance;

        public Player player;
        private Vector3 _playerPos;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (player == null)
                {
                    player = PlayerController.Instance.player;
                }
                player.transform.position = new Vector3(-53f, 25f, -30f);

                Debug.Log("Set PlayerPos To Dungeon Gate");
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (player == null)
                {
                    player = PlayerController.Instance.player;
                }
                player.transform.position = transform.position;
                Debug.Log("Set PlayerPos To startPoint");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (player == null)
                {
                    player = PlayerController.Instance.player;
                }
                player.transform.position = _playerPos;
                Debug.Log("Set PlayerPos To Continue Point");
            }
        }

        private void OnEnable()
        {
            player = PlayerController.Instance.player;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {            
            player = PlayerController.Instance.player;

            if (PlayerController.Instance.player.parameters.isDungeon)
            {
                if (PlayerController.Instance.player.parameters.isDie)
                {
                    PlayerController.Instance.player.playerStats.enabled = true;
                    PlayerController.Instance.player.playerStats.HealthSystem.ResetHealth();
                    PlayerController.Instance.player.playerStats.ThirstySystem.ResetThirsty();
                    PlayerController.Instance.player.playerStats.HungerSystem.ResetHunger();

                    StartCoroutine(LoadStartPoint());
                }
                else
                {
                    Debug.Log("OnSceneLoaded");
                    PlayerController.Instance.transform.position = new Vector3(-53f, 25f, -30f);
                    StartCoroutine(LoadPlayerInDungeon());
                    PlayerController.Instance.transform.position = new Vector3(-53f, 25f, -30f);
                }
                    
                SoundManager.Instance.PlayMusic(Sound.BackgroundMusic);
                return;
            }

            if (LevelManager.Instance.isNewGame)
            {
                PlayerController.Instance.transform.position = transform.position;
            }
            else
            {
                StartCoroutine(LoadPlayerDataAsync());
                    
                if (ES3.KeyExists("PlayerPosition"))
                {
                    _playerPos = ES3.Load<Vector3>("PlayerPosition");
                    PlayerController.Instance.transform.position = _playerPos;
                }
                else
                {
                    PlayerController.Instance.transform.position = transform.position;
                }
            }
        }
        
        private IEnumerator LoadPlayerDataAsync()
        {
            yield return LoadPlayerData();
        }

        private IEnumerator LoadPlayerData()
        {
            Task<PlayerPos> task = PlayerController.Instance.player.LoadData();
            
            yield return new WaitUntil(() => task.IsCompleted);
            
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                PlayerPos playerPos = task.Result;
                PlayerController.Instance.transform.position = new Vector3(playerPos.xPos, playerPos.yPos, playerPos.zPos);
            }
            else
            {
                PlayerController.Instance.transform.position = transform.position;
                Debug.LogWarning("Failed to load player data!");
            }
        }

        private IEnumerator LoadPlayerInDungeon()
        {
            yield return new WaitForSeconds(5f);
            PlayerController.Instance.player.parameters.isDungeon = false;
            PlayerController.Instance.transform.position = new Vector3(-53f, 25f, -30f);

            LevelManager.Instance.loadingScreen.SetActive(false);
        }

        private IEnumerator LoadStartPoint()
        {
            yield return new WaitForSeconds(5f);
            PlayerController.Instance.player.transform.position = transform.position;
            LevelManager.Instance.loadingScreen.SetActive(false);
            PlayerController.Instance.player.stateMachine.ForceSetState(PlayerController.Instance.player.stateMachine.DefaultState);
            PlayerController.Instance.player.parameters.isDie = false;
        }

        private IEnumerator LoadNewGame()
        {
            yield return new WaitForSeconds(3f);
        }
    }
}
