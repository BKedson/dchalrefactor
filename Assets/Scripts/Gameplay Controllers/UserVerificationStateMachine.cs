using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserVerificationStateMachine : BaseStateMachine<UserVerificationController.UVAction , UserVerificationStateMachine.UVState>
{
    //stores the possible states that this state machine can exist in
    public enum UVState{
        StartPage,
        LoginPage,
        RegisterPage,
        GuestPage,
        PresenceCheck,
        AbsenceCheck,
        MainMenu,
    }

    //overriding to set the default state
    protected override UVState GetDefaultState(){
        return UVState.StartPage;
    }

    protected override void HandleStateChange(UserVerificationController.UVAction action){
        // Handle state transitions and state-specific logic here - make sure the current state matches the current action to cause a transition
        switch(action)
        {
            case UserVerificationController.UVAction.RegisterButtonClick:
                if(GetCurrentState() == UVState.StartPage)
                    {
                        ChangeState(UVState.RegisterPage, action);
                    }
                break;
            case UserVerificationController.UVAction.CancelRegister:
                if(GetCurrentState() == UVState.RegisterPage)
                    {
                        ChangeState(UVState.StartPage, action);
                    }
                break;
            case UserVerificationController.UVAction.LoginButtonClick:
                if(GetCurrentState() == UVState.StartPage)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;
            case UserVerificationController.UVAction.LoginErrorNoSuchUser:
                if(GetCurrentState() == UVState.PresenceCheck)
                    {
                        ChangeState(UVState.LoginPage, action);
                    }
                break;  
            case UserVerificationController.UVAction.LoginUserLoaded:
                if(GetCurrentState() == UVState.PresenceCheck)
                    {
                        ChangeState(UVState.MainMenu, action);
                    }
                break;
            case UserVerificationController.UVAction.RegisterOperation:
                if(GetCurrentState() == UVState.RegisterPage)
                    {
                        ChangeState(UVState.AbsenceCheck, action);
                    }
                break;
            case UserVerificationController.UVAction.RegisterErrorUserExists:
                if(GetCurrentState() == UVState.AbsenceCheck)
                    {
                        ChangeState(UVState.RegisterPage, action);
                    }
                break;
            case UserVerificationController.UVAction.RegisterUserCreated:
                if(GetCurrentState() == UVState.AbsenceCheck)
                    {
                        ChangeState(UVState.MainMenu, action);
                    }
                break;
            case UserVerificationController.UVAction.GuestButtonClick:
                if(GetCurrentState() == UVState.StartPage)
                    {
                        ChangeState(UVState.GuestPage, action);
                    }
                break;
            case UserVerificationController.UVAction.CancelGuest:
                if(GetCurrentState() == UVState.GuestPage)
                    {
                        ChangeState(UVState.StartPage, action);
                    }
                break;
            case UserVerificationController.UVAction.GuestUserCreated:
                if(GetCurrentState() == UVState.GuestPage)
                    {
                        ChangeState(UVState.MainMenu, action);
                    }
                break;
            case UserVerificationController.UVAction.LoginOperation:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.PresenceCheck, action);
                    }
                break;
            case UserVerificationController.UVAction.CancelLogin:
                if(GetCurrentState() == UVState.LoginPage)
                    {
                        ChangeState(UVState.StartPage, action);
                    }
                break;
        }
    }
}
