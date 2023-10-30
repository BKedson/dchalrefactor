using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HighScores : MonoBehaviour
{

    //Add these to the end of the url to go to the website version of leaderboard (has commands there)
    const string privateCodeMaster = "fG1Hs0mcC02k9gIZnB3thQViDFw5hBHUi7V-S4HrZ_-g";  //Key to Upload New Total Scores and Average Times
    const string privateCodeAddition = "PS13fs9OxEWZXYXW_sYkpAbw1kf0P5xE2Z5PVBHhJDOA";  //Key to Upload New Average Addition Times/Scores
    const string privateCodeSubtraction = "VPo_e-xD006Syem3xpyPRQj2SD_2x3jk6NqUdP4iCWww";  //Key to Upload New Average Subtraction Times/Scores
    const string privateCodeMultiplication = "BAJCDDvqfUqtQZDC_y5KmgGWVtRDkAzkehSX3kURQcEQ";  //Key to Upload New Average Multiplication Times/Scores
    const string privateCodeDivision = "2FPMUBt2wkGg5LHs0o3_CQlDOD7pDh9UW1ggaZa2fHaQ";  //Key to Upload New Average Division Times/Scores

    const string publicCodeMaster = "642b18938f40bb109c047d68";   //Key to download Total Scores/Times
    const string publicCodeAddition = "64c402608f40bb8380e23d01";   //Key to download Addition Scores/Times
    const string publicCodeSubtraction = "64cfdd3e8f40bb8380f1c58c";   //Key to download Subtraction Scores/Times
    const string publicCodeMultiplication = "64cfdd818f40bb8380f1c5de";   //Key to download Multiplication Scores/Times
    const string publicCodeDivision = "64cfddb48f40bb8380f1c619";   //Key to download Division Scores/Times

    const string webURL = "http://dreamlo.com/lb/"; //  Website the keys are for

    public PlayerScore[] scoreList;
    DisplayHighscores myDisplay;
    public int activeWing;

    static HighScores instance; //Required for STATIC usability
    void Awake()
    {
        instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscores>();
    }
    
    public static void UploadScore(string username, int score, float time, int wing)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        instance.StartCoroutine(instance.DatabaseUpload(username, score, time, wing)); //Calls Instance
    }


    IEnumerator DatabaseUpload(string userame, int score, float time, int wing) //Called when sending new score to Website
    {
        if(wing == 1){
            WWW www = new WWW(webURL + privateCodeAddition + "/add/" + WWW.EscapeURL(userame) + "/" + score + "/" + (int)Math.Round(time*100));
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                Debug.Log(score);
                //DownloadScores(1);
            }
            else print("Error uploading" + www.error);
        }

        else if(wing == 2){
            WWW www = new WWW(webURL + privateCodeSubtraction + "/add/" + WWW.EscapeURL(userame) + "/" + score + "/" + (int)Math.Round(time*100));
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                Debug.Log(score);
                //DownloadScores(2);
            }
            else print("Error uploading" + www.error);
        }

        else if(wing == 3){
            WWW www = new WWW(webURL + privateCodeMultiplication + "/add/" + WWW.EscapeURL(userame) + "/" + score + "/" + (int)Math.Round(time*100));
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                //DownloadScores(3);
            }
            else print("Error uploading" + www.error);
        }

        else if(wing == 4){
            WWW www = new WWW(webURL + privateCodeDivision + "/add/" + WWW.EscapeURL(userame) + "/" + score + "/" + (int)Math.Round(time*100));
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                //DownloadScores(4);
            }
            else print("Error uploading" + www.error);
        }

        else{
            WWW www = new WWW(webURL + privateCodeMaster + "/add/" + WWW.EscapeURL(userame) + "/" + score + "/" + (int)Math.Round(time*100));
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload Successful");
                Debug.Log(score);
                //DownloadScores(0);
            }
            else print("Error uploading" + www.error);
        }
    }

    public void DownloadScores(int? wingNum){
        StartCoroutine("DatabaseDownload", wingNum);
    }
    IEnumerator DatabaseDownload(int? wingNum)
    {
        if(wingNum == 1){
            activeWing = 1;
            WWW www = new WWW(webURL + publicCodeAddition + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                myDisplay.SetScoresToMenu(scoreList);
            }
            else print("Error uploading" + www.error);
        }
        else if(wingNum == 2){
            activeWing = 2;
            WWW www = new WWW(webURL + publicCodeSubtraction + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                myDisplay.SetScoresToMenu(scoreList);
            }
            else print("Error uploading" + www.error);
        }
        else if(wingNum == 3){
            activeWing = 3;
            WWW www = new WWW(webURL + publicCodeMultiplication + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                myDisplay.SetScoresToMenu(scoreList);
            }
            else print("Error uploading" + www.error);
        }
        else if(wingNum == 4){
            activeWing = 4;
            WWW www = new WWW(webURL + publicCodeDivision + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                myDisplay.SetScoresToMenu(scoreList);
            }
            else print("Error uploading" + www.error);
        }
        else{
            activeWing = 0;
            //WWW www = new WWW(webURL + publicCodeMaster + "/pipe/"); //Gets the whole list
            WWW www = new WWW(webURL + publicCodeMaster + "/pipe/0/10"); //Gets top 10
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                OrganizeInfo(www.text);
                myDisplay.SetScoresToMenu(scoreList);
            }
            else print("Error uploading" + www.error);
        }
    }

    void OrganizeInfo(string rawData) //Divides Scoreboard info by new lines
    {
        string[] entries = rawData.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        scoreList = new PlayerScore[entries.Length];
        for (int i = 0; i < entries.Length; i ++) //For each entry in the string array
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            float time = float.Parse(entryInfo[2]) / 100;
            scoreList[i] = new PlayerScore(username,score, time);
            print(scoreList[i].username + ": " + scoreList[i].score + " In " + scoreList[i].time + "Seconds");
        }
    }
}

public struct PlayerScore //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;
    public float time;

    public PlayerScore(string _username, int _score, float _time)
    {
        username = _username;
        score = _score;
        time = _time;
    }
}