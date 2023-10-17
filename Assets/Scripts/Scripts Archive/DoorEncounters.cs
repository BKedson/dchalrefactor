using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorEncounters
{
    //This class stores information relating to a wing of operations that the player encounters, when they happened and the list of Scenarios that the player encountered
    
    //stores the difficulty
    public int difficulty;
    //stores the type of operation
    public string operation;
    //stores the date when this set of Encounters took place
    public string date;
    //stores a list of the scenarios associated with these DoorEncounters
    public List<Scenario> scenarios = new List<Scenario>();

    //stores the sessionID of a list of Door Encounters
    public string sessionID;

    //constructor - creates a new set of DoorEncounters

    // Parameterless constructor for XML Serialization
    public DoorEncounters() {}

    // Constructor with parameters
    public DoorEncounters(string operation, int difficulty, string date)
    {
        //assignments
        this.difficulty = difficulty;
        this.operation = operation;
        this.date = date;

        //initialize the scenarios
        scenarios = new List<Scenario>();
    }

    //METHODS
    //adds a new Scenario to the list of Scenarios
    public void addNewScenario(Scenario toAdd)
    {
        //add new scenario to the list of scenarios
        scenarios.Add(toAdd);
    }
}