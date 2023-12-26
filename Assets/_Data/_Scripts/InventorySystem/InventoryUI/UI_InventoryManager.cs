using DR.InputSystem;
using DR.MainMenuSystem;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.InventorySystem
{
    public class UI_InventoryManager : MonoBehaviour
    {
        [SerializeField] private UI_BackpackInventory uIBackpackInventory;

        private void Start()
        {
            InputHandler.EscapeEvent += OnEscapeEvent;
            //InputHandler.Instance.InteractInventoryEvent += OnInteractInventoryEvent;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnInteractInventoryEvent();
            }
        }


        private void OnInteractInventoryEvent()
        {
            LevelManager.Instance.SetHasUIEnable(!uIBackpackInventory.gameObject.activeSelf);
                
            uIBackpackInventory.gameObject.SetActive(!uIBackpackInventory.gameObject.activeSelf);

            if (uIBackpackInventory.gameObject.activeSelf)
            {
                PlayerController.Instance.cameraHolder.SetCameraSpeed(0f, 0f);
            }
            else
            {
                PlayerController.Instance.cameraHolder.SetDefaultSpeed();
            }
            
            Cursor.lockState = uIBackpackInventory.gameObject.activeSelf
                ? CursorLockMode.None
                : CursorLockMode.Locked;
            
            Cursor.visible = uIBackpackInventory.gameObject.activeSelf;
        }

        private void OnEscapeEvent()
        {
            if(!uIBackpackInventory.gameObject.activeSelf) return;
            
            PlayerController.Instance.cameraHolder.SetDefaultSpeed();
            LevelManager.Instance.SetHasUIEnable(false);
            uIBackpackInventory.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}