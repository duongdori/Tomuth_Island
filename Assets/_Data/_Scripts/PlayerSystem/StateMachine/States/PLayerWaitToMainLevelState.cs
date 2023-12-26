using DR.MainMenuSystem;

namespace DR.PlayerSystem.StateMachine.States
{
    public class PLayerWaitToMainLevelState : PlayerBaseState
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.Instance.LoadLevelNoLoading("MainLevelScene");
        }
    }
}