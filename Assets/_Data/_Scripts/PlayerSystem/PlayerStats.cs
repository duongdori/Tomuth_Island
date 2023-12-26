using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem.Stats
{
    public class PlayerStats : CharacterStats
    {
        [SerializeField] private StaminaSystem staminaSystem;
        public StaminaSystem StaminaSystem => staminaSystem;
        
        [SerializeField] private ThirstySystem thirstySystem;
        public ThirstySystem ThirstySystem => thirstySystem;
        
        [SerializeField] private HungerSystem hungerSystem;
        public HungerSystem HungerSystem => hungerSystem;

        public bool isInGame;
        private void Awake()
        {
            staminaSystem = new StaminaSystem();
            healthSystem = new HealthSystem(200);
            thirstySystem = new ThirstySystem(100f, 0.1f, this);
            hungerSystem = new HungerSystem(100f, 0.1f, this);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "MainLevelScene")
            {
                isInGame = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                healthSystem.DealDamage(10);
            }
        }

        private async void OnApplicationQuit()
        {
            isInGame = false;
            //await SaveData();
        }

        public async Task SaveData()
        {
            isInGame = false;
            await CloudSaveManager.ForceSaveSingleData("PlayerHealth", healthSystem.CurrentHealth);
            await CloudSaveManager.ForceSaveSingleData("PlayerThirsty", thirstySystem.CurrentThirsty);
            await CloudSaveManager.ForceSaveSingleData("PlayerHunger", hungerSystem.CurrentHunger);
        }

        public async Task LoadData()
        {
            int health = await CloudSaveManager.RetrieveSpecificData<int>("PlayerHealth");
            if (health != default)
            {
                healthSystem.SetCurrentHealth(health);
            }
            
            float thirsty = await CloudSaveManager.RetrieveSpecificData<float>("PlayerThirsty");
            if (thirsty != 0f)
            {
                thirstySystem.SetCurrentThirsty(thirsty);
            }
            
            float hunger = await CloudSaveManager.RetrieveSpecificData<float>("PlayerHunger");
            if (hunger != 0f)
            {
                hungerSystem.SetCurrentHunger(hunger);
            }
        }
    }
}