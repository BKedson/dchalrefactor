using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            OnPause();
        }
    }

    public void OnPause(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseCanvas.SetActive(true);
    }

    //Prevent bugs when quitting during assessment terminal challenge
    public void OnQuit(){
        AssessmentTerminalManager TermMan = GameObject.Find("Assessment Terminal").GetComponent<AssessmentTerminalManager>();
        if(TermMan){
            TermMan.QuitFromPauseMenu();
        }
    }

    public void OnPlay(){
        Time.timeScale = 1;
        AssessmentTerminalManager TermMan = GameObject.Find("Assessment Terminal").GetComponent<AssessmentTerminalManager>();
        if(TermMan){
            if(!TermMan.open){
                Cursor.lockState = CursorLockMode.Locked;
            }else{
                TermMan.ContinueFromPauseMenu();
            }
        }
        PauseCanvas.SetActive(false);
    }
}
