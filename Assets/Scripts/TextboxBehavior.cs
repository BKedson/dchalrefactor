using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using TMPro;

//This script can be attatched to the textbox gameobject, but 
//then it can't reactivate itself to loop text.
//if looping behavior is desired, attatch this script to a
//different gameobject instead
public class TextboxBehavior : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private string[] messages;

    [SerializeField] private GameObject textBoxContainer;
    private int numMessages;
    private int currMessage;

    void Start()
    {
        numMessages = messages.Length;
        currMessage = 0;
        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
    }

    void Update()
    {
        if (Input.GetKeyDown("t")){
            if(!textBoxContainer.activeSelf){
                textBoxContainer.SetActive(true);
            }
            if(currMessage >= numMessages-1){
                textBoxContainer.SetActive(false);
                currMessage=-1;
            }
            currMessage++;
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }
        
    }
}
