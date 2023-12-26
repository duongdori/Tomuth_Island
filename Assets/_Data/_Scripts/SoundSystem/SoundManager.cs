using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.SoundSystem
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
    
        [SerializeField] private List<SoundAudioClip> musicAudioClips;
        [SerializeField] private List<SoundAudioClip> sfxAudioClips;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            PlayMusic(Sound.BackgroundMusic);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "IntroScene")
            {
                musicSource.Pause();
            }
            else
            {
                if(musicSource.isPlaying) return;
                musicSource.UnPause();
            }
        }

        private void Start()
        {
        }
    
        public void PlayMusic(Sound sound)
        {
            SoundAudioClip soundAudioClip = musicAudioClips.Find(s => s.sound == sound);
            if(soundAudioClip == null) Debug.LogWarning("Sound not found: " + sound);
            else
            {
                musicSource.clip = soundAudioClip.audioClip;
                musicSource.Play();
            }
        }
        public void PlaySfx(Sound sound)
        {
            SoundAudioClip soundAudioClip = sfxAudioClips.Find(s => s.sound == sound);
            if(soundAudioClip == null) Debug.LogWarning("Sound not found: " + sound);
            else
            {
                sfxSource.PlayOneShot(soundAudioClip.audioClip);
            }
        }
    
        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void ChangeMusicVolume(float value)
        {
            musicSource.volume = value;
        }
        public void ChangeSfxVolume(float value)
        {
            sfxSource.volume = value;
        }
    }

    public enum Sound
    {
        BackgroundMusic,
        FootStep01,
        FootStep02,
        PlayerDie,
        Jump,
        StartCrouch,
        EndCrouch,
        Landing,
        Rolling,
        DungeonMusic,
        BossMusic,
        Farm,
        ButtonSfx,
        LevelUp,
        Order,
        BillPaid,
    }

    [Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }
}