namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleInfoState: AbstractState<FSMExampleState>
    {
        private readonly FSMExampleInfoBehaviour infoBehaviour;

        public FSMExampleInfoState(FSMExampleInfoBehaviour infoBehaviour)
        {
            this.infoBehaviour = infoBehaviour;
            this.infoBehaviour.ShowError += OnShowError;
            this.infoBehaviour.gameObject.SetActive(false);
        }

        private void OnShowError()
        {
            RaiseStateEncounteredError("There was an error.", FSMExampleState.Error);
        }

        protected override void Enter(FSMExampleState previousState)
        {
            infoBehaviour.gameObject.SetActive(true);
        }

        protected override void Exit(FSMExampleState nextState)
        {
            infoBehaviour.gameObject.SetActive(false);
        }
    }
}