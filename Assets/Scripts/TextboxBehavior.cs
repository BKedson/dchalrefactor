using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using TMPro;

//This script can be attatched to the textbox gameobject, but 
//then it can't reactivate itself to loop text.
//if looping behavior is desired, attatch this script to a
//different gameobject instead

// Message array section markers:        section#: Condtion for moving on
//  --> 0 is welcome                            0: ++ on player press t key or after a short time?
//  --> 1 is movement                           1: ++ when player moves
//  --> 2-3 describe terminal                   2: ++ when terminal opens
//  --> 4-5 describe typing answer              3: ++ on terminal submit
//  --> 6 is how to pick up ores                4: ++ on pick up ore/place ore
//  --> 7-9 describe intake question            5: ++ on correct intake
//  --> 10 is taking the sword                  6: ++ on sword equipped
//  --> 11 is how to attack                     7: ++ on attack (twice to be sure its not an accidental click?)
//  --> 12 is avoid getting hurt                8: ++ on kill all enemies
//  --> 13 describes pause and tutorial         9: stuck here, deactivate textbox but set back to 0 and reactivate when player presses t

//  *** t key always advances text, but only within the current section of the array

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
