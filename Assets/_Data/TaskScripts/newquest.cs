using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newquest : MonoBehaviour
{
    [SerializeField] private GameObject descriptionPanel;
    public RectTransform panelRect;

    void Start()
    {
        
    }

    void Update()
    {
        CheckMouseClickOutsidePanel();
    }

    public void ShowDescription()
    {
        descriptionPanel.SetActive(true);
    }

    void CheckMouseClickOutsidePanel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, Input.mousePosition, Camera.main, out mousePos);

            if (!panelRect.rect.Contains(mousePos))
            {
                descriptionPanel.SetActive(false);

            }
        }
    }
}
