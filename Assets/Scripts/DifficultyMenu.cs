using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyMenu : MonoBehaviour
{
    public GameObject difficultyMenu;
    public void menuControl(bool open){
        if(open){
            difficultyMenu.SetActive(false);
            Time.timeScale = 1f;
            open = false;
        }
        else{
            difficultyMenu.SetActive(true);
            open = true;
            Time.timeScale = 0f;
        }
    }


}
