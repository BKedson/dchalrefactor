using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//class to control all the math related to scores and points.
public class PointsAndScoreController : MonoBehaviour
{
    public static PointsAndScoreController Instance { get; private set; }

    private TMP_Text scoreBoard;

    //a list of variables that store different points related to different points in the game
    public int enemyPoints;
    public int doorPoints;

    public bool inGameScene = false;
    public int currentWingNum = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        scoreBoard = GameObject.Find("Canvas").transform.Find("CounterBorder").GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        //enemies killed are zero at beginning and updated when the player starts defeating enemies
        enemyPoints = 0;
    }

    //incrementing functions
    public void incrementEnemyPoints()
    {
        enemyPoints++;
    }
    public void resetEnemyPoints()
    {
        enemyPoints = 0;
    }

    public void updateDoorPoints(int points)
    {
        doorPoints = Mathf.Max(doorPoints + points, 0);
        scoreBoard.text = doorPoints.ToString();
        PlayerDataManager.UpdateScore(points + PlayerDataManager.getScore());
        PlayerDataManager.UpdateWingScore(points + PlayerDataManager.getWingScore());
    }

    public void ResetPoints()
    {
        doorPoints = 0;
        scoreBoard.text = "00";
    }
}
