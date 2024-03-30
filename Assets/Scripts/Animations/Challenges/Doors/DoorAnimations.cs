using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Animations.Challenges.Doors
{
    public class DoorAnimations : MonoBehaviour
    {
        //Stores and handles all the animations related to the Door Challenges
        public Animator doorAnimator;

        //DOORS------------------------------------
        //methods to open and close the door
        public void OpenDoor()
        {
            doorAnimator.SetTrigger("Open_Door");
        }

        public void CloseDoor()
        {
            doorAnimator.SetTrigger("Close_Door");
        }
    }
}
