using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string mixerVolume = "MusicVol";
    public Slider slider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SavedMusicVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("SavedMusicVolume");
            SetMusicMixerVolume(savedVolume);
            slider.value = savedVolume;
        }
    }

    public void SetMusicMixerVolume(float volume)
    {
        mixer.SetFloat(mixerVolume, Mathf.Log10(slider.value)*20);
        PlayerPrefs.SetFloat("SavedMusicVolume", slider.value);
    }

}
