using System;
using DR.InputSystem;
using DR.MainMenuSystem;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.Crafting
{
    public class UI_PlayerCrafting : MyMonobehaviour
    {
        [SerializeField] private UI_MainCrafting mainCraftingUI;
        [SerializeField] private bool isVisible;
        

        protected override void Awake()
        {
            base.Awake();
            mainCraftingUI.gameObject.SetActive(false);
            isVisible = false;
        }

        protected override void Start()
        {
            base.Start();
            
            //InputHandler.CraftingPopupEvent += OnCraftingPopupEvent;
            InputHandler.EscapeEvent += OnEscapeEvent;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnCraftingPopupEvent();
            }
        }

        private void OnEscapeEvent()
        {
            if(!isVisible) return;
            
            PlayerController.Instance.cameraHolder.SetDefaultSpeed();
            isVisible = false;
            LevelManager.Instance.SetHasUIEnable(false);
            mainCraftingUI.OnCraftingPopupEvent();
        }

        private void OnDestroy()
        {
            //InputHandler.CraftingPopupEvent -= OnCraftingPopupEvent;
            InputHandler.EscapeEvent -= OnEscapeEvent;
        }
        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadMainCraftingUI();
        }
        
        private void OnCraftingPopupEvent()
        {
            isVisible = !isVisible;
            LevelManager.Instance.SetHasUIEnable(isVisible);
            
            if (isVisible)
            {
                PlayerController.Instance.cameraHolder.SetCameraSpeed(0f, 0f);
                mainCraftingUI.gameObject.SetActive(isVisible);
            }
            else
            {
                PlayerController.Instance.cameraHolder.SetDefaultSpeed();
                mainCraftingUI.OnCraftingPopupEvent();
            }
        }

        private void LoadMainCraftingUI()
        {
            if(mainCraftingUI != null) return;
            mainCraftingUI = GetComponentInChildren<UI_MainCrafting>();
            Debug.LogWarning(transform.name + ": LoadMainCraftingUI", gameObject);
        }
    }
}