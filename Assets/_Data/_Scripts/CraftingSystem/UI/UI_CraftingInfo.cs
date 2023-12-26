using System.Collections.Generic;
using System.Linq;
using DR.InventorySystem;
using DR.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DR.Crafting
{
    public class UI_CraftingInfo : MyMonobehaviour
    {
        public HotbarInventory inventory;
        [SerializeField] private CraftingSystem craftingSystem;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image iconPreview;
        [SerializeField] private Button craftingButton;
        [SerializeField] private CraftingRecipeSO recipeSO;
        [SerializeField] private List<UI_CraftingSlotInput> slotInputList;
        
        private FunctionTimer functionTimer;
        protected override void Awake()
        {
            base.Awake();
            craftingButton.onClick.AddListener(Onclick);
            gameObject.SetActive(false);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadTitle();
            LoadDescription();
            LoadProgressBar();
            LoadIconPreview();
            LoadCraftingButton();
            LoadSlotInputList();
        }

        private void Update()
        {
            if(functionTimer == null || recipeSO == null) return;
            progressBar.fillAmount = functionTimer.GetTimer() / recipeSO.craftingTime;
        }

        private void Onclick()
        {
            if(recipeSO == null) return;
            craftingButton.interactable = false;
            functionTimer = FunctionTimer.Create((() =>
            {
                craftingSystem.Craft(recipeSO);
                UpdateCraftingInfo(recipeSO);
            }), recipeSO.craftingTime);

        }

        public void UpdateCraftingInfo(CraftingRecipeSO recipeSO)
        {
            functionTimer = null;
            progressBar.fillAmount = 0f;
            this.recipeSO = recipeSO;
            for (int i = 0; i < slotInputList.Count; i++)
            {
                if (i < recipeSO.inputItemList.Count)
                {
                    slotInputList[i].UpdateSlotInput(recipeSO.inputItemList[i]);
                }
                else
                {
                    slotInputList[i].UpdateSlotInput(null);
                }
            }

            craftingButton.interactable = recipeSO != null && craftingSystem.CanCraftItem(recipeSO);
            iconPreview.enabled = recipeSO != null;
            if(recipeSO == null) return;
            title.SetText(recipeSO.outputItem.itemData.itemName);
            description.SetText(recipeSO.outputItem.itemData.itemDescription);
            iconPreview.sprite = recipeSO.outputItem.itemData.itemIcon;
        }

        #region Load Components

        private void LoadTitle()
        {
            if(title != null) return;
            title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadTitle", gameObject);
        }
        
        private void LoadDescription()
        {
            if(description != null) return;
            description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning(transform.name + ": LoadDescription", gameObject);
        }
        private void LoadProgressBar()
        {
            if(progressBar != null) return;
            progressBar = transform.Find("ProgressBar").GetComponent<Image>();
            progressBar.fillAmount = 0f;
            Debug.LogWarning(transform.name + ": LoadProgressBar", gameObject);
        }
        private void LoadIconPreview()
        {
            if(iconPreview != null) return;
            iconPreview = transform.Find("SlotPreview").Find("Icon").GetComponent<Image>();
            iconPreview.enabled = false;
            Debug.LogWarning(transform.name + ": LoadIconPreview", gameObject);
        }
        
        private void LoadCraftingButton()
        {
            if(craftingButton != null) return;
            craftingButton = transform.Find("CraftingButton").GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadCraftingButton", gameObject);
        }
        
        private void LoadSlotInputList()
        {
            if(slotInputList.Count > 0) return;
            slotInputList = GetComponentsInChildren<UI_CraftingSlotInput>().ToList();
            Debug.LogWarning(transform.name + ": LoadSlotInputList", gameObject);
        }

        #endregion
        
    }
}