using Game.StateMachine;
using UnityEngine; 
namespace Game.AppCore.States
{
    public class StartAppState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.ShowStartAppMenu();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}