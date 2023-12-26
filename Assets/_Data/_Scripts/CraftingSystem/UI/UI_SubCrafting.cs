using System.Collections.Generic;
using System.Linq;
using Animancer;
using TMPro;
using UnityEngine;

namespace DR.Crafting
{
    public class UI_SubCrafting : MyMonobehaviour
    {
        [SerializeField] private CraftingRecipeGroup recipeGroup;
        [SerializeField] private AnimancerComponent animancer;
        
        public UI_CraftingInfo craftingInfoUI;
        
        [SerializeField] private bool isVisible;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private ClipTransition showAnim;
        [SerializeField] private ClipTransition hideAnim;
        [SerializeField] private List<UI_SubCraftingSlot> craftingSlotUIList;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadAnim();
            LoadTitle();
            LoadCraftingSlotUI();
        }

        private void OnEnable()
        {
            hideAnim.Events.OnEnd = () => { gameObject.SetActive(false); };
        }

        private void OnDisable()
        {
            DisableCraftingSlotHighlight(null);
        }

        public void DisableCraftingSlotHighlight(UI_CraftingSlot selectSlot)
        {
            foreach (UI_CraftingSlot slot in craftingSlotUIList)
            {
                if(slot == selectSlot) continue;
                slot.SetHighlight(false);
            }
        }

        public void SetVisible()
        {
            animancer.Play(hideAnim);
            craftingInfoUI.gameObject.SetActive(false);
        }

        public void SetRecipeGroup(CraftingRecipeGroup craftingRecipeGroup)
        {
            var state = animancer.Play(showAnim);
            state.Time = 0;
            recipeGroup = craftingRecipeGroup;
            UpdateSubCrafting(recipeGroup);
        }

        private void UpdateSubCrafting(CraftingRecipeGroup craftingRecipeGroup)
        {
            title.SetText(craftingRecipeGroup.groupTitle);
            UpdateSlot();
        }

        private void UpdateSlot()
        {
            if (recipeGroup.craftingRecipeList.Count == 0)
            {
                foreach (UI_SubCraftingSlot slot in craftingSlotUIList)
                {
                    slot.UpdateCraftingSlot(null);
                }
                return;
            }
            
            foreach (UI_SubCraftingSlot slot in craftingSlotUIList)
            {
                slot.UpdateCraftingSlot(null);
            }
            
            for (int i = 0; i < recipeGroup.craftingRecipeList.Count; i++)
            {
                craftingSlotUIList[i].UpdateCraftingSlot(recipeGroup.craftingRecipeList[i]);
            }
        }

        #region Load Components

        private void LoadAnim()
        {
            if(animancer != null) return;
            animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.LogWarning(transform.name + ": LoadAnim", gameObject);
        }
        private void LoadTitle()
        {
            if(title != null) return;
            title = animancer.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadTitle", gameObject);
        }
        
        private void LoadCraftingSlotUI()
        {
            if(craftingSlotUIList.Count > 0) return;
            craftingSlotUIList = new List<UI_SubCraftingSlot>();
            craftingSlotUIList = GetComponentsInChildren<UI_SubCraftingSlot>().ToList();
            Debug.LogWarning(transform.name + ": LoadCraftingSlotUI", gameObject);
        }

        #endregion
        
    }
}