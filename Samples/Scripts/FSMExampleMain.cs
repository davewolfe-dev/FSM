using System;
using DaveWolfe.FSM.Examples.States;
using UnityEngine;

namespace DaveWolfe.FSM.Examples
{
    public enum FSMExampleState
    {
        None, // The first element in the Enum will be the initial state of the state machine, it doesn't need to be implemented
        Welcome, 
        Info, 
        Error
    }
    
    public class FSMExampleMain : MonoBehaviour
    {
        [SerializeField] private FSMExampleWelcomeBehaviour welcomeBehaviour;
        [SerializeField] private FSMExampleInfoBehaviour infoBehaviour;
        [SerializeField] private FSMExampleErrorBehaviour errorBehaviour;
        
        private FiniteStateMachine<FSMExampleState> stateMachine;

        private void Awake()
        {
            stateMachine = new FiniteStateMachine<FSMExampleState>();
            stateMachine.AddState(FSMExampleState.Welcome, new FSMExampleWelcomeState(welcomeBehaviour));
            stateMachine.AddState(FSMExampleState.Info, new FSMExampleInfoState(infoBehaviour));
            stateMachine.AddState(FSMExampleState.Error, new FSMExampleErrorState(errorBehaviour));
            stateMachine.ChangeState(FSMExampleState.Welcome);
        }

        private void Update()
        {
            // calling UpdateState is optional, as states will usually be paired with a MonoBehaviour and update logic can be placed there
            stateMachine?.UpdateState();
        }
    }
}