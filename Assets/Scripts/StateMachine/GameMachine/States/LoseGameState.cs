using Game.StateMachine;
namespace Game.GameCore.States
{
    public class LoseGameState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.GameLoseUI();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}