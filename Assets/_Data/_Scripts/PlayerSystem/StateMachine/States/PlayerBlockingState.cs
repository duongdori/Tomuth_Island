using Animancer;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerBlockingState : PlayerBaseState
    {
        [SerializeField] private ClipTransition anim;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            anim.Events.OnEnd = player.stateMachine.ForceSetDefaultState;
            player.animancer.Play(anim);
        }
        
    }
}