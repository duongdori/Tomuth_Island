using System;
using Animancer;
using UnityEngine;

namespace DR.BossSystem.StateMachine.States
{
    public class BossChasingState : BossBaseState
    {
        [SerializeField] protected float parameterFadeSpeed = 3;
        [SerializeField] private LinearMixerTransitionAsset.UnShared anim;

        private void OnEnable()
        {
            boss.animancer.Play(anim);
            anim.State.Time = 0f;
        }

        private void Update()
        {
            if (boss.fieldOfView.player == null)
            {
                boss.stateMachine.TrySetDefaultState();
                return;
            }
            if (CheckDistance(boss.parameters.distanceAttack01))
            {
                boss.stateMachine.TrySetState(boss.brain.attackState);
                return;
            }
            
            anim.State.Parameter = Mathf.MoveTowards(anim.State.Parameter, boss.rigid.velocity.normalized.magnitude,
                parameterFadeSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            HandleMovement(boss.fieldOfView.player.transform, boss.parameters.moveSpeed);
        }

        private bool CheckDistance(float maxDistance)
        {
            float distance = Vector3.Distance(boss.transform.position, boss.fieldOfView.player.transform.position);

            return distance <= maxDistance;
        }
    }
}