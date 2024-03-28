using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.UserVerificationSystem;

public class UserVerificationController : BaseController<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the transition manager for this controller
    public UserVerification uvManager;

    //stores the possible input Actions for this Controller
    public enum UVAction
    {
        GuestButtonClick, CancelGuest, GuestUserCreated, RegisterButtonClick, CancelRegister, LoginButtonClick, CancelLogin, 
        RegisterOperation, RegisterErrorUserExists, LoginOperation, LoginErrorNoSuchUser, RegisterUserCreated, LoginUserLoaded
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

    //USER CREATIONS AND PROCEED TO MAIN MENU----------------------------
    public void OnClickGuest()
    {
        HandleInputAction(UVAction.GuestUserCreated);
    }

    //ERROR CHECKS AND CLOUD VERIFICATION--------------------------------
    public void RegisterUserAbsenceCheck()
    {
        //The normal condition would be if the user is absent
        if(uvManager.IsUserPresent()){
            HandleInputAction(UVAction.RegisterErrorUserExists);
        }
        else{
            HandleInputAction(UVAction.RegisterUserCreated);
        }
    }

    public void LoginUserPresenceCheck()
    {
        //The normal condition would be if the user is present
        if(uvManager.IsUserPresent())
        {
            HandleInputAction(UVAction.LoginUserLoaded);
        }
        else
        {
            HandleInputAction(UVAction.LoginErrorNoSuchUser);
        }    
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
            case UVAction.LoginErrorNoSuchUser:
                SetDelegate(uvManager.OnLoginInvalid);
                break;
            case UVAction.LoginUserLoaded:
                SetDelegate(uvManager.OnLoginValid); 
                break; 
            case UVAction.RegisterOperation:
                SetDelegate(uvManager.OnRegisterAttempt); 
                break;
            case UVAction.RegisterErrorUserExists:
                SetDelegate(uvManager.OnRegisterInvalid); 
                break;
            case UVAction.RegisterUserCreated:
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
            case UVAction.LoginOperation:
                SetDelegate(uvManager.OnLoginAttempt); 
                break;
            case UVAction.CancelLogin:
                SetDelegate(uvManager.OnLoginCancel); 
                break;
        }
    }
}
