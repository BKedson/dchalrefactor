using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System;
using System.Globalization;

public static class PlayerDataManager : object
{
    //stores the different player fields, name, score
    private static string name = "PLAYER";
    private static int score;
    private static int wingScore;
    private static List<float> times;
    private static List<float> wingTimes;

    //stores the last recorded time from the last math operation
    public static float prevTime = 0.0f;

    static PlayerDataManager(){
        times = new List<float>();
        wingTimes = new List<float>();
    }

    //methods for updating and returning the fields
    public static void UpdateTime(float timeSegment){
        //update last time
        prevTime = timeSegment;
        //adjust avg times
        times.Add(timeSegment);
        wingTimes.Add(timeSegment);
    }
    public static void UpdateName(string nameInput){
        //update the name
        name = nameInput;
    }
    public static void UpdateScore(int scoreInput){
        //update the wing score
        score = scoreInput;
    }

    public static void UpdateWingScore(int scoreInput){
        //update the wing score
        wingScore = scoreInput;
    }

    public static void resetTime(){
        wingTimes.Clear();
    }

    public static float getTime(){
        return times.Sum()/times.Count;
    }

    public static float getWingTime(){
        return wingTimes.Sum()/wingTimes.Count;
    }

    public static string getName(){
        //return the name
        return name;
    }

    public static int getScore(){
        //return the score
        return score;
    }
    public static int getWingScore(){
        //return the score
        return wingScore;
    }

    public static void uploadToDatabase(){
        HighScores.UploadScore(name, score * ((int)(10/getTime())), getTime(), 0);
    }

    public static void uploadToWingDatabase(int wingNum){
        HighScores.UploadScore(name, wingScore * ((int)(10/getWingTime())), getWingTime(), wingNum);
        resetTime();
        UpdateWingScore(0);
    }

    //creates a new WingSession and makes it the current wing session
    public static void CreateNewWingSession(string operation)
    {
        DiffManager diffManager = GameObject.Find("DifficultyManager").GetComponent<DiffManager>();
        if (diffManager == null) 
        {
            Debug.LogError("DifficultyManager not found!");
            return;
        }

        Func<int> difficultyFunc = null;

        switch (operation)
        {
            case "addition":
                difficultyFunc = diffManager.getAdd;
                break;
            case "subtraction":
                difficultyFunc = diffManager.getSub;
                break;
            case "multiplication":
                difficultyFunc = diffManager.getMult;
                break;
            case "division":
                difficultyFunc = diffManager.getDiv;
                break;
            default:
                Debug.LogError($"Unknown operation: {operation}");
                return;
        }

        PlayerData.UpdateCurrentWingSession(new DoorEncounters(operation, difficultyFunc(), DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
    }


    public static void Save()
    {
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "PlayerAcademicProgress_data.xml");
        XDocument xDoc = XDocument.Load(path);

        XElement playerElement = xDoc.Element("Player");
        if (playerElement != null)
        {
            playerElement.SetAttributeValue("name", PlayerData.GetName());
            playerElement.SetAttributeValue("PlayerID", PlayerData.GetID());

            foreach (var doorEncounter in PlayerData.GetWingSessions())
            {
                XElement difficultyElement = playerElement.Elements("Difficulty")
                                            .FirstOrDefault(d => d.Attribute("level")?.Value == doorEncounter.difficulty.ToString());

                if (difficultyElement != null)
                {
                    XElement wingElement = difficultyElement.Elements("Wing")
                                            .FirstOrDefault(w => w.Attribute("operation")?.Value == doorEncounter.operation);

                    if (wingElement != null)
                    {
                        // Check if the DoorEncounters with the sessionID already exists.
                        if (string.IsNullOrEmpty(doorEncounter.sessionID) ||
                            wingElement.Elements("DoorEncounters").FirstOrDefault(de => de.Attribute("sessionID")?.Value == doorEncounter.sessionID) == null)
                        {
                            // This means this DoorEncounters hasn't been added to the XML yet.
                            string lastSessionID = wingElement.Attribute("lastSessionID")?.Value;
                            int lastSessionNumber = int.Parse(lastSessionID.Split('-')[1]);
                            int nextSessionNumber = lastSessionNumber + 1;
                            string nextSessionID = $"{doorEncounter.operation[0]}{doorEncounter.difficulty}-{nextSessionNumber}";

                            XElement newDoorEncounterElement = new XElement("DoorEncounters");
                            newDoorEncounterElement.SetAttributeValue("sessionID", nextSessionID);
                            newDoorEncounterElement.SetAttributeValue("date", doorEncounter.date);
                            
                            // Assigning session ID to DoorEncounters object
                            doorEncounter.sessionID = nextSessionID;

                            foreach (var scenario in doorEncounter.scenarios)
                            {
                                XElement scenarioElement = new XElement("Scenario");
                                scenarioElement.Add(new XElement("Attempts", scenario.numAttempts));
                                scenarioElement.Add(new XElement("TimeTaken", scenario.timeTaken));

                                // Storing the operands as a single string under "operands"
                                string operandsString = $"{scenario.firstOperand} : {scenario.secondOperand}";
                                scenarioElement.Add(new XElement("operands", operandsString));

                                newDoorEncounterElement.Add(scenarioElement);
                            }

                            wingElement.Add(newDoorEncounterElement);
                            wingElement.SetAttributeValue("lastSessionID", nextSessionID);
                        }
                    }
                }
            }
        }

        xDoc.Save(path);
    }

}