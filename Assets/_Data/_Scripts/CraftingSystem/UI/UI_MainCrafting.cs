using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEngine;

namespace DR.Crafting
{
    public class UI_MainCrafting : MyMonobehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        // [SerializeField] private bool isVisible;
        public UI_SubCrafting subCraftingUI;
        [SerializeField] private ClipTransition showAnim;
        [SerializeField] private ClipTransition hideAnim;

        [SerializeField] private List<UI_CraftingSlot> craftingSlotUIList;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadAnim();
            LoadCraftingSlotUI();
        }

        private void OnEnable()
        {
            hideAnim.Events.OnEnd = () =>
            {
                gameObject.SetActive(false);
            };
            
            animancer.Play(showAnim);
            // isVisible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            DisableCraftingSlotHighlight(null);
        }

        public void OnCraftingPopupEvent()
        {
            subCraftingUI.SetVisible();
            // isVisible = false;
            animancer.Play(hideAnim);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void DisableCraftingSlotHighlight(UI_CraftingSlot selectSlot)
        {
            foreach (UI_CraftingSlot slot in craftingSlotUIList)
            {
                if(slot == selectSlot) continue;
                slot.SetHighlight(false);
            }
        }

        #region Load Components
        private void LoadAnim()
        {
            if(animancer != null) return;
            animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.LogWarning(transform.name + ": LoadAnim", gameObject);
        }
        private void LoadCraftingSlotUI()
        {
            if(craftingSlotUIList.Count > 0) return;
            craftingSlotUIList = GetComponentsInChildren<UI_CraftingSlot>().ToList();
            Debug.LogWarning(transform.name + ": LoadCraftingSlotUI", gameObject);
        }

        #endregion
        
        
    }
}
