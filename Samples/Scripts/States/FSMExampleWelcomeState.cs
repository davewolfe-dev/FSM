using UnityEngine;

namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleWelcomeState : AbstractState<FSMExampleState>
    {
        private readonly FSMExampleWelcomeBehaviour welcomeBehaviour;

        public FSMExampleWelcomeState(FSMExampleWelcomeBehaviour welcomeBehaviour)
        {
            this.welcomeBehaviour = welcomeBehaviour;
            this.welcomeBehaviour.ShowInfo += OnShowInfo;
            this.welcomeBehaviour.gameObject.SetActive(false);
        }

        private void OnShowInfo()
        {
            stateMachine.ChangeState(FSMExampleState.Info);
        }

        protected override void Enter(FSMExampleState previousState)
        {
            welcomeBehaviour.gameObject.SetActive(true);
        }

        protected override void Exit(FSMExampleState nextState)
        {
            welcomeBehaviour.gameObject.SetActive(false);
        }
    }
}