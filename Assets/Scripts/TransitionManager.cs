using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private bool exitToMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Prefab in Scene"){
            if (TransitionUIManager._instance) {
                TransitionUIManager._instance.EndTransition();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame() {
        // if (PlayerPrefs.HasKey("CurrentLevel")) {
        //     SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
        // } else {
        //     PlayerPrefs.SetString("CurrentLevel", "Room Generation Test Scene");
        //     SceneManager.LoadScene("Room Generation Test Scene");
        // }

        // if (GameManager.manager) {
        //     GameManager.manager.Save();
        // }

        if (GameManager.manager) {
            GameManager.manager.ResetComplexity();
			GameManager.manager.Restart();
            GameManager.manager.ResetPlayerPos();
            SceneManager.LoadScene("Prefab in Scene");
            GameManager.manager.Save();
		} else {
            SceneManager.LoadScene("Prefab in Scene");
        }
    }

    public void NewGame() {
        if (GameManager.manager) {
            GameManager.manager.ResetComplexity();
			GameManager.manager.Restart();
            GameManager.manager.ResetPlayerPos();
            SceneManager.LoadScene("Prefab in Scene");
            GameManager.manager.Save();
		} else {
            SceneManager.LoadScene("Prefab in Scene");
        }
    }

    // Tells the GameManager to reset everything marked as DoNotDestroy, then resets the scene
    public void RestartLevel() {
        //prevent ondestroy method of enemies from restarting the scene when exiting to main menu
        if(!exitToMenu){
            if (TransitionUIManager._instance) {
                TransitionUIManager._instance.StartTransition();
            }
            StartCoroutine(RestartTransition());
        }
    }

    public IEnumerator RestartTransition(){
        yield return new WaitForSeconds(TransitionUIManager._instance.GetStartTransitionSpan());
        if (GameManager.manager) {
            GameManager.manager.Restart();
            GameManager.manager.Save();
            GameManager.manager.ResetPlayerPos();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Options() {
        SceneManager.LoadScene("Options");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Menu() {
        exitToMenu = true;
        SceneManager.LoadScene("Main Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (GameManager.manager) {
            GameManager.manager.DisablePlayerSound();
            GameManager.manager.Save();
        }
    }

    public void Controls() {
        SceneManager.LoadScene("Controls");
    }

    public void Difficulty() {
        SceneManager.LoadScene("Difficulty");
    }

    public void Settings() {
        SceneManager.LoadScene("Settings");
    }

    public void Operands() {
        SceneManager.LoadScene("Operands");
    }

    public void Quit() {
        Application.Quit();
    }

    public void CharacterPage() {
        SceneManager.LoadScene("Character Selection");
    }

    public void ArrivedAtMenu(){
        exitToMenu = false;
    }
}
