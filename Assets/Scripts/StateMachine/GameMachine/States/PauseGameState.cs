using Game.StateMachine;
namespace Game.GameCore.States
{
    public class PauseGameState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.PauseUI();
            CommandKeeper.Instance.WriteState("PauseState");
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}