# FSM
## Finite State Machine for Unity

-----------------------

FSM is a simple finite state machine for use with your Unity projects. It is 
a plain C# library, the only Unity dependency is for debug logs.  

### Usage
#### State Enum
First, create an Enum that defines the states your state machine will use.  The first 
entry in the Enum should always be something like `None` or `Undefined`. When the 
state machine instance is created, this will be the initial state until you change 
to a valid state.  Additionally, you must define at least one error state that 
will be the default state used when an exception is thrown.

    public enum MyStates 
    { 
        None, 
        Loading, 
        Start, 
        Settings, 
        Playing, 
        Paused, 
        End, 
        Error 
    }

#### State Instance
State instances must derive from AbstractState\<T>, where T is the Enum your Enum.  State 
instances are only required to implement two methods, `Enter` and `Exit`.  `UpdateState` 
may also be implemented if required, but often your states will include MonoBehaviours 
which have their own Update function. A common pattern is to define a state with a 
constructor that requires a reference to a MonoBehaviour:

    public ExampleState : AbstractState<MyStates>
    {
        private ExampleBehaviour exampleBehaviour;

        public ExampleState(ExampleBehaviour exampleBehaviour)
        {
            this.exampleBehaviour = exampleBehaviour;
        }

        protected override void Enter(MyStates previousState)  
        {
            if (previousState == MyStates.Loading)
            {
                exampleBehaviour.DoSomething();
            }
        }
        
        protected override void Exit(MyStates nextState) {}
    }

State instances have a reference to the state machine from the `stateMachine` field. You 
can use this to change states from within your state instance without the need to 
pass a reference to the state machine.

#### Error Handling

If an error is encountered, there are several ways to handle it. In the `Enter`, `Exit`, 
and `UpdateState` methods, you can throw a `StateException`.  This will cause the 
state machine instance to change state to the TransitionTo state passed to the 
`StateException` constructor.  If an error is encountered in a different method, 
such as an event handler, you can call the `RaiseStateEncounteredError` method. 

For example, if a networked game loses connectivity, you can transition to a 
`WifiDisconnected` state that tells the user to check their Internet connection. 
If the `TransitionToState` implements the `IErrorState` interface, the state machine 
will set the `ErroMessage` property of the error state before transitioning to it.  
This is useful for a generic error state where the displayed error message may be 
different depending on the error.

#### State Machine Instance

Create a new instance of the state machine, and add states using the `AddState` method.
Once all the state instances have been created and added to the state machine, you can 
change to your starting state when desired.

    private void InitStateMachine()
    {
        stateMachine = new FiniteStateMachine<MyStates>();

        stateMachine.AddState(MyStates.Loading, new LoadingState(loadingBehaviour));
        stateMachine.AddState(MyStates.Start, new StartState(startBehaviour));
        stateMachine.AddState(MyStates.Settings, new SettingsState(settingsBehaviour));
        stateMachine.AddState(MyStates.Playing, new PlayingState(gameManager, playerController));
        stateMachine.AddState(MyStates.Paused, new PausedState(gameManager));
        stateMachine.AddState(MyStates.End, new EndState(endBehaviour));
        stateMachine.AddState(MyStates.Error, new ErrorState(errorBehaviour));
        
        stateMachine.ChangeState(MyStates.Start);
    }