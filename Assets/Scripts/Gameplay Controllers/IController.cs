using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IController<InputAction, IState> : MonoBehaviour 
    where InputAction : System.Enum
    where IState : System.Enum
{
    //reference to the EventHandler
    public IEventHandler<InputAction, IState> eventHandler;
    //reference to the IModel or Manager
    public IModel manager;
    //declares an input Action delegate
    public delegate void InputActionHandler(InputAction action); 
    public event InputActionHandler OnInputAction;
    //declares an output action delegate - capable of holding any of the methods defined as outputs in the IModel
    public delegate void OutputActionHandler();
    public OutputActionHandler outputAction;

    //Subscription to the StateMachineEvent delegate for output
    protected virtual void Start()
    {
        //subscribes to the OnStateChange delegate from stateMachine
        eventHandler.OnStateChangeEvent += HandleOutputAction;
    }

    //called by the various trigger functions in this controller
    protected void HandleInputAction(InputAction action){
        OnInputAction?.Invoke(action);
    }

    protected void HandleOutputAction(InputAction action){
        //update the delegate accordingly
        UpdateDelegate(action);
        outputAction();
    } 
    //method that sets a method delegate
    protected void SetDelegate(OutputActionHandler outputActionHandler){
        outputAction = outputActionHandler;
    }
    //method that updates the current delegate output function according to the action
    protected abstract void UpdateDelegate(InputAction action);
}
