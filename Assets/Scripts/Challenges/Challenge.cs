using UnityEngine;
using System.Collections;

public abstract class Challenge : MonoBehaviour
{
    // This abstract class represents a Generic Challenge that is presented to the player

    // Stores the Question associated with this challenge
    public Question question;
    // Stores the current state of the Door Challenge
    [SerializeField] protected ChallengeState CurrentState = ChallengeState.WaitingForPlayer; //Default State
    // Stores the different states that the challenge could exist in relation to the player
    public enum ChallengeState{
        WaitingForPlayer,
        PlayerAttempting,
        Completed
    }

    // Constructor
    public Challenge(Question question){
        //assign the question to the variable
        this.question = question;
    }

    //abstract methods that are implemented by all challenges
    public abstract void OnChallengeStart(); //called when a challenge is started

    public abstract void OnChallengePass(); //called when a challenge is completed - Player is correct

    public abstract void OnChallengeFail();  //called when the player gets a wrong answer on a challenge - Player is wrong

    public abstract void OnAttempCancel(); //called when a player leaves a challenge without completing

    public abstract bool AttemptChallenge(); //called to check whether the player has made an attempt to the challenge

    public abstract bool IsCorrectSolution(); //checks whether the player correctly solved the solution - returns false if not

    //concrete methods that are implemented by all challenges
    protected void OnTriggerEnter(Collider other) //when a player enters the challenge zone
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            //the trigger should do nothing if the state is not currently waiting for the player - initiate attempting state
            if (CurrentState == ChallengeState.WaitingForPlayer){ 
                HandleTriggerEvent(ChallengeState.PlayerAttempting);
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            //the trigger should only do something if the player was in a state of attempting
            if (CurrentState == ChallengeState.PlayerAttempting){
                //if the player succeeds or fails - we update state accordingly
                //Has the player made an answer attempt
                if(AttemptChallenge()){
                    //Okay now is the attempt right or wrong
                    if(IsCorrectSolution()){
                        //challenge completion
                        HandleTriggerEvent(ChallengeState.Completed);  
                    }
                    else{
                        //they fail the challenge
                        OnChallengeFail();
                    }
                }
            }
        }
    }

    protected void OnTriggerExit(Collider other) //when the player leaves the challenge zone
    {
        if(other.GetComponent<Collider>().tag == "Player"){
            //the trigger should only do something if the player was in a state of attempting - return to waiting
            if (CurrentState == ChallengeState.PlayerAttempting){ 
                HandleTriggerEvent(ChallengeState.WaitingForPlayer);
            }
        }
    }

    
    protected void HandleTriggerEvent(ChallengeState nextState) //this function initiates events caused by state changes on all challenges
    {
        // Handle state transitions and state-specific logic here
        switch (nextState)
        {
            case ChallengeState.PlayerAttempting: 
                OnChallengeStart(); // StartChallenge behavior
                break;
            case ChallengeState.WaitingForPlayer:
                OnAttempCancel(); //ForfeitChallenge behavior
                break;
            case ChallengeState.Completed:
                OnChallengePass(); // FinishChallenge behavior
                break;
        }
        // Update the current state to the next state to reflect the transition
        CurrentState = nextState;
    }
}