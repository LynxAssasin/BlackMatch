using System;  

namespace Game.StateMachine
{ 
	public class Transition 
	{

        private string transitionName;  
		private State from;  
		private State to;  

		public Transition(string transitionName, State from, State to)
		{  
			this.from = from;  
			this.to = to;
            this.transitionName = transitionName; 
		}
		
        public bool CheckNameAndState(string transitionName, State from)
        {
            return (transitionName == this.transitionName) && (from == this.from); 
        }	

		public virtual void Invoke(Action<State> returnNewState)
		{  
			from.ExitState ();
			if (!to.ImmidiateState)
			{
				to.EnterState();
				returnNewState(to);
			}
			else
			{
				returnNewState(to);
				to.EnterState();
			}
		}

	}
}
