using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public static TransitionManager transitionMan;

    private void Awake() {
        // Don't destroy the transition manager on load, but do destroy if there are duplicates
        if(transitionMan) {
            DestroyImmediate(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            transitionMan = this;
        }
    }

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
            PlayerPrefs.SetString("CurrentLevel", "Test Combat Room");
            SceneManager.LoadScene("Test Combat Room");
        }
    }

    public void NewGame() {
        SceneManager.LoadScene("Test Combat Room");
    }

    public void Options() {
        SceneManager.LoadScene("Options");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Quit() {
        Application.Quit();
    }

}
