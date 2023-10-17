using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPageUIController : MonoBehaviour
{
    //stores the gameObject containing the credits page
    public GameObject credits;

    //method to set active or inactive
    public void openCredits(bool activate){
        //if bool is true
        if(activate){
            //set the UI element to active
            credits.SetActive(true);
        }
        else{
            //set to inactive - called by a button
            credits.SetActive(false);
        }
    }
}
