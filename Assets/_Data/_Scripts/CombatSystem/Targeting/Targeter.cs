using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.CombatSystem.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;

        private Camera _mainCamera;
    
        [SerializeField] private List<Target> targets = new List<Target>();
        public Target CurrentTarget { get; private set; }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _mainCamera = Camera.main;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if(Camera.main == null) return;
            _mainCamera = Camera.main;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Target target)) return;
            if(targets.Contains(target)) return;
        
            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Target target)) return;
            if(!targets.Contains(target)) return;
        
            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if(targets.Count == 0) return false;

            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;
        
            foreach (Target target in targets)
            {
                Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
            
                if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) continue;
                //if(!target.GetComponentInChildren<Renderer>().isVisible) continue;

                Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
                if (toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }

            if (closestTarget == null) return false;

            CurrentTarget = closestTarget;
            cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        
            return true;
        }
    
        public void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }

            target.OnDestroyed -= RemoveTarget;
        
            if (targets.Contains(target))
            {
                targets.Remove(target);
            }
        }

        public void Cancel()
        {
            if(CurrentTarget == null) return;
        
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
    }
}
