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
    [SerializeField] private GameManagerProxy gameManager;

    private void Start()
    {
        float savedVolume = gameManager.GetSFXVolume();
        SetSFXMixerVolume(savedVolume);
        slider.value = savedVolume;
    }

    public void SetSFXMixerVolume(float volume)
    {
        mixer.SetFloat(mixerVolume, Mathf.Log10(volume)*20);
        gameManager.SetSFXVolume(slider.value);
    }

    public void OnSliderValueChanged()
    {
        SetSFXMixerVolume(slider.value);
    }
    
}
