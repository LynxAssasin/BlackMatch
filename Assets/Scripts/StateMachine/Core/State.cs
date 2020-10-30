using UnityEngine;
namespace Game.StateMachine
{
	public class State : MonoBehaviour {

		protected bool working;
		protected string stateName;

		public virtual bool ImmidiateState{ get { return false; } }
        public string StateName
		{  
			get
			{ 
				return stateName; 
			}
		}

		public State InitState(string stateName)
		{  
			this.stateName = stateName;  
			return this;  
		}

		public virtual void EnterState() 
		{  
			working = true;  
		} 

		public virtual void ExitState()
		{
			working = false; 
		}

	}
}