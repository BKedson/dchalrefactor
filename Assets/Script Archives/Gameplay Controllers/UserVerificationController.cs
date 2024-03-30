using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.UserVerificationSystem;

public class UserVerificationController : BaseController<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the transition manager for this controller
    UserVerification uvManager;

    //stores the possible input Actions for this Controller
    public enum UVAction{
        RegisterPressed,
        CancelRegister,
        Login,
        NoSuchLoginUser,
        UserLoaded,
        CheckAbsence,
        RegisterUserExists,
        NewUserCreated,
        Guest,
        CancelGuest,
        GuestUserCreated
    }

    //Start 
    protected virtual void Start(){
        
    }

    //INPUTS 
    //Button CLicks
    protected void OnClickRegisterPage()
    {
        HandleInputAction(UVAction.RegisterPressed);
    }

    protected void OnClickGuestPage()
    {
        HandleInputAction(UVAction.Guest);
    }

    protected void OnClickRegisterCancel()
    {
        HandleInputAction(UVAction.CancelRegister);
    }

    protected void OnClickLogin()
    {
        HandleInputAction(UVAction.Login);
    }

    protected void OnClickRegisterUser()
    {
        HandleInputAction(UVAction.CheckAbsence);
    }

    protected void OnClickGuestUser()
    {
        HandleInputAction(UVAction.GuestUserCreated);
    }

    protected void OnClickGuestCancel()
    {
        HandleInputAction(UVAction.CancelGuest);
    }

    protected void RegisterUserAbsenceCheck()
    {
        //The normal condition would be if the user is absent
        if(uvManager.IsUserPresent()){
            HandleInputAction(UVAction.RegisterUserExists);
        }
        else{
            HandleInputAction(UVAction.NewUserCreated);
        }
    }

    protected void LoginUserPresenceCheck()
    {
        //The normal condition would be if the user is present
        if(uvManager.IsUserPresent()){
            HandleInputAction(UVAction.UserLoaded);
        }
        else{
            HandleInputAction(UVAction.NoSuchLoginUser);
        }    
    }
    //OUTPUTS
    protected override void UpdateDelegate(UVAction action){
        //update the default delegate according to the action
        switch(action)
        {
            case UVAction.RegisterPressed:
                SetDelegate(uvManager.OnRegisterPressed);
                break;
            case UVAction.CancelRegister:
                SetDelegate(uvManager.OnRegisterCancel);
                break;
            case UVAction.Login:
                SetDelegate(uvManager.OnLogin);
                break;
            case UVAction.NoSuchLoginUser:
                SetDelegate(uvManager.OnLoginInvalid);
                break;
            case UVAction.UserLoaded:
                SetDelegate(uvManager.OnLoginValid); 
                break; 
            case UVAction.CheckAbsence:
                SetDelegate(uvManager.OnRegisterAttempt); 
                break;
            case UVAction.RegisterUserExists:
                SetDelegate(uvManager.OnRegisterInvalid); 
                break;
            case UVAction.NewUserCreated:
                SetDelegate(uvManager.OnRegisterValid); 
                break;
            case UVAction.Guest:
                SetDelegate(uvManager.OnGuestPressed); 
                break;
            case UVAction.CancelGuest:
                SetDelegate(uvManager.OnGuestCancel); 
                break;
            case UVAction.GuestUserCreated:
                SetDelegate(uvManager.OnGuestCreated); 
                break;
        }
    }
}
