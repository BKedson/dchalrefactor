using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshScores : MonoBehaviour
{
    public int wingNum;

    //send scores
    public void send(){
        transform.parent.GetComponent<HighScores>().DownloadScores(wingNum);
    }
}
