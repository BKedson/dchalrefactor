using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public static class PlayerData
{
    //this class stores the data related to the player during gameplay

    //stores the player fields
    private static string name = "PLAYER";
    //stores the player ID
    private static string playerID;
    //stores the current wing sessions
    private static DoorEncounters currentWingSession;
    //stores the entire list of all the wing sessions that this player has had while logged into the game
    private static List<DoorEncounters> wingSessions = new List<DoorEncounters>();  // Initialized the list to avoid null reference

    //getters 
    public static string GetName(){
        //return the name
        return name;
    }

    public static string GetID(){
        //return the playerID
        return playerID;
    }

    public static DoorEncounters GetCurrentWingSession(){
        //return the current Wing session
        return currentWingSession;
    }

    public static List<DoorEncounters> GetWingSessions(){
        //return the wingSessions
        return wingSessions;
    }

    //Updaters
    public static void UpdateName(string newName){
        //set the name
        name = newName;
    }

    public static void UpdateID(string newPlayerID){
        //set the ID
        playerID = newPlayerID;
    }

    public static void UpdateCurrentWingSession(DoorEncounters newCurrentWingSession){
        //set the current session
        currentWingSession = newCurrentWingSession;
    }

    public static void AddWingSession(DoorEncounters wingSession){
        //set the current session
        wingSessions.Add(wingSession);
    }
}