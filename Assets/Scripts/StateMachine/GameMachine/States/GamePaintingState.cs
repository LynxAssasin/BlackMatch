using Game.StateMachine;
namespace Game.GameCore.States
{
    public class GamePaintingState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.EnablePainting(true);
            CommandKeeper.Instance.BoardFingerUp += FingerUp;
            CommandKeeper.Instance.LockGameUI(false);

            CommandKeeper.Instance.WriteState("Painting");

        }

        public void FingerUp()
        {
            bool canBeDelete = CommandKeeper.Instance.CheckToDeleteElements();
            if (canBeDelete)
            {
                CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.DestroyElements);
            }
            else
            {
                CommandKeeper.Instance.StopPainting();
                CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.BackToIdle);
            }
        }
        public override void ExitState()
        {
            base.ExitState();
            CommandKeeper.Instance.EnablePainting(false);
            CommandKeeper.Instance.BoardFingerUp -= FingerUp;
            CommandKeeper.Instance.LockGameUI(true);
        }
    }
}