using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Challenges;

public class ChallengeController : BaseController<ChallengeController.ChallengeAction, ChallengeStateMachine.ChallengeState>
{
    //stores the transition manager for this controller
    public Challenge challengeManager;

    //Defined actions for state Transitions
    public enum ChallengeAction{
        WalkIn,
        Forfeit,
        Pass,
        Fail,
        Attempt
    }

    //Start 
    protected virtual void Start(){
        
    }

    //INPUTS
    //concrete methods that are implemented by all challenges
    protected void OnTriggerEnter(Collider other) //when a player enters the challenge zone
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            HandleInputAction(ChallengeAction.WalkIn);  
        }    
    }

    protected void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            //if while in Attempting - manager attempts the challenge
            if(challengeManager.AttemptChallenge()){
                HandleInputAction(ChallengeAction.Attempt);
            }
        } 
    }

    protected void OnTriggerExit(Collider other) //when the player leaves the challenge zone
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            HandleInputAction(ChallengeAction.Forfeit);
        }      
    }

    protected void PassOrFail() // called to pass or fail the player's attempt
    {
        bool isCorrectSolution = challengeManager.IsCorrectSolution();
        if(isCorrectSolution)
        {
            HandleInputAction(ChallengeAction.Pass);
        }
        else{
            HandleInputAction(ChallengeAction.Fail);
        }
    }

    //OUTPUTS
    protected override void UpdateDelegate(ChallengeAction action){
        //update the default delegate according to the action
        switch(action)
        {
            case ChallengeAction.WalkIn:
                SetDelegate(challengeManager.OnChallengeStart);
                break;
            case ChallengeAction.Forfeit:
                SetDelegate(challengeManager.OnAttempCancel);
                break;
            case ChallengeAction.Pass:
                SetDelegate(challengeManager.OnChallengePass);
                break;
            case ChallengeAction.Fail:
                SetDelegate(challengeManager.OnChallengeFail);
                break;
            case ChallengeAction.Attempt:
                SetDelegate(PassOrFail); 
                break; 
        }
    }
}
