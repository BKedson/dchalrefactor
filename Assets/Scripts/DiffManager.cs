using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffManager : MonoBehaviour
{
    // this script is attached to the DifficultyPage UI element
    //stores the four difficulties
    public int addDiff;
    public int minusDiff;
    public int timesDiff;
    public int divideDiff;

    void Start(){
        //do not destroy this gameObject
        DontDestroyOnLoad(this.gameObject);
        //have default set to easy
        addDiff = 1;
        minusDiff = 1;
        timesDiff = 1;
        divideDiff = 1;
    }
    
    //returns an integer that represents the difficulty of the multiply operation
    public int getMult() {
        //returns the difficulty
        return timesDiff;
    }
    //returns an integer that represents the difficulty of the division operation
    public int getDiv() {
        //returns the divide difficulty
        return divideDiff;
    }
    //returns an integer that represents the difficulty of the addition operation
    public int getAdd() {
        //returns the add difficulty
        return addDiff;
    }
    //returns an integer that represents the difficulty of the subtraction operation
    public int getSub() {
        //returns the minus difficulty
        return minusDiff; 
    }    

    //setters
    //These methods are only called by a button press of the respective buttons and the buttons also call the DiffUIController.buttonIndicate() method
    public void setMult(int timesDifficulty){
        timesDiff = timesDifficulty;
    }
    public void setAdd(int addDifficulty){
        addDiff = addDifficulty;
    }
    public void setDiv(int divideDifficulty){
        divideDiff = divideDifficulty;
    }
    public void setSub(int minusDifficulty){
        minusDiff = minusDifficulty;
    }
}
