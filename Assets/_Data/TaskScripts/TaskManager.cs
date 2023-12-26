using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DR.CameraSystem;
using DR.InputSystem;
using DR.PlayerSystem;
using DR.MainMenuSystem;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Transform parentPos;
    [SerializeField] private GameObject questPanel, questBtn;
    public List<Transform> questPrefabs = new List<Transform>();
    public bool canTrigger;

    [SerializeField] string tag;

    void Start()
    {
        InputHandler.InteractEvent += OnInteractEvent;
    }

    void OnDestroy()
    {
        InputHandler.FKeyEvent -= OnInteractEvent;
    }
    private void OnInteractEvent()
    {
        if (!canTrigger) return;
        if (LevelManager.Instance.HasUIEnable) return;
        OpenPanel();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(tag))
        {
            canTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            canTrigger = false;
        }
    }

    public void OpenPanel()
    {
        LevelManager.Instance.SetHasUIEnable(true);

        questPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraHolder.Instance.SetCameraSpeed(0, 0);
        // questBtn.SetActive(false);
        //Vector3 newPosition = parentPos.position;
        //foreach (Transform prefab in questPrefabs)
        //{
        //    if (prefab != null)
        //    {
        //        newPosition.y -= 10f;
        //        Instantiate(prefab, newPosition, parentPos.rotation, parentPos);
        //    }
        //}
    }

    public void RemoveTask(string taskID)
    {
        //questPrefabs.RemoveAll(item => item != null && item.name == taskID);
        //CloseAll();
        OpenPanel();
    }

    public void CloseAll()
    {
        //foreach (Transform child in parentPos)
        //{
        //    Destroy(child.gameObject);
        //}
        LevelManager.Instance.SetHasUIEnable(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraHolder.Instance.SetDefaultSpeed();
        questPanel.SetActive(false);
        // questBtn.SetActive(true);
    }
}
