using DR.CameraSystem;
using DR.CharacterCustomSystem;
using DR.CombatSystem.Weapons;
using DR.InventorySystem;
using UnityEngine;

namespace DR.PlayerSystem
{
    public class PlayerController : MyMonobehaviour
    {
        public static PlayerController Instance { get; private set; }
    
        // public InputHandler inputHandler;
        public Player player;
        public HotbarInventory hotbarInventory;
        public BackpackInventory backpackInventory;
        public WeaponSlotManger weaponSlotManger;
        public GameObject damagePopupPrefab;
        public Transform cameraFocus;
        public CameraHolder cameraHolder;
        public CharacterCustomization characterCustomization;
        public GameObject diePanel;
        public GameObject playerCanvas;
        public PlayerUI playerUI;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            // LoadInputHandler();
            LoadPlayer();
            LoadHotbarInventory();
            LoadBackpackInventory();
            LoadWeaponSlotManager();
            LoadDamagePopup();
            LoadCameraFocus();
            LoadCharacterCustomization();
        }

        // private void LoadInputHandler()
        // {
        //     if(inputHandler != null) return;
        //     inputHandler = GetComponent<InputHandler>();
        //     Debug.LogWarning(transform.name + ": LoadInputHandler", gameObject);
        // }
        private void LoadPlayer()
        {
            if(player != null) return;
            player = GetComponent<Player>();
            Debug.LogWarning(transform.name + ": LoadPlayer", gameObject);
        }
    
        private void LoadHotbarInventory()
        {
            if(hotbarInventory != null) return;
            hotbarInventory = GetComponent<HotbarInventory>();
            Debug.LogWarning(transform.name + ": LoadHotbarInventory", gameObject);
        }
    
        private void LoadBackpackInventory()
        {
            if(backpackInventory != null) return;
            backpackInventory = GetComponentInChildren<BackpackInventory>();
            Debug.LogWarning(transform.name + ": LoadBackpackInventory", gameObject);
        }
    
        private void LoadWeaponSlotManager()
        {
            if(weaponSlotManger != null) return;
            weaponSlotManger = GetComponent<WeaponSlotManger>();
            Debug.LogWarning(transform.name + ": LoadWeaponSlotManager", gameObject);
        }
        private void LoadDamagePopup()
        {
            if(damagePopupPrefab != null) return;
            damagePopupPrefab = Resources.Load<GameObject>("Prefabs/UI/DamagePopupPrefab");
            Debug.LogWarning(transform.name + ": LoadDamagePopup", gameObject);
        }
        private void LoadCameraFocus()
        {
            if(cameraFocus != null) return;
            cameraFocus = transform.Find("CameraFocus");
            Debug.LogWarning(transform.name + ": LoadCameraFocus", gameObject);
        }
        private void LoadCharacterCustomization()
        {
            if(characterCustomization != null) return;
            characterCustomization = GetComponentInChildren<CharacterCustomization>();
            Debug.LogWarning(transform.name + ": LoadCharacterCustomization", gameObject);
        }
    }
}
