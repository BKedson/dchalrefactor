using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            OnPause();
        }
    }

    public void OnPause(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        PauseCanvas.SetActive(true);
    }

    public void OnPlay(){
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        PauseCanvas.SetActive(false);
    }
}
