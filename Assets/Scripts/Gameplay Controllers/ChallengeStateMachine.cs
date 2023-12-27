using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines a generic state machine for a challenge of the entire game
public class ChallengeStateMachine : IStateMachine<ChallengeController.ChallengeAction, ChallengeStateMachine.ChallengeState>
{
    // Stores the possible Challenge states for the state machine
    public enum ChallengeState{
        WaitingForPlayer,
        PlayerAttempting,
        VerityCheck,
        Completed
    }

    //overriding the get default state method - defines teh default state for this state machine
    protected override ChallengeState GetDefaultState(){
        return ChallengeState.WaitingForPlayer;
    }

    //Depending on the EventHandler information
    protected override void HandleStateChange(ChallengeController.ChallengeAction action) //this function initiates events caused by state changes on all challenges
    {
        // Handle state transitions and state-specific logic here - make sure the current state matches the current action to cause a transition
        switch(action)
        {
            case ChallengeController.ChallengeAction.WalkIn:
                if(GetCurrentState() == ChallengeState.WaitingForPlayer)
                    {
                        ChangeState(ChallengeState.PlayerAttempting, action);
                    }
                break;
            case ChallengeController.ChallengeAction.Forfeit:
                if(GetCurrentState() == ChallengeState.PlayerAttempting)
                    {
                        ChangeState(ChallengeState.WaitingForPlayer, action);
                    }
                break;
            case ChallengeController.ChallengeAction.Pass:
                if(GetCurrentState() == ChallengeState.VerityCheck)
                    {
                        ChangeState(ChallengeState.Completed, action);
                    }
                break;
            case ChallengeController.ChallengeAction.Fail:
                if(GetCurrentState() == ChallengeState.VerityCheck)
                    {
                        ChangeState(ChallengeState.PlayerAttempting, action);
                    }
                break;  
            case ChallengeController.ChallengeAction.Attempt:
                if(GetCurrentState() == ChallengeState.PlayerAttempting)
                    {
                        ChangeState(ChallengeState.VerityCheck, action);
                    }
                break;
        }
    }
}
