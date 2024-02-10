using UnityEngine;
using System.Collections;

namespace dchalrefactor.Scripts.Challenges
{
    public abstract class Challenge : MonoBehaviour
    {
        // Stores the Question associated with this challenge
        public Question question;
        
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

        public abstract bool AttemptChallenge(); //called to assert that player has made an attempt to the challenge - used for verity check

        public abstract bool IsCorrectSolution(); //checks whether the player correctly solved the solution - returns false if not

    }
}