using Game.StateMachine;
namespace Game.AppCore.States
{
    public class PlayingGameState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.HideOtherMenus(null);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}