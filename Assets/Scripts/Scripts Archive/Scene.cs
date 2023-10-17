using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Scene : MonoBehaviour
{
    public void LoadScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void Quit(){
        if(!(SceneManager.GetActiveScene().name == "Start")){
            Time.timeScale = 1;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        else{
            Application.Quit();
        }
    }
}
