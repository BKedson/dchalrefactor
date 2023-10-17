using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Victory_DefeatUI : MonoBehaviour
{
    //stores a reference to the canvas 
    public GameObject canvas;
    //stores whether we have overlapped with this trigger before
    public bool hasBeenOverlapped = false;
    //stores the number of points
    private int points;
    //stores the victory animator
    public Animator victoryAnim;
    //stores the defeat animator
    public Animator defeatAnim;
    //stores reference to the weapons as a trophy
    public GameObject sword;
    public GameObject controller;
    public GameObject laser;
    public int wing;
    //stores the Sound effects manager reference
    public SoundEffectsManager s_manager;

    private ReactiveTarget[] doors;

    // Start is called before the first frame update
    void Start()
    {
        //get the canvas
        canvas = GameObject.Find("Canvas");  
        //get the points
        points = PointsAndScoreController.Instance.doorPoints;  
        //get the animators
        victoryAnim = canvas.transform.Find("Victory").gameObject.GetComponent<Animator>();   
        defeatAnim = canvas.transform.Find("Death").gameObject.GetComponent<Animator>();
        doors = (ReactiveTarget[])GameObject.FindObjectsOfType(typeof(ReactiveTarget));
    }

    void Update(){
        //continuously get the points
        points = PointsAndScoreController.Instance.doorPoints;
    }

    // when this trigger is overlapped
    void OnTriggerEnter(Collider other){
        if(other.GetComponent<Collider>().tag == "Player" && !hasBeenOverlapped){
            //we must save the points and restart the pointer
            //int saveScore = PointsAndScoreController.Instance.doorPoints;
            //reset
            //PointsAndScoreController.Instance.ResetPoints();
            //Send the points to the PlayerDataManager

            //float avgTime = PlayerDataManager.getTime();
            //PlayerDataManager.UpdateScore(saveScore);

            //PlayerDataManager.uploadToDatabase(wing);
            //upload goes here

            //set the boolean
            //hasBeenOverlapped = true;
            //start the animation sequence depending on the number of points
            if(points >= 100){
                int saveScore = PointsAndScoreController.Instance.doorPoints; //use these
                float avgTime = PlayerDataManager.getTime();// use these

                
                PlayerDataManager.uploadToWingDatabase(wing);
                PlayerDataManager.uploadToDatabase();
                //PointsAndScoreController.Instance.ResetPoints();

                hasBeenOverlapped = true;
                //set the Object to active
                canvas.transform.Find("Victory").gameObject.SetActive(true);
                //start the victory animator
                victoryAnim.SetBool("startVictory", true);
                //give the player the key
                other.gameObject.GetComponent<PlayerCharacter>().hasDoorKey = true;
                //play the victory sound
                s_manager.play("VictorySound");
                //prompt the player to battle and return to base using PromptController
                GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("TimeForBattle");
                //set the hasFinished to true - level was a success!!
                other.gameObject.GetComponent<PlayerCharacter>().hasFinishedLevel = true;
            }
            else{

                foreach(ReactiveTarget d in doors){
                    AnswerUICollider doorUI = d.gameObject.GetComponentInChildren<AnswerUICollider>();
                    /*Animator doorClose = d.gameObject.GetComponentInChildren<Animator>();
                    doorClose.SetBool("openDoor", false);
                    doorClose.SetTrigger("closeDoor");*/
                    //openDoorAnim = doorReference.GetComponent<Animator>(); 
                    doorUI.doorReset();
                    Generator g = d.gameObject.GetComponent<Generator>();
                    g.generate();
                    if(d.gameObject.transform.parent == null){
                        d.gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                    }
                    else{
                        d.gameObject.transform.parent.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                    }
                }
                //set the Object to active
                canvas.transform.Find("Death").gameObject.SetActive(true);
                //points are less than 90
                defeatAnim.SetBool("startDeath", true);
                //play the defeat sound
                s_manager.play("DefeatSound");
                //prompt the player to try again using PromptController
                GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("TryAgain");
            }
        }
    }
}
