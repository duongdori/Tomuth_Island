using System.Collections;
using DR.PlayerSystem;
using UnityEngine;

namespace DR.EnemySystem.StateMachine
{
    public class EnemyFieldOfView : MonoBehaviour
    {
        [SerializeField] private float radius;
        
        [Range(0,360)]
        [SerializeField] private float angle;

        [SerializeField] private GameObject playerRef;
        
        [SerializeField] private LayerMask targetMask;

        [SerializeField] private bool canSeePlayer;

        public bool CanSeePlayer => canSeePlayer;
        public GameObject PlayerRef => playerRef;

        private void Start()
        {
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }
        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                
                //Debug.Log(Vector3.Dot(target.forward, directionToTarget));
                playerRef = PlayerController.Instance.gameObject;
                
                canSeePlayer = Vector3.Dot(transform.forward, directionToTarget) >= 0;
            }
            else if (canSeePlayer)
            {
                canSeePlayer = false;
                playerRef = null;
            }
            else
            {
                canSeePlayer = false;
                playerRef = null;
            }
        }

        // private void FieldOfViewCheck()
        // {
        //     Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        //     
        //     if (rangeChecks.Length != 0)
        //     {
        //         Transform target = rangeChecks[0].transform;
        //         Vector3 directionToTarget = (target.position - transform.position).normalized;
        //
        //         if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
        //         {
        //             float distanceToTarget = Vector3.Distance(transform.position, target.position);
        //
        //             if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
        //                 canSeePlayer = true;
        //             else
        //                 canSeePlayer = false;
        //         }
        //         else
        //             canSeePlayer = false;
        //     }
        //     else if (canSeePlayer)
        //         canSeePlayer = false;
        // }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}