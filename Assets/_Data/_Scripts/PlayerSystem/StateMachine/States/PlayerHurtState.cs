using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerHurtState : PlayerBaseState
    {
        [SerializeField] private CustomClipTransition anim;

        protected override void OnEnable()
        {
            base.OnEnable();
            anim.Events.OnEnd = ReturnToLocomotion;
            
            player.animancer.Play(anim);
            SoundManager.Instance.PlaySfx(anim.soundFX);
        }

        protected override void Update()
        {
            base.Update();
            Move(Time.deltaTime);
        }
    }
}