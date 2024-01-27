using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserVerificationStateMachine : BaseStateMachine<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the possible states that this state machine can exist in
    public enum UVState{
        LoginPage,
        RegisterPage,
        GuestPage,
        PresenceCheck,
        AbsenceCheck,
        StartMenu,
    }

    //overriding to set the default state
    protected override UVState GetDefaultState(){
        return UVState.LoginPage;
    }

    protected override void HandleStateChange(UserVerificationController.UVAction action){
        // Handle state transitions and state-specific logic here - make sure the current state matches the current action to cause a transition
        switch(action)
        {
            case UserVerificationController.UVAction.RegisterPressed:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.RegisterPage, action);
                    }
                break;
            case UserVerificationController.UVAction.CancelRegister:
                if(GetCurrentState() == UVState.RegisterPage)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;
            case UserVerificationController.UVAction.Login:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.PresenceCheck, action);
                    }
                break;
            case UserVerificationController.UVAction.NoSuchLoginUser:
                if(GetCurrentState() == UVState.PresenceCheck)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;  
            case UserVerificationController.UVAction.UserLoaded:
                if(GetCurrentState() == UVState.PresenceCheck)
                    {
                        ChangeState(UVState.StartMenu, action);
                    }
                break;
            case UserVerificationController.UVAction.CheckAbsence:
                if(GetCurrentState() == UVState.RegisterPage)
                    {
                        ChangeState(UVState.AbsenceCheck, action);
                    }
                break;
            case UserVerificationController.UVAction.RegisterUserExists:
                if(GetCurrentState() == UVState.AbsenceCheck)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;
            case UserVerificationController.UVAction.NewUserCreated:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.StartMenu, action);
                    }
                break;
            case UserVerificationController.UVAction.Guest:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.GuestPage, action);
                    }
                break;
            case UserVerificationController.UVAction.CancelGuest:
                if(GetCurrentState() == UVState.GuestPage)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;
            case UserVerificationController.UVAction.GuestUserCreated:
                if(GetCurrentState() == UVState.GuestPage)
                    {
                        ChangeState(UVState.StartMenu, action);
                    }
                break;
        }
    }
}
