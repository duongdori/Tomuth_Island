using System;
using Animancer;
using UnityEngine;

namespace DR.BossSystem.StateMachine.States
{
    public class BossMoveState : BossBaseState
    {
        [SerializeField] private ClipTransition anim;

        private void OnEnable()
        {
            boss.animancer.Play(anim);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            boss.rigid.velocity = Vector3.zero;
        }

        private void Update()
        {
            if (CheckDistance(boss.parameters.startPoint, 2f))
            {
                Debug.Log("gethuergdfgdfg");
                boss.stateMachine.ForceSetState(boss.stateMachine.DefaultState);
                return;
            }
        }

        private void FixedUpdate()
        {
            HandleMovement(boss.parameters.startPoint, boss.parameters.walkSpeed);
        }
    }
}