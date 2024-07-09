using System;

namespace ATG.StateMachine
{
	public abstract class Statement
	{
		protected readonly IStateSwitcher _stateSwitcher;

		public event Action OnExit;

		public Statement(IStateSwitcher sw)
		{
			_stateSwitcher = sw;
		}

        public abstract void Enter();

        public abstract void Execute();

		public virtual void Exit()
		{
            OnExitInvoke();
		}


        private void OnExitInvoke()
        {
            if (OnExit == null) return;
            OnExit.Invoke();
        }
	}
}

