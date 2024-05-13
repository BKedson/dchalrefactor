using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame() {
        if (PlayerPrefs.HasKey("CurrentLevel")) {
            SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
        } else {
            PlayerPrefs.SetString("CurrentLevel", "Room Generation Test Scene");
            SceneManager.LoadScene("Room Generation Test Scene");
        }
    }

    public void NewGame() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameManager.Restart();
        SceneManager.LoadScene("Room Generation Test Scene");
    }

    public void RestartLevel() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Debug.Log(gameManager.GetAddDifficulty());
        gameManager.Restart();
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
