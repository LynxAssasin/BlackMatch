using Game.StateMachine;
namespace Game.GameCore.States
{
    public class WinGameState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.GameWinUI();
            CommandKeeper.Instance.SaveLevelProgress();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}