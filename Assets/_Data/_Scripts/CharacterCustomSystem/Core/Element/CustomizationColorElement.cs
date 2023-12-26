using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DR.CharacterCustomSystem
{
    public class CustomizationColorElement : MonoBehaviour
    {
        public event UnityAction OnColorElementChanged;

        [SerializeField] private CharacterCustomization characterCustomization;
        [SerializeField] private ElementGroup elementGroup;
        [SerializeField] private int index = 0;
        [SerializeField] private Material material;
        [SerializeField] private string colorName;
        [SerializeField] private List<Color> colors = new List<Color>();
        
        // private void Start()
        // {
        //     ChangeColor();
        // }

        public void PreviousColor()
        {
            characterCustomization.SetElementGroup(elementGroup);
            index -= 1;
            if (index < 0)
            {
                index = colors.Count - 1;
            }
            
            ChangeColor();
        }
        
        public void NextColor()
        {
            characterCustomization.SetElementGroup(elementGroup);
            index += 1;
            if (index > colors.Count - 1)
            {
                index = 0;
            }
            ChangeColor();
        }

        private void ChangeColor()
        {
            if (colors.Count > 0)
            {
                material.SetColor(colorName, colors[index]);
            }
            else
            {
                Debug.LogWarning("No " + colorName + " Specified In The Inspector");
            }
        }

        public void SetColorElement()
        {
            index = 0;
            ChangeColor();
            OnColorElementChanged?.Invoke();
        }

        public Color GetCurrentColor()
        {
            return colors[index];
        }
    }
}