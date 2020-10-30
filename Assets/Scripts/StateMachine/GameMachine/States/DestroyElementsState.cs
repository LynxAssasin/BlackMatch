using Game.StateMachine;
namespace Game.GameCore.States
{
    public class DestroyElementsState : State
    {
        public override bool ImmidiateState { get { return true; } }
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.DeletePaintedElements(ElementsDeleted); 
        }

        public void ElementsDeleted()
        {
            if (!CommandKeeper.Instance.CheckGameFinish())
            {
                CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.MoveDownElements);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}