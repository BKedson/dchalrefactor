using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
            // GameManager.manager.ResetComplexity();
			GameManager.manager.Restart();
            SceneManager.LoadScene("Room Generation Test Scene");
            GameManager.manager.Save();
		} else {
            SceneManager.LoadScene("Room Generation Test Scene");
        }
    }

    public void NewGame() {
        if (GameManager.manager) {
            GameManager.manager.ResetComplexity();
			GameManager.manager.Restart();
            SceneManager.LoadScene("Room Generation Test Scene");
            GameManager.manager.Save();
		} else {
            SceneManager.LoadScene("Room Generation Test Scene");
        }
    }

    // Tells the GameManager to reset everything marked as DoNotDestroy, then resets the scene
    public void RestartLevel() {
        if (GameManager.manager) {
            Debug.Log(GameManager.manager.GetAddDifficulty());
            GameManager.manager.Restart();
            GameManager.manager.Save();
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

    public void Audio() {
        SceneManager.LoadScene("Audio");
    }

    public void Quit() {
        Application.Quit();
    }

    public void CharacterPage() {
        SceneManager.LoadScene("Character Selection");
    }
}
