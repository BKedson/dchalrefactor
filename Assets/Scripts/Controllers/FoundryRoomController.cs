using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Managers.Rooms;
public class FoundryRoomController : BaseController<FoundryRoomController.FoundryRoomAction , FoundryRoomStateMachine.FoundryRoomState>
{
   //stores the transition manager for this controller
   FoundryRoomManager manager;

   //stores the possible input actions for this controller
   public enum FoundryRoomAction{
    EnterRoom,
    CorrectWindowSolution,
    CorrectNumberSolution,
    PickupWeapon,
    ExitRoom
   }

   //overriding the Update Delegate method
   protected override void UpdateDelegate(FoundryRoomAction action)
   {
        switch(action)
        {
            case FoundryRoomAction.EnterRoom:
                SetDelegate(manager.OnForgeEnter);
                break;
            case FoundryRoomAction.CorrectWindowSolution:
                SetDelegate(manager.OnWindowComplete);
                break;
            case FoundryRoomAction.CorrectNumberSolution:
                SetDelegate(manager.OnNumberComplete);
                break;
            case FoundryRoomAction.PickupWeapon:
                SetDelegate(manager.OnWeaponPickup);
                break;
            case FoundryRoomAction.ExitRoom:
                SetDelegate(manager.OnForgeExit);
                break;
        }
   }
}
