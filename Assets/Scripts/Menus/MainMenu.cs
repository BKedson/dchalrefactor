using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class MainMenu : MonoBehaviour
{
    //stores the reference to the continue button
    public GameObject continueButton;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerGameDataController.Instance.CheckIfNewUser())
        {
            //if the user is new, they should not have the option to continue
            continueButton.SetActive(false);
        }

        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerGameDataController.Instance.MusicVolume)*20);
        Debug.Log(PlayerGameDataController.Instance.MusicVolume);
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(PlayerGameDataController.Instance.SFXVolume)*20);
        Debug.Log(PlayerGameDataController.Instance.SFXVolume);
        
    }
    // sets the game type depending on whether the user chose to continue or start New Game
    public void SetGameType(bool gameType) 
    {
        PlayerGameDataController.Instance.IndicateNewGame(gameType);
    }
}
