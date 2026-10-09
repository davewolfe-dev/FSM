using System;

namespace DaveWolfe.FSM
{
    public abstract class AbstractState<T> where T : Enum
    {
        public delegate void StateError(string errorMessage, T transitionToState, Exception initialException = null);

        public event StateError StateEncounteredError;

        protected FiniteStateMachine<T> stateMachine;

        internal FiniteStateMachine<T> StateMachine
        {
            get => stateMachine;
            set => stateMachine = value;
        }

        protected abstract void Enter(T previousState);

        protected abstract void Exit(T nextState);

        internal void EnterState(T previousState)
        {
            Enter(previousState);
        }

        internal void ExitState(T previousState)
        {
            Exit(previousState);
        }

        public virtual void UpdateState() {}

        protected void RaiseStateEncounteredError(string errorMessage, T transitionToState,
            Exception initialException = null)
        {
            StateEncounteredError?.Invoke(errorMessage, transitionToState, initialException);
        }
    }
}