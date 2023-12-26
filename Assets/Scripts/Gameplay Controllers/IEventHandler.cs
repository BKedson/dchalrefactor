using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEventHandler<TControllerEvent, TStateMachineEvent> : MonoBehaviour
    where TControllerEvent : System.Enum
    where TStateMachineEvent : System.Enum
{
    //references to the controller and State Machine
    public IController<TControllerEvent, TStateMachineEvent> controller;
    public IStateMachine<TControllerEvent,TStateMachineEvent> stateMachine;

    //declares a controller Event delegate
    public delegate void ControllerEventHandler(TControllerEvent controllerEvent);
    public event ControllerEventHandler OnControllerEvent;
    //declares a state Machine event delegate
    public delegate void StateChangeEventHandler(TControllerEvent controllerEvent);
    public event StateChangeEventHandler OnStateChangeEvent;

    protected virtual void Start()
    {
        // Subscribes to the OnInputAction delegate from controller
        controller.OnInputAction += HandleControllerEvent;
        //subscribes to the OnStateChange delegate from stateMachine
        stateMachine.OnStateChange += HandleStateChangeEvent;
    }
    //Methods to handle events from the stateMachine and the controller
    protected void HandleControllerEvent(TControllerEvent controllerEvent)
    {
        OnControllerEvent?.Invoke(controllerEvent);
    }

    protected void HandleStateChangeEvent(TControllerEvent controllerEvent)
    {
        OnStateChangeEvent?.Invoke(controllerEvent);       
    }
}
