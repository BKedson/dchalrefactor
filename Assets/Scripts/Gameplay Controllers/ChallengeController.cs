using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Challenges;

public class ChallengeController : IController<ChallengeController.ChallengeAction, ChallengeStateMachine.ChallengeState>
{
    //stores the manager for this controller
    Challenge challengeManager;

    //Defined actions for state Transitions
    public enum ChallengeAction{
        WalkIn,
        Forfeit,
        Pass,
        Fail,
    }

    //Start 
    protected virtual void Start(){
        //type casting default IModel type manager to Challenge
        challengeManager = (Challenge)manager;
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
                //check for correctness
                if(challengeManager.IsCorrectSolution()){
                    HandleInputAction(ChallengeAction.Pass);
                }
                else{
                    HandleInputAction(ChallengeAction.Fail);
                }
            }
        } 
    }

    protected void OnTriggerExit(Collider other) //when the player leaves the challenge zone
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            HandleInputAction(ChallengeAction.Forfeit);
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
        }
    }
}
