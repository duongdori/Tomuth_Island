using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace DR.EnemySystem.StateMachine.State
{
    public class EnemySpecialDieState : EnemyBaseState
    {
        [SerializeField] private List<ClipTransition> animList;
        public int _randomIndex;
        
        public override void OnEnterState()
        {
            base.OnEnterState();
            enemy.enemyStats.enabled = false;
            enemy.controller.enabled = false;
            enemy.agent.enabled = false;
            enemy.redirectRootMotion.enabled = false;
        }
        
        private void OnEnable()
        {
            enemy.animancer.Play(animList[_randomIndex]);
            enemy.animancer.Animator.applyRootMotion = true;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            enemy.animancer.Animator.applyRootMotion = false;
        }

        public void SetIndex(int index)
        {
            _randomIndex = index;
        }
    }
}