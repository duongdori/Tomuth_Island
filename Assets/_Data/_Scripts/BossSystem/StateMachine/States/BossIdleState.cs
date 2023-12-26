using System;
using Animancer;
using UnityEngine;

namespace DR.BossSystem.StateMachine.States
{
    public class BossIdleState : BossBaseState
    {
        [SerializeField] private ClipTransition anim;

        private void OnEnable()
        {
            boss.rigid.velocity = Vector3.zero;
            boss.animancer.Play(anim);
        }

        private void Update()
        {
            if (boss.fieldOfView.player == null) return;

            boss.stateMachine.TrySetState(boss.brain.chasingState);
        }
    }
}