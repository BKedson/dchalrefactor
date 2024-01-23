using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour, IStateMachineNew
{
    public abstract bool IsValid(Action action);

    public abstract NewState CurrentState();
}
