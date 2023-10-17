using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenario{
    //this class stores information relating to a specific Question answering scenario at a door in any of the wings. 

    //stores the number of attempts before the question on the door was solved
    public int numAttempts;
    //stores the time taken from when the question was revealed to when the question was solved
    public float timeTaken;
    //stores the operands in the question
    public int firstOperand;
    public int secondOperand;

    //parameterless constructor
    public Scenario(){}

    //Constructor
    public Scenario(int numAttempts, float timeTaken, int firstOperand, int secondOperand){
        //assignments
        this.numAttempts = numAttempts;
        this.timeTaken = timeTaken;
        this.firstOperand = firstOperand;
        this.secondOperand = secondOperand;
    }
}