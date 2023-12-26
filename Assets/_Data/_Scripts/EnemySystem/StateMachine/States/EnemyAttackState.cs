using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemyAttackState : EnemyBaseState
    {
        [SerializeField] private float delay = 2f;
        [SerializeField] private ClipTransition anim;
        private void OnEnable()
        {
            delay = 2f;
            anim.Events.SetCallback("EnableWeapon", enemy.weaponHandler.EnableWeapon);
            anim.Events.SetCallback("DisableWeapon", enemy.weaponHandler.DisableWeapon);
            enemy.weaponDamage.SetAttack(enemy.parameters.damageAmount);
            enemy.animancer.Play(anim);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            anim.Events.RemoveCallback("EnableWeapon", enemy.weaponHandler.EnableWeapon);
            anim.Events.RemoveCallback("DisableWeapon", enemy.weaponHandler.DisableWeapon);
        }

        private void Update()
        {
            if (delay <= 0)
            {
                delay = 2f;
                enemy.stateMachine.TrySetState(enemy.brain.chasingState);
                return;
            }

            delay -= Time.deltaTime;
            
            FacePlayer();
        }
        
        private void FacePlayer()
        {
            if(enemy.fieldOfView.PlayerRef == null) return;
        
            Vector3 lookPos = enemy.fieldOfView.PlayerRef.transform.position - enemy.transform.position;
            lookPos.y = 0f;
        
            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation,
                Quaternion.LookRotation(lookPos), 10f * Time.deltaTime);
        }
    }
}