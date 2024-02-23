using UnityEngine;
using System.Collections;
using dchalrefactor.Scripts.Animations.Challenges.Doors;

namespace dchalrefactor.Scripts.Challenges
{
    public class DoorChallenge : Challenge
    {
        //VARIABLES
        //Stores the Animations manager for the door
        public DoorAnimations anim;
        //---METHODS---
        //constructor
        public DoorChallenge(Question question) : base(question){ }

        private void Start(){
            // Initialize the challenge, e.g., set the question, UI, etc.
        }

        //called when starting a challenge
        public override void OnChallengeStart(){
            //Testing Debug Log
            Debug.Log("Challenge Started..");
            anim.OpenDoor();
        }

        //called when the player gets a correct answer - challenge completed
        public override void OnChallengePass(){
            //Testing Debug Log
            Debug.Log("Challenge Completed..");
        }

        //called when a challenge is forfeited
        public override void OnAttempCancel(){
            //Testing Debug Log
            Debug.Log("Challenge Forfeited..");
            anim.CloseDoor();
        }

        //called when the player gets a wrong answer
        public override void OnChallengeFail(){
            //Testing Debug Log
            Debug.Log("Challenge Failed..");
            //Trigger actions for a wrong answer like Trial account
            // Handle UI or door interactions specific to failing the challenge
        }

        //called to indicate whether the player has made an attempt
        public override bool AttemptChallenge(){
            //Check input
            if(Input.GetKeyDown("return")){
                return true;
            }
            //logic to check button presses related to answering the question
            else{
                return false;
            }
        }

        //used to check whether the solution is correct
        public override bool IsCorrectSolution(){
            return false;
        }
    }
}
