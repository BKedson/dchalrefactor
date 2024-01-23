using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IChallenge interface.
// Extended by all challenge controllers.
public abstract class BaseChallengeController : MonoBehaviour, IController
{
    // The state machine for this controller
    internal readonly BaseStateMachine machine;

    internal readonly BaseChallenge challenge;

    public abstract void TakeAction(Action action);

    public abstract void InputToAction();

}
