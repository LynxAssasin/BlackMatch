using Game.StateMachine;
namespace Game.GameCore.States
{
    public class ElementsDownState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.CreateNewElements(()=> 
            {
                CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.BackToIdle);
            });
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}