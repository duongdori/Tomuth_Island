using Animancer;
using Animancer.Units;
using UnityEngine;

namespace DR.PlayerSystem.StateMachine
{
    public class PlayerBrain : MonoBehaviour
    {
        public Player player;
        public AnimancerLayer basicLayer;
        public AnimancerLayer actionLayer;
        [SerializeField] private AvatarMask actionMask;
        [Seconds] public float actionFadeOutDuration = AnimancerPlayable.DefaultFadeDuration;
        
        public PlayerBaseState jumpState;
        public PlayerBaseState landingState;
        public PlayerBaseState crouchingState;
        public PlayerBaseState crouchingTargetState;
        public PlayerBaseState targetState;
        public PlayerBaseState rollingTargetState;
        public PlayerBaseState rollingState;
        public PlayerBaseState attackState;
        public PlayerBaseState backStabState;
        public PlayerBaseState specialAttackState;
        public PlayerBaseState blockingState;
        public PlayerBaseState dieState;
        public PlayerBaseState hurtState;
        public PlayerBaseState waitToMainLevelState;

        private void Awake()
        {
            basicLayer = player.animancer.Layers[0];
            actionLayer = player.animancer.Layers[1];
            actionLayer.SetMask(actionMask);
            actionLayer.SetDebugName("Action Layer");
            
        }
    }
}