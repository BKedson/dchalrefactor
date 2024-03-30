using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundryRoomStateMachine : BaseStateMachine<FoundryRoomController.FoundryRoomAction , FoundryRoomStateMachine.FoundryRoomState>
{
    //Stores the possible Foundry Room states for the state machine
    public enum FoundryRoomState{
        WaitingForPlayer,
        WindowStage,
        BuildingNumber,
        BuildingWeapon,
        DoorUnlocked,
        RoomComplete
    }

    //overriding the Get Default method 
    protected override FoundryRoomState GetDefaultState()
    {
        return FoundryRoomState.WaitingForPlayer;
    }

    protected override void HandleStateChange(FoundryRoomController.FoundryRoomAction action)
    {
        switch(action)
        {
            case FoundryRoomController.FoundryRoomAction.EnterRoom:
                if(GetCurrentState()==FoundryRoomState.WaitingForPlayer)
                {
                    ChangeState(FoundryRoomState.WindowStage, action);
                }
                break;
            case FoundryRoomController.FoundryRoomAction.CorrectWindowSolution:
                if(GetCurrentState()==FoundryRoomState.WindowStage)
                    {
                        ChangeState(FoundryRoomState.BuildingNumber, action);
                    }
                    break;
            case FoundryRoomController.FoundryRoomAction.CorrectNumberSolution:
                if(GetCurrentState()==FoundryRoomState.BuildingNumber)
                    {
                        ChangeState(FoundryRoomState.BuildingWeapon, action);
                    }
                    break;
            case FoundryRoomController.FoundryRoomAction.PickupWeapon:
                if(GetCurrentState()==FoundryRoomState.BuildingWeapon)
                    {
                        ChangeState(FoundryRoomState.DoorUnlocked, action);
                    }
                    break;
            case FoundryRoomController.FoundryRoomAction.ExitRoom:
                if(GetCurrentState()==FoundryRoomState.DoorUnlocked)
                    {
                        ChangeState(FoundryRoomState.RoomComplete, action);
                    }
                    break;
        }
    }
}
