using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines the base definition for any logic controller in the game
public abstract class BaseController<InputAction, IState> : MonoBehaviour
    where InputAction : System.Enum
    where IState : System.Enum
{
    //reference to the State Machine
    public BaseStateMachine<InputAction,IState> stateMachine;

    //declares an input Action delegate
    public delegate void InputActionHandler(InputAction action); 
    public event InputActionHandler OnInputAction;

    //declares an output action delegate - capable of holding any of the methods defined as outputs in the respective Transition manager
    public delegate void OutputActionHandler();
    public OutputActionHandler OnOutputAction;

    //Subscription to the StateMachine delegate for any State Change calls
    protected virtual void Awake()
    {
        //subscribes to the OnStateChange delegate from stateMachine
        stateMachine.OnStateChange += HandleOutputAction;
    }

    //called by the various trigger functions in this controller
    protected void HandleInputAction(InputAction action){
        OnInputAction?.Invoke(action);
    }

    protected void HandleOutputAction(InputAction action){
        //update the delegate accordingly
        UpdateDelegate(action);
        OnOutputAction();
    } 
    
    //method that sets a method delegate
    protected void SetDelegate(OutputActionHandler outputActionHandler){
        OnOutputAction = outputActionHandler;
    }
    //method that updates the current delegate output function according to the action
    protected abstract void UpdateDelegate(InputAction action);
}
