using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IStateMachine interface is the main abstraction for StateMachine objects. 
// TODO: Rename when refactor is complete
public interface IStateMachineNew
{
    // Returns true if the action is valid and false otherwise
    bool IsValid(Action action);

    // Returns the current state
    NewState CurrentState();
}
