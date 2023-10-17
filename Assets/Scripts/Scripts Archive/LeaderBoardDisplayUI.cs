using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardDisplayUI : MonoBehaviour
{
    //stores a reference to the NameChange UI
    public GameObject scoresUIElement;
   

    //enable and disable UI of this element
    public void disableUI(){
        GenericUIController.disableUI(scoresUIElement);
    }
    public void enableUI(){
        GenericUIController.enableUI(scoresUIElement);
    }
}
