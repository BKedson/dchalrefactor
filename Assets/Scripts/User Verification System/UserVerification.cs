using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using dchalrefactor.Scripts.Animations.UI.LoginMenu.Classes;

namespace dchalrefactor.Scripts.UserVerificationSystem
{
    public class UserVerification : MonoBehaviour
    {
        //stores the UI Pages for the User Verification System
        public GameObject StartPage;
        public GameObject GuestPage;
        public GameObject LoginPage;
        public GameObject RegisterPage;
        public GameObject PromptErrorText;
        //stores the Animations Class for the Start Menu/ User Verification
        public StartMenuAnimations MenuAnim;
        //stores the Transition manager to load Main Menu scene
        public TransitionManager transitionManager;

        // Allow for specific error messages
        private string errorMessage = "";
        //-----------------------------------------------------------------------------------------
        //methods to handle different Actions during User verification-----------------------------
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

        public void OnLoginInvalid()
        {
            //Result from Query - false, activate false indicator UI element on Login Page
            PromptErrorText.GetComponent<TMP_Text>().text = errorMessage;
            //Indicate error
            IndicateError();
            Debug.Log("Login Invalid!");
        }

        public void OnLoginInputError()
        {
            //Invalid input
            PromptErrorText.GetComponent<TMP_Text>().text = errorMessage;
            //Indicate error
            IndicateError();
        }

        public void OnLoginValid()
        {
            //Load the user's data into the game - go to Main menu
            Debug.Log("Load the Main Menu");
            //load the main menu
            transitionManager.Menu();
        }

        public void OnRegisterInvalid()
        {
            //Result from Query - false, activate false indicator UI element on Login Page
            PromptErrorText.GetComponent<TMP_Text>().text = errorMessage;
            //Indicate error
            IndicateError();
        }

        public void OnRegisterInputError()
        {
            //Invalid input
            PromptErrorText.GetComponent<TMP_Text>().text = errorMessage;
            //Indicate error
            IndicateError();
        }

        public void OnRegisterValid()
        {
            //Load new user's data into the game - go to Main menu
            Debug.Log("Load the Main Menu");
            transitionManager.Menu();
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
            //Load new User's data into the game - go to start menu
        }

        //------------------------------------------------------------------------------------------
        //ADDITONAL HELPER METHODS------------------------------------------------------------------
        //Username Error Indicator
        public void IndicateError()
        {
            //simply sets it active
            MenuAnim.PromptError();
        }

        public void SetErrorMessage(string message){
            errorMessage = message;
        }
    }
}
