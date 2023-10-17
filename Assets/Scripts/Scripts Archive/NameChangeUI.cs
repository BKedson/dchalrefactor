using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChangeUI : MonoBehaviour
{
    //stores a reference to the NameChange UI
    public GameObject nameChangeUIElement;
    public GameObject nameReminderUIElement;

    //enable and disable UI of this element
    public void disableUI(){
        GenericUIController.disableUI(nameChangeUIElement);
        GenericUIController.disableUI(nameReminderUIElement);
    }
    public void enableUI(){
        GenericUIController.enableUI(nameChangeUIElement);
    }
}
