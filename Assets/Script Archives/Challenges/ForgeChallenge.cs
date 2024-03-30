using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Challenges
{
    public class ForgeChallenge : Challenge
    {
        //---METHODS---
        //constructor
        public ForgeChallenge(Question question) : base(question){ }

        private void Start(){
            // Initialize the window challenge, e.g., set the question, UI, etc.
            //Remeber that this window is within a larger Foundry System
        }

        //called when starting a challenge
        public override void OnChallengeStart(){
            //Testing Debug Log
            Debug.Log("Forge Challenge Started..");
        }

        //called when the player gets a correct answer - challenge completed
        public override void OnChallengePass(){
            //Remember - finishing the window should activate the Forge
            //Testing Debug Log
            Debug.Log("Forge Challenge Completed..");
        }

        //called when a challenge is forfeited
        public override void OnAttempCancel(){
            //Testing Debug Log
            Debug.Log("Forge Challenge Forfeited..");
        }

        //called when the player gets a wrong answer
        public override void OnChallengeFail(){
            //Testing Debug Log
            Debug.Log("Forge Challenge Failed..");
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