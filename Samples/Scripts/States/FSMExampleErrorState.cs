using UnityEngine;

namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleErrorState : AbstractState<FSMExampleState>, IErrorState
    {
        private readonly FSMExampleErrorBehaviour errorBehaviour;

        public FSMExampleErrorState(FSMExampleErrorBehaviour errorBehaviour)
        {
            this.errorBehaviour = errorBehaviour;
            this.errorBehaviour.BackToWelcome += OnBackToWelcome;
            this.errorBehaviour.gameObject.SetActive(false);
        }

        private void OnBackToWelcome()
        {
            stateMachine.ChangeState(FSMExampleState.Welcome);
        }

        protected override void Enter(FSMExampleState previousState)
        {
            errorBehaviour.gameObject.SetActive(true);
            Debug.LogError($"An error occured: {ErrorMessage}");
        }

        protected override void Exit(FSMExampleState nextState)
        {
            errorBehaviour.gameObject.SetActive(false);
        }

        public string ErrorMessage { get; set; }
    }
}