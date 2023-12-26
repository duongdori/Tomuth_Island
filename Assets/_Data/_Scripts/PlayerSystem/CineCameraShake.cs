using Cinemachine;
using UnityEngine;

namespace DR.PlayerSystem
{
    public class CineCameraShake : MonoBehaviour
    {
        public static CineCameraShake Instance { get; private set; }
        
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        private float _shakeTimer;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            freeLookCamera = GetComponent<CinemachineFreeLook>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            _shakeTimer = time;
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                if (_shakeTimer <= 0)
                {
                    freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                    freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                    freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                }
            }
        }
    }
}