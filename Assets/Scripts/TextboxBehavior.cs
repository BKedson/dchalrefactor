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
//  --> 6 is how to pick up ores                4: ++ on place ore
//  --> 7-9 describe intake question            5: ++ on correct intake
//  --> 10 is taking the sword                  6: ++ on sword equipped
//  --> 11 is how to attack                     7: ++ on attack (twice to be sure its not an accidental click?)
//  --> 12 is avoid getting hurt                8: ++ after several attacks
//  --> 13 describes pause and tutorial         9: stuck here, deactivate textbox but set back to 0 and reactivate when player presses t

//  *** t key always advances text, but only within the current section of the array ***

public class TextboxBehavior : MonoBehaviour
{
    [SerializeField] private string[] messages;
    private int numMessages;
    private int currMessage;

    private int section;
    private int clickCount;

    [SerializeField] private GameObject text;
    [SerializeField] private GameObject textBoxContainer;
    

    void Awake()
    {
        numMessages = messages.Length;
        currMessage = 0;
        section = 0;
        clickCount = 0;

        // text = GameObject.Find("TutorialText");
        // textBoxContainer = GameObject.Find("TutorialTextBox");
        if (text) {
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("t")){
            //if a section has several messages, allow looping text through that section
            if(currMessage == 0 || currMessage == 2 || currMessage == 4 || currMessage == 7 || currMessage == 8){
                currMessage++;
            }else if(currMessage == 3){
                currMessage = 2;
            }else if(currMessage == 5){
                currMessage = 4;
            }else if(currMessage == 9){
                currMessage = 7;
            }
            
            //if tutorial has ended, reactivate it
            if(!textBoxContainer.activeSelf){
                section = 0;
                currMessage = 0;
                clickCount = 0;
                textBoxContainer.SetActive(true);
            }

            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }

        if(textBoxContainer.activeSelf){
            if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d")){
                PlayerMoved();
            }

            if(Input.GetKeyDown("mouse 0")){
                if(section >= 7){
                    clickCount++;
                    if(clickCount > 2){
                        CombatOver();
                    }else{
                        OnAttack();
                    }
                }
            }
        }
        
        
    }

    public void PlayerMoved(){
        if(section < 2){
            section = 2;
            currMessage = 2;
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }
    }

    public void TerminalOpened(){
        if(textBoxContainer && textBoxContainer.activeSelf){
            if(section < 3){
                section = 3;
                currMessage = 4;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            }
        }
        
    }

    public void TerminalCorrectlySubmitted(){
        if(textBoxContainer.activeSelf){
            if(section < 4){
                section = 4;
                currMessage = 6;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            }
        }
        
    }

    public void OrePlaced(){
        if(textBoxContainer.activeSelf){
            if(section < 5){
                section = 5;
                currMessage = 7;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            }
        }
        
    }

    public void IntakeCorrectlySubmitted(){
        if(textBoxContainer.activeSelf){
            if(section < 6){
                section = 6;
                currMessage = 10;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            }
        }
    }

    public void SwordEquipped(){
        if(textBoxContainer.activeSelf){
            if(section < 7){
                section = 7;
                currMessage = 11;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            }
        }
    }

    public void OnAttack(){
        section = 8;
        currMessage = 12;
        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
    }

    public void CombatOver(){
        section = 9;
        currMessage = 13;
        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        StartCoroutine(WaitToDeactivate());
    }

    private IEnumerator WaitToDeactivate(){
        yield return new WaitForSeconds(5.0f);
        
        //check that the tutorial hasn't already restarted 
        if(section == 9){
            textBoxContainer.SetActive(false);
        }
    }
}
