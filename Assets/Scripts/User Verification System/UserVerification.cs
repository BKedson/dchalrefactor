using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.UserVerificationSystem
{
    public class UserVerification : MonoBehaviour
    {
        //stores the UI Pages for the User Verification System
        public GameObject StartPage;
        public GameObject GuestPage;
        public GameObject LoginPage;
        public GameObject RegisterPage;

        //methods to handle different Actions during User verification
        public void OnRegisterPressed()
        {
            //Load the Register UI Page
            RegisterPage.SetActive(true);
        }

        public void OnRegisterCancel()
        {
            //Revert to the Start UI Page
            RegisterPage.SetActive(false);
        }
        
        public void OnLoginPressed()
        {
            //Load the Login UI Page
            LoginPage.SetActive(true);
        }

        public void OnLoginCancel()
        {
            //Revert to the Login UI Page
            LoginPage.SetActive(false);
        }
        public void OnLoginAttempt()
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
            GuestPage.SetActive(true);
        }

        public void OnGuestCancel()
        {
            //Revert to the Start UI Page
            GuestPage.SetActive(false);
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
