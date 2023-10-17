using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    //stores references to all the corresponding UI element texts
    public GameObject get90Points;
    public GameObject findTheTeleporters;
    public GameObject timeForBattle;
    public GameObject tryAgain;
    public GameObject unlockedDoors;
    public GameObject enemyCountPrompt;
    public GameObject victory;


    //method Prompt that sets the appropriate prompt name active on the TaskBorder at the top of the page
    public void promptUI(string task){
        //Different prompts for the different tasks
        //if prompt is x, set all others to inactive
        if(task == "Get90Points"){
            //set true
            get90Points.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            timeForBattle.SetActive(false);
            tryAgain.SetActive(false);
            unlockedDoors.SetActive(false);
            enemyCountPrompt.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "FindTheTeleporters"){
            //set true
            findTheTeleporters.SetActive(true);
            //set the rest to false
            get90Points.SetActive(false);
            timeForBattle.SetActive(false);
            tryAgain.SetActive(false);
            unlockedDoors.SetActive(false);
            enemyCountPrompt.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "TimeForBattle"){
            //set true
            timeForBattle.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            get90Points.SetActive(false);
            tryAgain.SetActive(false);
            unlockedDoors.SetActive(false);
            enemyCountPrompt.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "TryAgain"){
            //set true
            tryAgain.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            timeForBattle.SetActive(false);
            get90Points.SetActive(false);
            unlockedDoors.SetActive(false);
            enemyCountPrompt.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "UnlockedDoors"){
            //set true
            unlockedDoors.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            timeForBattle.SetActive(false);
            tryAgain.SetActive(false);
            get90Points.SetActive(false);
            enemyCountPrompt.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "EnemyCountPrompt"){
            //set true
            enemyCountPrompt.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            timeForBattle.SetActive(false);
            tryAgain.SetActive(false);
            unlockedDoors.SetActive(false);
            get90Points.SetActive(false);
            victory.SetActive(false);
        }
        //if prompt is x, set all others to inactive
        if(task == "Victory"){
            //set true
            victory.SetActive(true);
            //set the rest to false
            findTheTeleporters.SetActive(false);
            timeForBattle.SetActive(false);
            tryAgain.SetActive(false);
            unlockedDoors.SetActive(false);
            get90Points.SetActive(false);
            enemyCountPrompt.SetActive(false);
        }
    }
}
