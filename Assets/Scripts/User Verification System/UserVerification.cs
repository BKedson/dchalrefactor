using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.UserVerificationSystem
{
    public class UserVerification : MonoBehaviour
    {
        //methods to handle different Actions during User verification
        public void OnRegisterPressed()
        {
            //Load the Register UI Page
        }

        public void OnRegisterCancel()
        {
            //Revert to the Login UI Page
        }
        
        public void OnLogin()
        {
            //Query the database for presence or absence of user
        }

        public void OnLoginInvalid()
        {
            //Result from Query - false, activate false indicator UI element on Login Page
        }

        public void OnLoginValid()
        {
            //Result from Query - true, Load the user's data into the game
        }

        public void OnRegisterAttempt()
        {
            //Query the database for presence or absence of user
        }

        public void OnRegisterInvalid()
        {
            //Result from Query - false, activate false indicator UI element on Login Page
        }

        public void OnRegisterValid()
        {
            //Result from Query - true, Load new user's data into the game
        }

        public void OnGuestPressed()
        {
            //Load the Guest UI Page
        }

        public void OnGuestCancel()
        {
            //Revert to the Login UI Page
        }

        public void OnGuestCreated()
        {
            //Load new User's data into the game
        }

        //Method to check user presence or absence in Database
        public bool IsUserPresent(){
            //return true or false
            //future implementation to handle exceptions when it comes to database queries
            return true;
        }
    }
}
