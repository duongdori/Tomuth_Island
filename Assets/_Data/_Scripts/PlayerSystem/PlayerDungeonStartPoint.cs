using System;
using System.Collections;
using DR.MainMenuSystem;
using DR.SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem
{
    public class PlayerDungeonStartPoint : MonoBehaviour
    {

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            SoundManager.Instance.PlayMusic(Sound.DungeonMusic);
            StartCoroutine(SetPlayerPos());
        }
        

        private IEnumerator SetPlayerPos()
        {
            yield return new WaitForSeconds(2f);
            
            PlayerController.Instance.player.parameters.isDungeon = true;
            PlayerController.Instance.transform.position = transform.position;

            yield return new WaitForSeconds(2f);
            
            PlayerController.Instance.transform.position = transform.position;
            
            LevelManager.Instance.loadingScreen.SetActive(false);
            
        }
    }
}