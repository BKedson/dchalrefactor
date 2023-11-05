using UnityEngine;
using System.Collections;

public class DoorChallenge : Challenge
{
    //---METHODS---
    //constructor
    public DoorChallenge(Question question) : base(question){ }

    private void Start(){
        // Initialize the challenge, e.g., set the question, UI, etc.
    }

    //called when starting a challenge
    public override void StartChallenge(){
        //Testing Debug Log
        Debug.Log("Challenge Started..");

        CurrentState = ChallengeState.PlayerAttempting;
        // Handle UI or door interactions specific to starting the challenge
    }

    //called when the player gets a correct answer - challenge completed
    public override void FinishChallenge(){
        //Testing Debug Log
        Debug.Log("Challenge Completed..");

        CurrentState = ChallengeState.Completed;
        // Handle UI or door interactions specific to finishing the challenge
    }

    //called when a challenge is forfeited
    public override void ForfeitChallenge(){
        //Testing Debug Log
        Debug.Log("Challenge Forfeited..");

        CurrentState = ChallengeState.WaitingForPlayer;
        //handle UI or door interactions specific to leaving a challenge without completion
    }

    //called when the player gets a wrong answer
    public override void FailChallenge(){
        //Testing Debug Log
        Debug.Log("Challenge Failed..");
        //Trigger actions for a wrong answer like Trial account
        // Handle UI or door interactions specific to failing the challenge
    }

    //called to indicate whether the player has made an attempt
    public override bool AttemptChallenge(){
        //logic to check button presses related to answering the question
        return false;
    }

    //used to check whether the solution is correct
    public override bool IsCorrectSolution(){
        //logic to check the correctness of a solution
        return false;
    }
}
