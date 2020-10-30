using UnityEngine;
using Game.StateMachine;
using Game.AppCore.States;
 
namespace Game.AppCore
{
    public class AppMachine : MonoBehaviour
    {
        private StateMachineGeneric<State, Transition> stateMachine;

        public void StartMachine()
        {
            stateMachine = new StateMachineGeneric<State, Transition>(gameObject);
            CreateStates();
            CreatTransitions();
            CommandKeeper.Instance.MakeTransitionInApp += MakeTransition;
            stateMachine.SetDefaultState(State.App_Start);

        }

        private void CreateStates()
        {
            stateMachine.CreateState<StartAppState>(State.App_Start);
            stateMachine.CreateState<MainMenuState>(State.Main_Menu);
            stateMachine.CreateState<PlayingGameState>(State.Playing_Game);
    
        }

        private void CreatTransitions()
        {

            stateMachine.CreateTransition(Transition.ToMainMenu, State.App_Start, State.Main_Menu);
            stateMachine.CreateTransition(Transition.ToMainMenu, State.Playing_Game, State.Main_Menu);
            stateMachine.CreateTransition(Transition.StartPlayingGame, State.Main_Menu, State.Playing_Game);
            stateMachine.CreateTransition(Transition.ReturnToStartScreen, State.Main_Menu, State.App_Start);
        }

        public void MakeTransition(Transition transition)
        {
            stateMachine.MakeTransition(transition);
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.MakeTransitionInApp -= MakeTransition;
            stateMachine.StopMachine();
            stateMachine = null;
        }

        public enum State
        {
            App_Start = 0,
            Main_Menu = 1,
            Playing_Game = 2,
        }

        public enum Transition
        {
            ToMainMenu = 0, 
            StartPlayingGame = 1, 
            ReturnToStartScreen = 2, 
        }
    }
}