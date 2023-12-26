using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DR.InputSystem
{
    public class InputHandler : MonoBehaviour, PlayerControls.IPlayerActions, PlayerControls.IPlayerUIActions
    {
        public static InputHandler Instance { get; private set; }
    
        public event UnityAction JumpEvent;
        public static event UnityAction TargetEvent;
        public event UnityAction CrouchEvent;
        public static event UnityAction RollEvent;
        public static event UnityAction BlockEvent;
        public static event UnityAction TestEvent;
        public static event UnityAction InteractEvent;
        public static event UnityAction<int> OnSlotActive;
        public static event UnityAction SprintEvent;
        public static event UnityAction EscapeEvent;
        public static event UnityAction FKeyEvent;
        public event UnityAction InteractInventoryEvent;
    
        public static event UnityAction CraftingPopupEvent;

        public event UnityAction ExitEvent;
    
        private PlayerControls _playerControls;
    
        public bool IsAttacking { get; private set; }
        public static bool IsBlocking { get; private set; }
        public static bool IsSprint { get; private set; }
        public static Vector2 MovementValue { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            _playerControls.HotBar.Slot1.performed += OnSlotPerformed;
            _playerControls.HotBar.Slot2.performed += OnSlotPerformed;
            _playerControls.HotBar.Slot3.performed += OnSlotPerformed;
            _playerControls.HotBar.Slot4.performed += OnSlotPerformed;
            _playerControls.HotBar.Slot5.performed += OnSlotPerformed;
        }
        
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // if(_playerControls != null) return;
            //
            // _playerControls = new PlayerControls();
            // _playerControls.Player.SetCallbacks(this);
            // _playerControls.Player.Enable();
            // _playerControls.HotBar.Enable();
            // _playerControls.PlayerUI.SetCallbacks(this);
            // _playerControls.PlayerUI.Enable();
            //
            // _playerControls.HotBar.Slot1.performed += OnSlotPerformed;
            // _playerControls.HotBar.Slot2.performed += OnSlotPerformed;
            // _playerControls.HotBar.Slot3.performed += OnSlotPerformed;
            // _playerControls.HotBar.Slot4.performed += OnSlotPerformed;
            // _playerControls.HotBar.Slot5.performed += OnSlotPerformed;
        }

        private void OnEnable()
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.SetCallbacks(this);
            _playerControls.Player.Enable();
            _playerControls.HotBar.Enable();
            _playerControls.PlayerUI.SetCallbacks(this);
            _playerControls.PlayerUI.Enable();
        }

        private void OnDestroy()
        {
            if(_playerControls == null) return;
        
            _playerControls.HotBar.Slot1.performed -= OnSlotPerformed;
            _playerControls.HotBar.Slot2.performed -= OnSlotPerformed;
            _playerControls.HotBar.Slot3.performed -= OnSlotPerformed;
            _playerControls.HotBar.Slot4.performed -= OnSlotPerformed;
            _playerControls.HotBar.Slot5.performed -= OnSlotPerformed;
        
            _playerControls.Player.Disable();
            _playerControls.HotBar.Disable();
            _playerControls.PlayerUI.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            JumpEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsSprint = true;
            }
            else if (context.canceled)
            {
                IsSprint = false;
                SprintEvent?.Invoke();
            }
        }

        public static void SetIsSprint(bool value)
        {
            IsSprint = value;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }
    
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }
    
        public void OnBlock(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            BlockEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            CrouchEvent?.Invoke();
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            RollEvent?.Invoke();
        }

        public void OnTest(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            TestEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            TargetEvent?.Invoke();
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            InteractEvent?.Invoke();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            EscapeEvent?.Invoke();
            ExitEvent?.Invoke();
        }

        private void OnSlotPerformed(InputAction.CallbackContext context)
        {
            string controlName = context.action.name;
            int slotIndex = int.Parse(controlName.Replace("Slot", "")) - 1;
            OnSlotActive?.Invoke(slotIndex);
        }

        public void OnCraftingPopup(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            CraftingPopupEvent?.Invoke();
        }

        public void OnInteractInventory(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            InteractInventoryEvent?.Invoke();
        }

        public void OnFKeyInteract(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        
            FKeyEvent?.Invoke();
        }
    }
}
