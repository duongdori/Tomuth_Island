using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Q4 : MonoBehaviour
{
    public int Defeated; // gia tri nay bang so luong quai tieu diet duoc
    [SerializeField]
    private int target;
    [SerializeField]
    private TextMeshProUGUI cur;
    [SerializeField]
    private Button claimBtn, selfSwitch;
    [SerializeField]
    private GameObject itemGet;
    [SerializeField]
    private RectTransform panelRect;

    void Update()
    {
        CheckMouseClickOutsidePanel();
        cur.text = Defeated.ToString();

        if (Defeated == target) 
        {
            claimBtn.interactable = true;       
        }
    }

    public void Reward1()
    {
        // thuc hien lenh nhan thuong o day
        claimBtn.gameObject.SetActive(false);
        itemGet.SetActive(true);
        selfSwitch.interactable = false;
    }

    void CheckMouseClickOutsidePanel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, Input.mousePosition, Camera.main, out mousePos);

            if (!panelRect.rect.Contains(mousePos))
            {
                itemGet.SetActive(false);
            }
        }
    }
}
