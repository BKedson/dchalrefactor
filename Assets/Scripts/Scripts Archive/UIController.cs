using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject canvas;
    public static GameObject mainCam;
    public static GameObject player;
    public static bool isPaused = false;
    //Static variable that is set on Update by the AnswerUICollider
    public static bool AnswerTriggerFire;
    public static bool resumeCalled;
    public GameObject uiOnlyForGameScene;
    public GameObject uiNotForGameScene;
    
    void Start(){
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        resumeCalled = false;
        AnswerTriggerFire = false;
        mainCam = GameObject.Find("Main Camera");
    }
    void Update(){
        //CODE THAT GOVERNS THE ANSWER UI
        //entering the collider
        if(AnswerTriggerFire){
            if(!isPaused){
                Pause(false);
                AnswerTriggerFire = false;
            }
        }
        //exiting the collider
        if(resumeCalled){
            Resume();
        }

        //CODE THAT GOVERNS THE PAUSE MENU
        if(Input.GetKeyDown("escape")){
            //run the Quit UI
            Pause(true);
            uiOnlyForGameScene.SetActive(PointsAndScoreController.Instance.inGameScene);
            uiNotForGameScene.SetActive(!PointsAndScoreController.Instance.inGameScene);
        }
    }

    public void Pause(bool isQuit)
    {
        isPaused = true;
        

        if(!isQuit){
            //AnswerUI
            canvas.transform.Find("Answer UI").gameObject.SetActive(true);
            //Activate the input field
            // we also do this when a wrong answer has been put in -so that the field is activated again ( AnswerUICollider.wrongAnswer() )
            canvas.transform.Find("Answer UI").Find("MyInputField").GetComponent<TMP_InputField>().ActivateInputField(); 
            //slow down the mouse movement while answering
            player.GetComponent<MouseLook>().SetSensitivities(0.5f, 0.5f);
        }

        else{
            //QuitUI
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canvas.transform.Find("Quit UI").gameObject.SetActive(true);
            //pause functionality
            Time.timeScale = 0f;
            player.GetComponent<MouseLook>().enabled = false;
            mainCam.GetComponent<RayShooter>().enabled = false;
            //gameObject.GetComponent<FPSInput>().enabled = false;
            gameObject.GetComponent<MouseLook>().enabled = false;
        }

    }


    public void Resume()
    {
        //stores whether any other interative UI elements are active to be used later
        bool interactiveElements = checkPauseAndInteractive();

        //only remove the Answer UI if the Options menu is not active - that would mean the resume button doesn't remove the AnswerUI
        if(!canvas.transform.Find("Quit UI").gameObject.activeSelf){
            //if not active - remove the answerUi because we are not pressing the Options Resume button
            canvas.transform.Find("Answer UI").gameObject.SetActive(false);
        }

        //remove the Quit UI
        canvas.transform.Find("Quit UI").gameObject.SetActive(false);
        resumeCalled = false;
        isPaused = false;
        //cursor removing code and Resuming code
        //we only want to remove the cursor if the non-interactive UI is active 
        //If the game is paused with interactive UI in the background, we shall lose ability to interact with the UI if cursor is disabled
        //so we should NOT set cursor visibility off if the PauseMenu and any Interactive UI are both active
        if(!interactiveElements){
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<MouseLook>().enabled = true;
            mainCam.GetComponent<RayShooter>().enabled = true;
            gameObject.GetComponent<FPSInput>().enabled = true;
            gameObject.GetComponent<MouseLook>().enabled = true;
            
            //restore the mouse movement
            player.GetComponent<MouseLook>().SetSensitivities(5.0f, 2.0f);  
        }
        
        
        //Re-activate the input field
        canvas.transform.Find("Answer UI").Find("MyInputField").GetComponent<TMP_InputField>().ActivateInputField();
    }

    //method to check if background UI is interactive while game is paused
    public bool checkPauseAndInteractive(){
        //LIST OF INTERACTIVE UI ELEMENTS
        //stores whether option menu is active
        bool quitActive = canvas.transform.Find("Quit UI").gameObject.activeSelf;
        //stores whether Find teleporter UI is active
        bool teleActive = canvas.transform.Find("FindTheTeleporter").gameObject.activeSelf;
        //stores whether Welcome UI is active
        bool welcomeActive = canvas.transform.Find("WelcomeUI").gameObject.activeSelf;
        //stores whether gunUI is active
        bool gunActive = canvas.transform.Find("gunUI").gameObject.activeSelf;
        //stores whether swordUI is active
        bool swordActive = canvas.transform.Find("swordUI").gameObject.activeSelf;
        //stores whether controllerUI is active
        bool controlActive = canvas.transform.Find("controllerUI").gameObject.activeSelf;

        //if any of these are true and the quitUI is active - that means there is a background pause state which must be maintained therefore return true
        if ((quitActive && teleActive) || (quitActive && welcomeActive) || (quitActive && gunActive) || (quitActive && swordActive) || (quitActive && controlActive) ){
            return true;
        }
        else{
            return false;
        }
    }
}
