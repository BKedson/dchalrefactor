using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Audio;

public class AnswerUICollider : MonoBehaviour
{
    public GameObject mainCanvas; 
    //stores the canvas - The canvas is obtained during gameplay
    public GameObject playerAnswer;
    //stores the integers to be tested
    public int firstNumberCheck;
    public int secondNumberCheck;
    //stores the sign to be checked
    public string operatorCheck;
    //stores the input
    public int answerInput;
    //stores the Animator
    Animator openDoorAnim;
    //stores a reference to the door of this collider
    public GameObject doorReference;
    //stores a reference to the audioManager
    public SoundEffectsManager manager;
    //stores a reference to the indicator lights
    public GameObject indicator1;
    public GameObject indicator2;

    //stores references to the doorUI elements
    public GameObject operand1Door;
    public GameObject operand2Door;
    public GameObject operatorDoor;
    public GameObject warning;
    public GameObject padlockRed;
    public GameObject padlockGreen;
    public GameObject MapPadlockRed;
    public GameObject MapPadlockGreen;
    public GameObject questionMark; //time Update
    //stores reference to the UI question holder and the corresponding elements on the canvas
    public GameObject questionHolder;

    public bool hasBeenOverlapped = false;
    public bool doorOpen = false;

    void Start(){
        openDoorAnim = doorReference.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        //make the question appear and remove the QuestionMarkUI
        operand1Door.SetActive(true);
        operand2Door.SetActive(true);
        operatorDoor.SetActive(true);
        questionMark.SetActive(false);
        //start the timer

        //we want the question to be updated

        //get the canvas to obtain the answer and update the question
        playerAnswer = GameObject.Find("Canvas");
        //get reference to the UI elements

        //
        padlockRed = playerAnswer.transform.Find("Answer UI").GetChild(3).gameObject;
        padlockGreen = playerAnswer.transform.Find("Answer UI").GetChild(2).gameObject;

        //get the element that represent the question holder.
        questionHolder = playerAnswer.transform.Find("Answer UI").GetChild(0).gameObject;
        //update the respective elements
        questionHolder.transform.Find("Q_Operand1").gameObject.GetComponent<TMP_Text>().text = firstNumberCheck.ToString();
        questionHolder.transform.Find("Q_Operand2").gameObject.GetComponent<TMP_Text>().text = secondNumberCheck.ToString();
        questionHolder.transform.Find("Q_Operation").gameObject.GetComponent<TMP_Text>().text = operatorCheck;

        if (other.GetComponent<Collider>().tag == "Player" && !hasBeenOverlapped && !doorOpen)
        {
            Debug.Log("AHHHH!!!");
            //reset the padlock UI
            padlockRed.SetActive(true);
            padlockGreen.SetActive(false);
            //UIController is a class that controls the UI of the game
            //Set AnswerTriggerFire of the UI controller class to true
            UIController.AnswerTriggerFire = true;
            hasBeenOverlapped = true;
            //fire the answer UI sound
            manager.play("AnswerPopup");
        }
    }

    public void doorReset(){
        openDoorAnim.SetBool("openDoor", false);
        openDoorAnim.SetTrigger("closeDoor");
        MapPadlockGreen.SetActive(false);
        MapPadlockRed.SetActive(true);
        warning.SetActive(true);
        hasBeenOverlapped = false;
        indicator1.GetComponent<ColorChange>().changeLight("red");
        indicator2.GetComponent<ColorChange>().changeLight("red");
        questionMark.SetActive(true);
        doorOpen = false;
        //UIController.AnswerTriggerFire = false;
    }

    void OnTriggerStay(Collider other)
    {

        //wait for the player to press the enter key
        //if they press enter
        if ( (Input.GetKeyDown("return") || Input.GetKeyDown("enter")) && warning.activeSelf)
        {
            //get the input answer
            if (playerAnswer.transform.Find("Answer UI").GetChild(1).GetComponent<TMP_InputField>().text.Any(char.IsDigit))
            {
                answerInput = int.Parse(playerAnswer.transform.Find("Answer UI").GetChild(1).GetComponent<TMP_InputField>().text);
            }
            //clear the answer
            playerAnswer.transform.Find("Answer UI").GetChild(1).GetComponent<TMP_InputField>().text = "";
            //check wrong or right
            //if answers are same
            if (answerCheck(answerInput))
            {
                //answer is correct
                correctAnswer(); //run the correct answer code
            }

            else
            {
                wrongAnswer(); //run the wrong answer code
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Collider>().tag == "Player" && hasBeenOverlapped)
        {
            Debug.Log("BYE!!!");
            //UIController is a class that controls the UI of the game
            //Resume(): UIController: remove the Answer UI 
            UIController.resumeCalled = true;
            //if the door is still closed: there was no work done
            if (!doorOpen)
            {
                //hasBeenOverlapped back to false
                //doorReset();
                hasBeenOverlapped = false;
            }
            else
            {
                //door is Open so we have completed a successful overlap. Work was done! Future overlaps won't trigger the answer UI.
                //doorReset(); 
                hasBeenOverlapped = true;
            }
        }
    }

    //method called to have an answer on standby
    public bool answerCheck(int playerGuess)
    {
        //if the sign is addition
        if (operatorCheck == "+")
        {
            if (playerGuess == (firstNumberCheck + secondNumberCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //if the sign is minus
        else if (operatorCheck == "-")
        {
            if (playerGuess == (firstNumberCheck - secondNumberCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //if the sign is multiplication
        else if (operatorCheck == "x")
        {
            if (playerGuess == (secondNumberCheck * firstNumberCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //if the sign is division
        else
        {
            if (playerGuess == (firstNumberCheck / secondNumberCheck))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //called for a correct answer
    public void correctAnswer()
    {
        //change the Minimap indicators
        MapPadlockGreen.SetActive(true);
        MapPadlockRed.SetActive(false);
        //destroy the Door UI Elements by setting active to false
        operand1Door.SetActive(false);
        operand2Door.SetActive(false);
        operatorDoor.SetActive(false);
        warning.SetActive(false);

        //play the openDoor sound: the sound for doorClosing is used for both opening and closing scenarios
        manager.play("DoorClosing");
        //play the correctAnswer sound
        manager.play("CorrectAnswer");
        //turn the indicator lights green - Using GameObject.Indicator.ColorChange.changeLight();
        indicator1.GetComponent<ColorChange>().changeLight("green");
        indicator2.GetComponent<ColorChange>().changeLight("green");
        //add 3 points to the indicator
        PointsAndScoreController.Instance.updateDoorPoints(7);

        //delay a second for the padlock animation
        StartCoroutine(padlockOpen());
    }

    //called for a wrong answer
    public void wrongAnswer()
    {
        //play the Wrong Answer sound
        manager.play("WrongAnswer");
        //turn the indicator lights red - Using GameObject.Indicator.ColorChange.changeLight();
        indicator1.GetComponent<ColorChange>().changeLight("red");
        indicator2.GetComponent<ColorChange>().changeLight("red");
        //we also want to reset the activation of the input Text field
        playerAnswer.transform.Find("Answer UI").Find("MyInputField").GetComponent<TMP_InputField>().ActivateInputField();
        //deduct 9 points from the indicator
        PointsAndScoreController.Instance.updateDoorPoints(-9);
    }

    //Padlock Opening Animation
    IEnumerator padlockOpen()
    {
        //set the padlock green and wait half a second
        //playerAnswer is the reference to the canvas, canvas contains the green and red padlock as Children 1 and 2 and AnswerUI
        //playerAnswer.transform.Find("Answer UI").GetChild(2).SetActive(false); //red
        padlockRed.SetActive(false);
        //playerAnswer.transform.Find("Answer UI").GetChild(1).SetActive(true); //green
        padlockGreen.SetActive(true);
        yield return new WaitForSeconds(0.125f);
        //after a half second - we can remove the UI - remember to reset the padlocks

        //remove UI
        //Resume(): UIController: remove the Answer UI
        UIController.resumeCalled = true;
        //open the Door actions
        openDoorAnim.ResetTrigger("closeDoor");
        openDoorAnim.SetBool("openDoor", true);
        //set door open to true
        doorOpen = true;
    }
}
