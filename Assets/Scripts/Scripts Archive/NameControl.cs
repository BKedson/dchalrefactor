using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameControl : MonoBehaviour
{
    //stores a reference to the name Text element
    public TMP_Text nameDisplay;
    //stores the name as a string
    private string name;
    //stores the change UI element
    public GameObject changeUI;

    // Start is called before the first frame update
    void Start()
    {
        //initialize name to default
        name = "PLAYER";
        nameDisplay.text = name;      
    }

    // Update is called once per frame
    void Update()
    {
        //update the the changeName UI is disabled
        if(!(changeUI.active)){
            //continuously updates the text to be displayed
            name = PlayerDataManager.getName();
            nameDisplay.text = name;
        }    
    }

    //This method sets the name displayed
    public void setName(string name){
        //set the name field
        this.name = name;
    }
}
