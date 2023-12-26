using System.Collections.Generic;
using Animancer;
using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerSpecialAttackState : PlayerBaseState
    {
        [SerializeField] private List<SpecialClipTransition> animList;
        public int randomIndex;
        protected override void OnEnable()
        {
            base.OnEnable();
            player.weaponDamage.enemy.FaceTarget();
            player.weaponDamage.enemy.transform.position = player.enemyPoint.transform.position;
            randomIndex = Random.Range(0, animList.Count);
            // player.weaponDamage.canSpecialAttack = false;
            // player.targeter.RemoveTarget(player.weaponDamage.enemy.target);
            animList[randomIndex].Events.OnEnd = player.stateMachine.ForceSetDefaultState;
            SpawnVFX(animList[randomIndex].vfx);
            player.animancer.Play(animList[randomIndex]);
            player.animancer.Animator.applyRootMotion = true;
            player.weaponDamage.enemy.brain.SetSpecialDieState(randomIndex);
            //player.parameters.enemy.stateMachine.ForceSetState(player.parameters.enemy.brain.specialDieState);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            player.playerStats.StaminaSystem.TrySpendMana(player.parameters.attackCost);
            PlayerController.Instance.playerUI.SetActiveRightClickIcon(false);
            
        }

        public override void OnExitState()
        {
            base.OnExitState();
            player.weaponDamage.enemy = null;
            player.parameters.enemy = null;
            player.animancer.Animator.applyRootMotion = false;
        }

        protected override void Update()
        {
            base.Update();
            Move(Time.deltaTime);
        }

        private void SetEvent()
        {
            animList[randomIndex].Events.SetCallback("PlaySoundFX01",
                () => { SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX01); });
            
            animList[randomIndex].Events.SetCallback("PlaySoundFX02",
                () => { SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX02); });
            
            animList[randomIndex].Events.SetCallback("PlaySoundFX03",
                () =>
                {
                    if(animList[randomIndex].soundFX03 == null) return;
                    SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX03);
                });
        }

        private void SpawnVFX(ParticleSystem vfx)
        {
            if(vfx == null) return;
            
            ParticleSystem fx = Instantiate(vfx, player.transform.position, player.transform.rotation);
            fx.Play();
        }
        public void PlaySoundFX01()
        {
            SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX01);
        }
        public void PlaySoundFX02()
        {
            SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX02);
        }
        public void PlaySoundFX03()
        {
            if(animList[randomIndex].soundFX03 == null) return;
            SoundManager.Instance.PlaySfx(animList[randomIndex].soundFX03);
        }
    }
}