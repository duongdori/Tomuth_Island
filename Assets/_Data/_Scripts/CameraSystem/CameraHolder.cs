using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.CameraSystem
{
    public class CameraHolder : MyMonobehaviour
    {
        public static CameraHolder Instance { get; private set; }
        
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;

        private float xSpeedDefault;
        private float ySpeedDefault;

        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            xSpeedDefault = freeLookCamera.m_XAxis.m_MaxSpeed;
            ySpeedDefault = freeLookCamera.m_YAxis.m_MaxSpeed;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {

        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadFreeLookCamera();
            LoadVirtualCamera();
            LoadTargetGroup();
        }

        public void SetupCamera(Transform cameraFocus)
        {
            freeLookCamera.Follow = cameraFocus;
            freeLookCamera.LookAt = cameraFocus;
            virtualCamera.Follow = cameraFocus;
        }

        public void SetCameraSpeed(float xSpeed, float ySpeed)
        {
            xSpeedDefault = freeLookCamera.m_XAxis.m_MaxSpeed;
            ySpeedDefault = freeLookCamera.m_YAxis.m_MaxSpeed;
        
            freeLookCamera.m_XAxis.m_MaxSpeed = xSpeed;
            freeLookCamera.m_YAxis.m_MaxSpeed = ySpeed;
        }

        public void SetDefaultSpeed()
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = xSpeedDefault;
            freeLookCamera.m_YAxis.m_MaxSpeed = ySpeedDefault;
        }

        public void SetCameraTarget(Transform target)
        {
            stateDrivenCamera.Follow = target;
            stateDrivenCamera.LookAt = target;
        
            freeLookCamera.Follow = target;
            freeLookCamera.LookAt = target;
        }
        private void LoadFreeLookCamera()
        {
            if(freeLookCamera != null) return;
            freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
            Debug.LogWarning(transform.name + ": LoadFreeLookCamera", gameObject);
        }
        private void LoadVirtualCamera()
        {
            if(virtualCamera != null) return;
            virtualCamera = transform.Find("State-Driven Camera").Find("TargetingCamera").GetComponent<CinemachineVirtualCamera>();
            Debug.LogWarning(transform.name + ": LoadVirtualCamera", gameObject);
        }
        private void LoadTargetGroup()
        {
            if(targetGroup != null) return;
            targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
            Debug.LogWarning(transform.name + ": LoadTargetGroup", gameObject);
        }
    }
}
