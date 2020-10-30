using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.StateMachine
{
    public class StateMachineGeneric<N, M>
                                      where N : struct, IComparable, IConvertible, IFormattable
                                      where M : struct, IComparable, IConvertible, IFormattable
    {

        private GameObject parent;

        private State currentState;
        private bool inTransition;

        private List<State> states;
        private List<Transition> transitions;


        public StateMachineGeneric(GameObject parent)
        {
            this.parent = parent;
            states = new List<State>();
            transitions = new List<Transition>();
        }

        public virtual void SetDefaultState(N state)
        {
            currentState =  GetState(state.ToString());
            currentState.EnterState();
        }
        
        public virtual void MakeTransition(M transitionName)
        {
            string transitionNameStr = transitionName.ToString();
            if (!inTransition)
            {
                Transition transition = GetTransition(transitionNameStr, currentState);
                if (transition != null)
                {
                    inTransition = true;
                    transition.Invoke(ChangeState);
                }
            }
        }

        public void CreateState<S>(N stateName) where S : State
        {
            string stateNameStr = stateName.ToString();
            if (GetState(stateNameStr) == null)
            {
                states.Add(parent.AddComponent<S>().InitState(stateNameStr));
            }
            else
            {
                Debug.LogWarning(String.Format("State with name {0} already exist. Can't create it. State machine {1}",
                                stateNameStr,
                                parent.name));
            }
        }

        public void CreateTransition(M transitionName, N from, N to)
        {
            string transitionNameStr = transitionName.ToString();
            State stateFrom = GetState(from.ToString());
            State stateTo = GetState(to.ToString());

            if (stateFrom == null)
            {
                Debug.LogWarning(String.Format("State <from> with name {0} not exist. Can't create transition {1}. State machine {2}",
                                from,
                                transitionNameStr,
                                parent.name));
                return;
            }

            if (stateTo == null)
            {
                Debug.LogWarning(String.Format("State <to> with name {0} not exist. Can't create transition {1}. State machine {2}",
                                to,
                                transitionNameStr,
                                parent.name));
                return;
            }

            if (GetTransition(transitionNameStr, stateFrom) != null)
            {
                Debug.LogWarning(String.Format("Transition from state {0} with name {1} already exist. State machine {2}",
                                from,
                                transitionName,
                                parent.name));
                return;
            }
        
            transitions.Add(new Transition(transitionNameStr, stateFrom, stateTo));
        }

        protected void ChangeState(State state)
        {
            currentState = state;
            inTransition = false;
        }

        private State GetState(string name)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].StateName == name)
                {
                    return states[i];
                }
            }
            return null;
        }

        private Transition GetTransition(string name, State state)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].CheckNameAndState(name, state))
                {
                    return transitions[i];
                }
            }
            return null;
        }

        public void StopMachine()
        {
            currentState.ExitState();
            states = null;
            transitions = null;
            GC.Collect();
        }
    }
}