using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericUIController : object
{
    //this class has two funtions. Disable or enable UI elements
    public static void disableUI(GameObject element){
        element.SetActive(false);
    }

    //this method has two funtions. Disable or enable UI elements
    public static void enableUI(GameObject element){
        element.SetActive(true);
    }
}
