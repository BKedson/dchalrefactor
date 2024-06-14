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
    [SerializeField] private GameManagerProxy gameManager;

    private void Start()
    {
        float savedVolume = gameManager.GetMusicVolume();
        SetMusicMixerVolume(savedVolume);
        slider.value = savedVolume;
    }

    public void SetMusicMixerVolume(float volume)
    {
        mixer.SetFloat(mixerVolume, Mathf.Log10(slider.value)*20);
        gameManager.SetMusicVolume(slider.value);
    }

    public void OnSliderValueChanged()
    {
        SetMusicMixerVolume(slider.value);
    }

}
