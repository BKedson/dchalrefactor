using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timeText;

    [Header("Time Settings")]
    public float currentTime;
    private bool countDown;
    public bool started;

    void OnEnable() {
        started = true;
    }

    void OnDisable(){
        started = false;
        PlayerDataManager.UpdateTime(currentTime);
        currentTime = 0;
    }
    
    void Update()
    {
        if(started){
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timeText.text = currentTime.ToString("0.0"); //stop on question answer and add to total wing time
        }
    }
}
