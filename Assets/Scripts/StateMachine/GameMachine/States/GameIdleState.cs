using Game.StateMachine;
namespace Game.GameCore.States
{
    public class GameIdleState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.EnableToTouchBoard(true);
            CommandKeeper.Instance.BoardFingerDown += FingerDown;
            CommandKeeper.Instance.LockGameUI(false);

            CommandKeeper.Instance.WriteState("IdleState"); 
        }

        private void FingerDown()
        {
            CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.StartPainting);
        }

        public override void ExitState()
        {
            base.ExitState();
            CommandKeeper.Instance.BoardFingerDown -= FingerDown;
            CommandKeeper.Instance.LockGameUI(true);
        }
    }
}