using DR.InputSystem;
using DR.MainMenuSystem;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.InventorySystem
{
    public class ChestInventory : InventorySystem
    {
        [SerializeField] private UI_ChestInventory chestInventoryUI;
        [SerializeField] private Interactor interactor;

        protected override void Start()
        {
            base.Start();
            InputHandler.InteractEvent += OnInteractEvent;
            InputHandler.EscapeEvent += OnEscapeEvent;
            interactor.OnExitInteract += () =>
            {
                LevelManager.Instance.SetHasUIEnable(false);
                
                PlayerController.Instance.cameraHolder.SetDefaultSpeed();
                chestInventoryUI.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            };
        }

        private void OnInteractEvent()
        {
            if(!interactor.IsInteract) return;
            LevelManager.Instance.SetHasUIEnable(true);
            
            chestInventoryUI.gameObject.SetActive(true);
            interactor.SetActiveInteractKey(false);
            PlayerController.Instance.cameraHolder.SetCameraSpeed(0f, 0f);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnEscapeEvent()
        {
            if(!interactor.IsInteract) return;
            LevelManager.Instance.SetHasUIEnable(false);
            
            chestInventoryUI.gameObject.SetActive(false);
            interactor.SetActiveInteractKey(true);
            PlayerController.Instance.cameraHolder.SetDefaultSpeed();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}