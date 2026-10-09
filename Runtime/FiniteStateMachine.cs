using System;
using System.Collections.Generic;
using UnityEngine;

namespace DaveWolfe.FSM
{
    /// <summary>
    /// A finite state machine for Unity. Before creating a state machine instance, you must first
    /// define an Enum that enumerates the valid states for this instance. The first state in the
    /// enum should be something like None or Initializing.
    /// </summary>
    /// <remarks>
    /// Packages require XmlDoc documentation for ALL Package APIs.
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments
    /// </remarks>
    public class FiniteStateMachine<T> where T : Enum
    {
        public delegate void StateChangeDelegate(T oldState, T newState);
        public event StateChangeDelegate StateWillChange;
        public event StateChangeDelegate StateDidExit;
        public event StateChangeDelegate StateDidEnter;
        
        public T CurrentState { get; private set; }
        public T PreviousState { get; private set; }

        private AbstractState<T> CurrentStateInstance => states.ContainsKey(CurrentState) ? states[CurrentState] : null;

        private readonly Dictionary<T, AbstractState<T>> states = new();
        
        /// <summary>
        /// Adds a state to the collection of valid states for this state machine instance. 
        /// </summary>
        public void AddState(T state, AbstractState<T> stateInstance)
        {
            if (stateInstance == null)
            {
                Debug.LogError("Attempted to add null state");
                return;
            }

            if (states.ContainsKey(state))
            {
                Debug.LogError($"{state} has already been added");
                return;
            }

            stateInstance.StateEncounteredError += OnStateEncounteredError;

            stateInstance.StateMachine = this;
            states[state] = stateInstance;
        }

        /// <summary>
        /// Changes to the specified state. The current states Exit method will be called, followed by the new states Enter method.
        /// </summary>
        public void ChangeState(T state)
        {
            if (state == null)
            {
                Debug.LogError("Attempted to set a null state");
                return;
            }

            if (!states.ContainsKey(state))
            {
                Debug.LogError($"Cannot change to a state that has not been added: {state}");
                return;
            }

            if (Equals(state, CurrentState))
            {
                Debug.LogWarning($"Cannot change to {state} because it's already the current state");
                return;
            }
            
            Debug.Log($"Changing from {CurrentState} to {state}");

            PreviousState = CurrentState;
            CurrentState = state;
            
            StateWillChange?.Invoke(PreviousState, CurrentState);
            
            try
            { 
                if (states.TryGetValue(PreviousState, out var exitingStateInstance))
                {
                    exitingStateInstance.ExitState(CurrentState);
                }
            }
            catch (StateException<T> exception)
            {
                HandleStateException(exception);
            }
            
            StateDidExit?.Invoke(PreviousState, CurrentState);

            try
            {
                states[CurrentState].EnterState(PreviousState);
            }
            catch (StateException<T> exception)
            {
                HandleStateException(exception);
            }
            
            StateDidEnter?.Invoke(PreviousState, CurrentState);
        }

        /// <summary>
        /// Updates the current state. This should be called in your main scripts Update method.
        ///
        /// Calling UpdateState is optional as states will often include GameObjects that have their own Update method. 
        /// </summary>
        public void UpdateState()
        {
            try
            {
                CurrentStateInstance?.UpdateState();
            }
            catch (StateException<T> exception)
            {
                HandleStateException(exception);
            }
        }

        private void OnStateEncounteredError(string errorMessage, T transitionToState, Exception initialException)
        {
            Debug.LogError($"{errorMessage} \n{initialException}");
            
            if (IsStateValid(transitionToState))
            {
                if (states[transitionToState] is IErrorState stateInstance)
                {
                    stateInstance.ErrorMessage = errorMessage;
                }
                ChangeState(transitionToState);
            }
            else
            {
                Debug.LogError($"{CurrentState} raised a StateError event but did not provide a valid state to transition to: {transitionToState}");
            }
        }
        
        private void HandleStateException(StateException<T> exception)
        {
            Debug.LogError($"{exception.ErrorMessage} \n{exception.InitialException}");

            if (IsStateValid(exception.TransitionToState))
            {
                CurrentState = exception.TransitionToState;
                if (states[CurrentState] is IErrorState stateInstance)
                {
                    stateInstance.ErrorMessage = exception.ErrorMessage;
                }
                states[CurrentState].EnterState(PreviousState);
            }
            else
            {
                Debug.LogError($"{exception.TransitionToState} is not a valid state for this state machine instance.");
            }
        }

        private bool IsStateValid(T state)
        {
            return state != null && 
                   states.ContainsKey(state) && 
                   !EqualityComparer<T>.Default.Equals(state, CurrentState);
        }
    }
}
