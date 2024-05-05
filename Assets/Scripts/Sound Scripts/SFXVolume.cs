using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string mixerVolume = "SFXVol";
    public Slider slider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SavedSFXVol"))
        {
            float savedVolume = PlayerPrefs.GetFloat("SavedSFXVol");
            SetSFXMixerVolume(savedVolume);
            slider.value = savedVolume;
        }
    }

    public void SetSFXMixerVolume(float volume)
    {
        mixer.SetFloat(mixerVolume, Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SavedSFXVol", volume);
    }

    public void OnSliderValueChanged()
    {
        SetSFXMixerVolume(slider.value);
    }
    
}
