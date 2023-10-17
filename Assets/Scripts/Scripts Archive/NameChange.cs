using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameChange : MonoBehaviour
{
    //THIS CLASS IS UNDER THE NAMECHANGE ELEMENT
    //stores the TMP element
    public TMP_InputField input;

    //update the name
    public void updateName(){
        if(!(input.text == "")){
            PlayerDataManager.UpdateName(input.text);
        }
    }
}
