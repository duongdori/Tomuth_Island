using UnityEngine;
using UnityEngine.UI;

namespace DR.Crafting
{
    public class UI_CraftingSlot : MyMonobehaviour
    {
        [SerializeField] protected Button button;
        [SerializeField] protected Image icon;
        [SerializeField] protected Image highlight;
        [SerializeField] protected bool isVisible;

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(OnClick);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadButton();
            LoadIcon();
            LoadHighlight();
        }

        protected virtual void OnClick()
        {
            
        }

        public void SetHighlight(bool value)
        {
            isVisible = value;
            highlight.enabled = value;
        }
        
        #region Load Components
        private void LoadButton()
        {
            if(button != null) return;
            button = GetComponent<Button>();
            Debug.LogWarning(transform.name + ": LoadButton", gameObject);
        }
        
        private void LoadIcon()
        {
            if(icon != null) return;
            icon = transform.Find("Icon").GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadIcon", gameObject);
        }
        
        private void LoadHighlight()
        {
            if(highlight != null) return;
            highlight = transform.Find("Highlight").GetComponent<Image>();
            Debug.LogWarning(transform.name + ": LoadHighlight", gameObject);
        }

        #endregion
    }
}