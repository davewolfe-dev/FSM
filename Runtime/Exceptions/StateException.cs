using System;
using JetBrains.Annotations;

namespace DaveWolfe.FSM
{
    public class StateException<T> : Exception where T : Enum
    {
        public T TransitionToState { get; }
        
        public string ErrorMessage { get; }
        
        public Exception InitialException { get; }
        
        public StateException(string errorMessage, T transitionToState, Exception initialException = null)
        {
            ErrorMessage = errorMessage;
            TransitionToState = transitionToState;
            InitialException = initialException;
        }
    }
}
