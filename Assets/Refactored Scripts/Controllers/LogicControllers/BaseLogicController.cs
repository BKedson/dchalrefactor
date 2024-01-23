using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IChallenge interface.
// Extended by all logic controllers.
public abstract class BaseLogicController : MonoBehaviour, IController
{
    // The state machine for this controller
    internal readonly BaseStateMachine machine;

    public abstract void TakeAction(Action action);

    public abstract void InputToAction();

}
