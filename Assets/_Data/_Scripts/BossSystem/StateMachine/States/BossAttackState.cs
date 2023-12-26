using System.Collections.Generic;
using Animancer;
using DR.SoundSystem;
using DR.Utilities;
using UnityEngine;

namespace DR.BossSystem.StateMachine.States
{
    public class BossAttackState : BossBaseState
    {
        [SerializeField] private List<SpecialClipTransition> animList;

        private bool _isEnd;
        private float _timer;
        private int _randomIndex;
        private void OnEnable()
        {
            _timer = 1f;
            _isEnd = false;

            _randomIndex = Random.Range(0, animList.Count);
            
            animList[_randomIndex].Events.OnEnd = OnEnd;
            animList[_randomIndex].Events.SetCallback("EnableDamage", boss.weaponHandler.EnableWeapon);
            animList[_randomIndex].Events.SetCallback("DisableDamage", boss.weaponHandler.DisableWeapon);
            
            animList[_randomIndex].Events.SetCallback("PlayVFX", (() =>
            {
                boss.animationEventEffects.InstantiateEffect(_randomIndex);
            }));
            
            animList[_randomIndex].Events.SetCallback("PlaySoundFX01", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX01);
                
            }));
            animList[_randomIndex].Events.SetCallback("PlaySoundFX02", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX02);
                
            }));
            animList[_randomIndex].Events.SetCallback("PlaySoundFX03", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX03);
                boss.weaponDamage.SetAttack(30);
            }));
            
            boss.weaponDamage.SetAttack(Random.Range(20, 25));
            boss.animancer.Play(animList[_randomIndex]);
            boss.animancer.Animator.applyRootMotion = true;
            FaceTarget();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            boss.animancer.Animator.applyRootMotion = false;
            
            boss.weaponHandler.DisableWeapon();
            animList[_randomIndex].Events.RemoveCallback("EnableDamage", boss.weaponHandler.EnableWeapon);
            animList[_randomIndex].Events.RemoveCallback("DisableDamage", boss.weaponHandler.DisableWeapon);
            
            animList[_randomIndex].Events.RemoveCallback("PlaySoundFX01", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX01);
                
            }));
            animList[_randomIndex].Events.RemoveCallback("PlaySoundFX02", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX02);
                
            }));
            animList[_randomIndex].Events.RemoveCallback("PlaySoundFX03", (() =>
            {
                SoundManager.Instance.PlaySfx(animList[_randomIndex].soundFX03);
                
            }));
            
            animList[_randomIndex].Events.RemoveCallback("PlayVFX", (() =>
            {
                boss.animationEventEffects.InstantiateEffect(_randomIndex);
            }));
        }

        private void OnEnd()
        {
            _isEnd = true;
            animList[_randomIndex].Events.OnEnd = null;
        }

        private void Update()
        {
            if(!_isEnd) return;

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _isEnd = false;
                boss.stateMachine.TrySetState(boss.brain.chasingState);
                return;
            }
        }

        private void FaceTarget()
        {
            if(boss.fieldOfView.player == null) return;
            
            Vector3 direction = boss.fieldOfView.player.transform.position - boss.transform.position;
            direction.y = 0f;
            direction.Normalize();

            boss.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}