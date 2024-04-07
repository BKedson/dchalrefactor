using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserVerificationStateMachineNew : BaseStateMachine
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override NewState CurrentState()
    {
        return NewState.Finished;
    }

    public override bool IsValid(Action action)
    {
        return false;
    }
}
