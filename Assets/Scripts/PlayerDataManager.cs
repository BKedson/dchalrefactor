using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerDataManager : object
{
    //stores the different player fields, name, score
    private static string name = "PLAYER";
    private static int score;
    private static int wingScore;
    private static List<float> times;
    private static List<float> wingTimes;

    static PlayerDataManager(){
        times = new List<float>();
        wingTimes = new List<float>();
    }

    //methods for updating and returning the fields
    public static void UpdateTime(float timeSegment){
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
}
