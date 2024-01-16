using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines the base definition for any state machine in the game
public abstract class IStateMachine<InputAction,IState> : MonoBehaviour 
    where IState : System.Enum
    where InputAction : System.Enum
 {
    //reference to the Eventhandler
    public IEventHandler<InputAction,IState> eventHandler;
    
    //declares a state Change delegate delegate
    public delegate void StateChangeHandler(InputAction action); 
    public event StateChangeHandler OnStateChange;

    //stores the current active state in this State machine among its siblings 
    protected IState currentState; 
    //stores the default State of this state machine, the entry state
    protected IState defaultState;

    //start method to set the current State to the default state at the beginning
    protected virtual void Awake(){
        defaultState = GetDefaultState();
        ChangeState(defaultState);
    }

    protected virtual void Start()
    {
        // Subscribes to the OnControllerEvent delegate from eventHandler
        eventHandler.OnControllerEvent += HandleStateChange;
    }

    //default method that must be overriden to sure that a default state is set with its return value
    protected abstract IState GetDefaultState();

    //returns the current state of this state machine
    public IState GetCurrentState(){
        return currentState;
    }

    //method overload
    protected void ChangeState(IState newState){
        //sets the current state to the new State
        currentState  = newState;
    }
    //method to handle the changing of states for this state Machine
    protected void ChangeState(IState newState, InputAction action){
        //sets the current state to the new State
        ChangeState(newState);
        //Invokes the OnStateChange delegate hence firing off all methods in the EventHandler subscribed to it
        OnStateChange?.Invoke(action);
    }

    //method to handle the state changes of this State machine - varies according to the state machine
    protected abstract void HandleStateChange(InputAction inputEvent);
}
