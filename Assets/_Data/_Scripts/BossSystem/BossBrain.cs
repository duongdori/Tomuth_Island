using DR.BossSystem.StateMachine;
using UnityEngine;

namespace DR.BossSystem
{
    public class BossBrain : MonoBehaviour
    {
        public BossController bossController;

        public BossBaseState moveState;
        public BossBaseState chasingState;
        public BossBaseState attackState;
        public BossBaseState attack02State;
    }
}
