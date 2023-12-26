using UnityEngine;
using UnityEngine.Video;

namespace DR.MainMenuSystem
{
    public class IntroController : MyMonobehaviour
    {
        public static IntroController Instance { get; private set; }
        
        public VideoPlayer videoPlayer;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == null) Instance = this;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void Start()
        {
            base.Start();
            
            videoPlayer.loopPointReached += OnEnd;
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadVideoPlayer();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                videoPlayer.Stop();
                LevelManager.Instance.LoadLevel("CharacterCustomScene");
            }
        }

        private void OnEnd(VideoPlayer source)
        {
            if (source == videoPlayer)
            {
                LevelManager.Instance.LoadLevel("CharacterCustomScene");
            }
        }

        private void LoadVideoPlayer()
        {
            if(videoPlayer != null) return;
            videoPlayer = GetComponent<VideoPlayer>();
            Debug.Log(transform.name + ": LoadVideoPlayer", gameObject);
        }
    }
}
