using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound{
    //this is for identification within our audio manager
    public string name;
    //this is the data that we want to get from the Sound instance and transfer it to the AudioSource Settings
    //the AudioSource Settings has these attributes 
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
