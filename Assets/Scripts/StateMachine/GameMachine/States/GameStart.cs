using System.Collections;
using UnityEngine;
using Game.StateMachine;
namespace Game.GameCore.States
{
    public class GameStart : State
    {
        public override void EnterState()
        {
            base.EnterState();

            //To Check!! 
            StartCoroutine(WaitSomeTime());
        }

        IEnumerator WaitSomeTime()
        {
            yield return new WaitForSeconds(1);
            CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.MoveDownElements);
        }
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}