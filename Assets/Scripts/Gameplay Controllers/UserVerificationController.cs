using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.UserVerificationSystem;

public class UserVerificationController : IController<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the manager for this controller
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
        //type casting default IModel type manager to Challenge
        uvManager = (UserVerification)manager;
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
