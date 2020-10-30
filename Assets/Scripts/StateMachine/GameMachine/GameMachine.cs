using UnityEngine;
using Game.StateMachine;
using Game.GameCore.States;
namespace Game.GameCore
{
    public class GameMachine : MonoBehaviour
    {
        private StateMachineGeneric<State, Transition> stateMachine;  

        public void StartMachine()
        {
            stateMachine = new StateMachineGeneric<State, Transition>(gameObject);
            CreateStates();
            CreatTransitions();
            CommandKeeper.Instance.MakeTransitionInGame += MakeTransition; 
            stateMachine.SetDefaultState(State.Game_Start);

        }


        private void CreateStates()
        {
            stateMachine.CreateState<GameStart>(State.Game_Start);
            stateMachine.CreateState<GameIdleState>(State.Game_Idle);
            stateMachine.CreateState<GamePaintingState>(State.Game_Painting);
            stateMachine.CreateState<DestroyElementsState>(State.Animation_DestroyElements);
            stateMachine.CreateState<ElementsDownState>(State.Animation_ElementsDown);
            stateMachine.CreateState<LoseGameState>(State.Game_Lose);
            stateMachine.CreateState<WinGameState>(State.Game_Win);
            stateMachine.CreateState<PauseGameState>(State.Game_Pause);
        }

        private void CreatTransitions()
        {
            //Start Game
            stateMachine.CreateTransition(Transition.MoveDownElements, State.Game_Start, State.Animation_ElementsDown);

            //Game Loop
            stateMachine.CreateTransition(Transition.StartPainting, State.Game_Idle, State.Game_Painting);
            stateMachine.CreateTransition(Transition.BackToIdle, State.Game_Painting, State.Game_Idle);
            stateMachine.CreateTransition(Transition.DestroyElements, State.Game_Painting, State.Animation_DestroyElements);
            stateMachine.CreateTransition(Transition.MoveDownElements, State.Animation_DestroyElements, State.Animation_ElementsDown);
            stateMachine.CreateTransition(Transition.BackToIdle, State.Animation_ElementsDown, State.Game_Idle);

            //Pause And Finish Game 
            stateMachine.CreateTransition(Transition.PauseGame, State.Game_Idle, State.Game_Pause);
            stateMachine.CreateTransition(Transition.PauseGame, State.Game_Painting, State.Game_Pause);
            stateMachine.CreateTransition(Transition.BackToIdle, State.Game_Pause, State.Game_Idle);

            stateMachine.CreateTransition(Transition.WinGame, State.Game_Idle, State.Game_Win);
            stateMachine.CreateTransition(Transition.WinGame, State.Game_Painting, State.Game_Win);
            stateMachine.CreateTransition(Transition.WinGame, State.Animation_DestroyElements, State.Game_Win);
            stateMachine.CreateTransition(Transition.WinGame, State.Animation_ElementsDown, State.Game_Win);

            stateMachine.CreateTransition(Transition.LoseGame, State.Game_Idle, State.Game_Lose);
            stateMachine.CreateTransition(Transition.LoseGame, State.Game_Painting, State.Game_Lose);
            stateMachine.CreateTransition(Transition.LoseGame, State.Animation_DestroyElements, State.Game_Lose);
            stateMachine.CreateTransition(Transition.LoseGame, State.Animation_ElementsDown, State.Game_Lose);
        }
    
        public void MakeTransition(Transition transition)
        {
            stateMachine.MakeTransition(transition);
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.MakeTransitionInGame -= MakeTransition;
            stateMachine.StopMachine();
            stateMachine = null;
        }

        public enum State
        {
            Game_Start = 0, 
            Game_Idle = 1,
            Game_Painting = 2,
            Animation_DestroyElements = 3,
            Animation_ElementsDown = 4,

            Game_Win = 9,
            Game_Lose = 10,

            Game_Pause = 11,

        }

        public enum Transition
        {
            StartPainting = 0,
            BackToIdle = 1,

            DestroyElements = 2,
            MoveDownElements = 3,

            WinGame = 4,
            LoseGame = 5, 
            PauseGame = 6,

        }
    }
}