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

        StartCoroutine(SetAudioLevels());
    }

    private IEnumerator SetAudioLevels(){

        //wait to ensure game data is properly set
        yield return new WaitForSeconds(0.5f);

        float musicVol = PlayerGameDataController.Instance.MusicVolume;
        float sfxVol = PlayerGameDataController.Instance.SFXVolume;
        //ensure valid volume levels, unsure how or where but sometimes it seems to round to 0
        if(musicVol == 0){
            musicVol = 0.0001f;
        }
        if(sfxVol == 0){
            sfxVol = 0.0001f;
        }
        musicMixer.SetFloat("MusicVol", Mathf.Log10(musicVol)*20);
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(sfxVol)*20);
    }
    // sets the game type depending on whether the user chose to continue or start New Game
    public void SetGameType(bool gameType) 
    {
        PlayerGameDataController.Instance.IndicateNewGame(gameType);
    }
}
