using Animancer;
using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerBackStabState : PlayerBaseState
    {
        [SerializeField] private CustomClipTransition anim;
        protected override void OnEnable()
        {
            base.OnEnable();
            
            anim.Events.OnEnd = () => { player.stateMachine.TrySetDefaultState(); };
            player.animancer.Play(anim);
            
            anim.Events.SetCallback("PlaySoundFX", () => {SoundManager.Instance.PlaySfx(anim.soundFX);});
            
            player.parameters.enemy.stateMachine.ForceSetState(player.parameters.enemy.brain.stabbedState);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.playerStats.StaminaSystem.TrySpendMana(player.parameters.attackCost);

        }

        public override void OnExitState()
        {
            base.OnExitState();
            player.parameters.enemy = null;
        }
    }
}