using Game.StateMachine;
using UnityEngine;
namespace Game.AppCore.States
{
    public class MainMenuState : State
    {
        public override void EnterState()
        {
            base.EnterState();
            CommandKeeper.Instance.LoadMenu(); 
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}