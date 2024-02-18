using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Challenges;

public class ForgeChallengeController : ChallengeController
{
    //stores a reference to the Foundry Room State Machine
    FoundryRoomStateMachine roomStateMachine;

    //overriding the OnTriggerEnter function
    protected override void OnTriggerEnter(Collider other)
    {
        //additional condition to chack if Challenge is active
        if(other.GetComponent<Collider>().tag == "Player" && IsCorrectState()){
            HandleInputAction(ChallengeAction.WalkIn);  
        } 
    }

    //checks whether we are in the correct state to activate the Challenge
    protected bool IsCorrectState()
    {
        return roomStateMachine.GetCurrentState() == FoundryRoomStateMachine.FoundryRoomState.BuildingNumber;
    }
}
