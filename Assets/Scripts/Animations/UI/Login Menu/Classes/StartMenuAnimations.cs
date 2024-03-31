using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Animations.UI.LoginMenu.Classes
{
    public class StartMenuAnimations : MonoBehaviour
    {
        //Stores and handles all the animations related to the Door Challenges
        public Animator ErrorPromptAnimator;

        //DOORS------------------------------------
        //methods to open and close the door
        public void PromptError()
        {
            ErrorPromptAnimator.SetTrigger("Prompt");
        }
    }
}
