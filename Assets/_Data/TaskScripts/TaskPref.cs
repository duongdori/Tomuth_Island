using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskPref : MonoBehaviour
{
    public string TaskID;
    [HideInInspector]
    public bool complete = false;
    public TextMeshProUGUI questNam, curValue, tarValue, questNam2, des, reward1, reward2;
    public GameObject DesPanel;
    public string questNamTx;
    public int curValueNum, tarValueTxNum;
    public string questdescriptionTx;
    public int rw1, rw2;
    public Image desImg, RewardImage1, RewardImage2;
    public Sprite img, img1, img2;
    public Button claimBtn;
    public bool twoRewards = false;
    public RectTransform panelRect;

    private void Start()
    {
        questNam.text = questNamTx;
        questNam2.text = questNamTx;
        curValue.text = curValueNum.ToString();
        tarValue.text = tarValueTxNum.ToString(); // số lượng cần đạt được, số gỗ, đá thu được hoặc vật dụng chế được; quái diệt được,...
        des.text = questdescriptionTx;
        reward1.text = rw1.ToString();
        reward2.text = null;
        RewardImage2.gameObject.SetActive(false);
        if (twoRewards)
        {
            reward2.text = rw2.ToString();
            RewardImage2.gameObject.SetActive(true);
            RewardImage2.sprite = img2;
        }
    }

    public void ShowDescription()
    {
        Vector3 newPosition = new Vector3(0f, 0f, DesPanel.transform.position.z);
        DesPanel.transform.position = newPosition;
        DesPanel.SetActive(true);
    }

    public void CloseDescription()
    {
        DesPanel.SetActive(false);
    }

    private void Update()
    {
        CheckMouseClickOutsidePanel();
        if (curValueNum == tarValueTxNum) // 2 giá trị này bằng nhau thì hoàn thành nhiệm vụ
        {
            claimBtn.interactable = true;
        }
    }

    void CheckMouseClickOutsidePanel()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, Input.mousePosition, Camera.main, out mousePos);

            if (!panelRect.rect.Contains(mousePos))
            {
                DesPanel.SetActive(false);

            }
        }
    }


    public void ClaimReward()
    {
        complete = true;
        Destroy(gameObject);
        Debug.Log("hopan thanh");
        TaskManager taskManager = FindObjectOfType<TaskManager>();
        if (taskManager != null)
        {
            List<Transform> questPrefabs = taskManager.questPrefabs;

            if (questPrefabs != null)
            {
                Debug.Log("QuestManager have " + questPrefabs.Count + " in list");
                foreach (Transform prefab in questPrefabs)
                {
                    Debug.Log("name: " + prefab.name);
                }
                taskManager.RemoveTask(TaskID);
            }
            else
            {
                Debug.LogWarning("no list");
            }
        }
        else
        {
            Debug.LogWarning("no TaskManager in Scene.");
        }
    }



}
