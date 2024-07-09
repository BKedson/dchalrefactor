using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using dchalrefactor.Scripts.UserVerificationSystem;

public class UserVerificationController : BaseController<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the transition manager for this controller
    public UserVerification uvManager;
    //stores a reference to the Player Data Manager and data controller for the player
    public PlayerDataManager dataManager;
    public PlayerGameDataController dataController;

    //store the UI elements associated with the Player's Identity
    public GameObject LoginFirstNameInput;
    public GameObject LoginNickNameInput;
    public GameObject LoginCodeNumberInput;
    public GameObject RegisterFirstNameInput;
    public GameObject RegisterNickNameInput;
    public GameObject RegisterCodeNumberInput;
    private string firstName;
    private string nickName;
    private int codeNumber;

    //stores the possible input Actions for this Controller
    public enum UVAction
    {
        GuestButtonClick, CancelGuest, GuestUserCreated, RegisterButtonClick, CancelRegister, LoginButtonClick, CancelLogin, LoginInputError,
        RegisterOperation, RegisterErrorUserExists, LoginOperation, LoginErrorNoSuchUser, RegisterUserCreated, LoginUserLoaded, RegisterInputError
    }
    
    //Start 
    protected virtual void Start(){   
    }

    //INPUTS 
    //PAGES-------------------------------------------------------
    public void OnClickRegisterPage()
    {
        HandleInputAction(UVAction.RegisterButtonClick);
    }

    public void OnClickGuestPage()
    {
        HandleInputAction(UVAction.GuestButtonClick);
    }

    public void OnClickLoginPage()
    {
        HandleInputAction(UVAction.LoginButtonClick);
    }

    //CANCEL BUTTONS------------------------------------------------
    public void OnClickRegisterCancel()
    {
        HandleInputAction(UVAction.CancelRegister);
    }

    public void OnClickGuestCancel()
    {
        HandleInputAction(UVAction.CancelGuest);
    }

    public void OnClickLoginCancel()
    {
        HandleInputAction(UVAction.CancelLogin);
    }

    //OPERATIONS-------------------------------------------------------
    public void OnClickLogin()
    {
        HandleInputAction(UVAction.LoginOperation);
    }

    public void OnClickRegister()
    {
        HandleInputAction(UVAction.RegisterOperation);
    }

    //USER CREATIONS AND PROCEED TO MAIN MENU-------------------------------------------------------------------
    public void OnClickGuest()
    {
        HandleInputAction(UVAction.GuestUserCreated);
    }

    //ERROR CHECKS AND CLOUD VERIFICATION------------------------------------------------------------------------
    public async void RegisterUserAbsenceCheck()
    {
        //check Input credentials - if invalid, call manager error
        if(!AreRegisterCredentialInputsValid())
        {
            //returns the state machine to the RegisterPage state
            HandleInputAction(UVAction.RegisterInputError);
        }
        
        //Check with the data manager for succesful registration
        else if(await dataManager.IsRegisrationSuccessful(GenerateRegisterCredentials()[0],GenerateRegisterCredentials()[1]))
        {
            //loading game file into manager-loginData
            dataManager.InitializeGameLoginData(firstName, nickName, codeNumber);
            //Call the MainMenu sequence after loading the data
            HandleInputAction(UVAction.RegisterUserCreated);
        }
        else {
            uvManager.SetErrorMessage("Cannot register, user already exists or cloud is unavailable.");
            HandleInputAction(UVAction.RegisterErrorUserExists);
        }
    }

    public async void LoginUserPresenceCheck()
    {
        //check Input credentials - if invalid, call manager error
        if(!AreLoginCredentialInputsValid())
        {
            //returns the state machine to the RegisterPage state
            HandleInputAction(UVAction.LoginInputError);
        }
        //The normal condition would be if the user is present
        else if(await dataManager.IsLoginSuccessful(GenerateLoginCredentials()[0],GenerateLoginCredentials()[1]))
        {
            //download the file from the cloud to the manager's login file
            dataManager.RetrieveGameDataFromCloud();
            //Indicate old user - IndicateNewUser(false)
            dataController.IndicateNewUser(false);
            //Call the MainMenu sequence after loading the data
            HandleInputAction(UVAction.LoginUserLoaded);
        }
        else
        {
            uvManager.SetErrorMessage("Names or number are incorrect, or cloud is unavailable.");
            HandleInputAction(UVAction.LoginErrorNoSuchUser);
        }    
    }

    //MASTER METHOD FOR CREDENTIALS-------------------------------------------------------------------------
    protected bool AreLoginCredentialInputsValid()
    {
        //check if the names and number are valid
        firstName = NameToString(LoginFirstNameInput);
        nickName = NameToString(LoginNickNameInput);
        codeNumber = NumberToInteger(LoginCodeNumberInput);
        return IsUserNameValid(firstName) && IsUserNameValid(nickName) && IsCodeNumberValid(codeNumber);
    }
    protected bool AreRegisterCredentialInputsValid()
    {
        //check if the names and number are valid
        firstName = NameToString(RegisterFirstNameInput);
        nickName = NameToString(RegisterNickNameInput);
        codeNumber = NumberToInteger(RegisterCodeNumberInput);
        return IsUserNameValid(firstName) && IsUserNameValid(nickName) && IsCodeNumberValid(codeNumber);
    }
    protected string[] GenerateLoginCredentials()
    {
        //standardize the first name and nickname
        firstName = UsernameToTitleCase(firstName);
        nickName = UsernameToTitleCase(nickName);
        //generate the username and password
        string username = GenerateUsername(firstName, nickName, codeNumber);
        string password = GeneratePassword(firstName, nickName, codeNumber);
        //create array
        string[] result = {username,password};
        return result;
    }
    protected string[] GenerateRegisterCredentials()
    {
        //standardize the first name and nickname
        firstName = UsernameToTitleCase(firstName);
        nickName = UsernameToTitleCase(nickName);
        //generate the username and password
        string username = GenerateUsername(firstName, nickName, codeNumber);
        string password = GeneratePassword(firstName, nickName, codeNumber);
        //create array
        string[] result = {username,password};
        return result;
    }

    //INTERMEDIATE CHECKERS---------------------------------------------
    //used to make sure the name is btn 3 and 10 characters and has no non-alphabetic chars 
    protected bool IsUserNameValid(string s)
    {
        //if longer than 10 characters, reject
        if(s.Length > 10 || s.Length < 3)
        {
            uvManager.SetErrorMessage("Names must contain between 3 and 10 characters.");
            return false;
        }
        //looping to make sure the string only has valid chars
        foreach (char c in s)
        {
            if (!char.IsLetter(c))
            {
                uvManager.SetErrorMessage("Names must contain only letters.");
                return false;
            }
        }
        return true;
    }
    //used to check the code number
    protected bool IsCodeNumberValid(int n)
    {
        //if bigger than 9, reject
        if(n < 0 || n > 9)
        {
            uvManager.SetErrorMessage("Number must be between 0 and 9.");
            return false;
        }
        return true;
    }

    //Used to obtain Usernames to strings
    protected string NameToString(GameObject obj)
    {
        //obtain the UI elements and return the String
        return obj.GetComponent<TMP_InputField>().text;
    }
    protected int NumberToInteger(GameObject obj)
    {
        //obtain the UI elements and return the Integer
        //If nothing has been entered, return an invalid number to prevent number format exception
        if (obj.GetComponent<TMP_InputField>().text == ""){
            return 10;
        }
        return int.Parse(obj.GetComponent<TMP_InputField>().text);
    }
    //Used to generate Usernames and passwords
    protected string GenerateUsername(string FirstName, string NickName, int CodeNumber)
    {
        return FirstName + "_" + NickName + "_" + CodeNumber.ToString();
    }

    protected string GeneratePassword(string FirstName, string NickName, int CodeNumber)
    {
        return FirstName + "_" + NickName + "_" + CodeNumber.ToString() + "!";
    }

    //used to create a uniform representation for all names in the game
    protected string UsernameToTitleCase(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }

    //------------------------------------------------------------------
    //OUTPUTS-----------------------------------------------------------
    //------------------------------------------------------------------
    protected override void UpdateDelegate(UVAction action){
        //update the default delegate according to the action
        switch(action)
        {
            case UVAction.RegisterButtonClick:
                SetDelegate(uvManager.OnRegisterPressed);
                break;
            case UVAction.CancelRegister:
                SetDelegate(uvManager.OnRegisterCancel);
                break;
            case UVAction.LoginButtonClick:
                SetDelegate(uvManager.OnLoginPressed);
                break;
            case UVAction.LoginErrorNoSuchUser: //ui signal error
                SetDelegate(uvManager.OnLoginInvalid);
                break;
            case UVAction.LoginUserLoaded: //load the data to dataController and go to start menu
                SetDelegate(uvManager.OnLoginValid); 
                break; 
            case UVAction.RegisterOperation: // make DataManager call-internal
                SetDelegate(RegisterUserAbsenceCheck); 
                break;
            case UVAction.RegisterErrorUserExists: //ui signal error
                SetDelegate(uvManager.OnRegisterInvalid); 
                break;
            case UVAction.RegisterUserCreated: //create a new user,load to dataController and go to start menu
                SetDelegate(uvManager.OnRegisterValid); 
                break;
            case UVAction.GuestButtonClick:
                SetDelegate(uvManager.OnGuestPressed); 
                break;
            case UVAction.CancelGuest:
                SetDelegate(uvManager.OnGuestCancel); 
                break;
            case UVAction.GuestUserCreated:
                SetDelegate(uvManager.OnGuestCreated); 
                break;
            case UVAction.LoginOperation: //make DataManager call-internal
                SetDelegate(LoginUserPresenceCheck); 
                break;
            case UVAction.CancelLogin:
                SetDelegate(uvManager.OnLoginCancel); 
                break;
            case UVAction.RegisterInputError:
                SetDelegate(uvManager.OnRegisterInputError); 
                break;
            case UVAction.LoginInputError:
                SetDelegate(uvManager.OnLoginInputError); 
                break;
        }
    }
}
