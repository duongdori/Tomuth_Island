using Animancer;
using DR.MainMenuSystem;
using DR.SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PlayerDieState : PlayerBaseState
    {
        [SerializeField] private ClipTransition anim;
        private float _timer;
        private bool _canWaiting;
        protected override void OnEnable()
        {
            base.OnEnable();
            player.parameters.isDie = true;
            
            _canWaiting = false;
            _timer = 2f;
            anim.Events.OnEnd = OnEnd;
            player.animancer.Play(anim);
            player.animancer.Animator.applyRootMotion = true;
            player.playerStats.enabled = false;
            SoundManager.Instance.PlaySfx(Sound.PlayerDie);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            anim.Events.OnEnd = null;
        }

        protected override void Update()
        {
            base.Update();
            if (_canWaiting)
            {
                _timer -= Time.deltaTime;
                if(_timer > 0) return;

                if (SceneManager.GetActiveScene().name == "MainLevelScene")
                {
                    PlayerController.Instance.diePanel.SetActive(false);
                    _canWaiting = false;
                    player.playerStats.enabled = true;
                    player.playerStats.HealthSystem.ResetHealth();
                    player.playerStats.ThirstySystem.ResetThirsty();
                    player.playerStats.HungerSystem.ResetHunger();
                    player.animancer.Animator.applyRootMotion = false;
                    player.transform.position = PlayerStartPoint.Instance.transform.position;
                    player.stateMachine.ForceSetState(player.stateMachine.DefaultState);
                }
            }
        }

        private void OnEnd()
        {
            PlayerController.Instance.diePanel.SetActive(true);
            _canWaiting = true;

            if (SceneManager.GetActiveScene().name == "Dungeon01")
            {
                player.animancer.Animator.applyRootMotion = false;
                PlayerController.Instance.diePanel.SetActive(false);
                _canWaiting = false;
                player.stateMachine.ForceSetState(player.brain.waitToMainLevelState);
            }
        }
    }
}