using UnityEngine.Audio;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    //An array of type Sound - a class we defined
    public Sound[] sounds;

    //Awake called before start
    void Awake()
    {
        //looping through each Sound object in the Sounds array
        foreach (Sound s in sounds){
            //create an audioSource component
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //void Start
    void Start()
    {
    }

    //PLAY METHOD
    public void play(string name){
        //stores the Sound with the audioSource to be played
        Sound toBePlayed;
        //finding sound with the appropriate name
        foreach (Sound s in sounds){
            //if sound is name
            if(s.name == name){
                toBePlayed = s;
                toBePlayed.source.Play();
                return;
            }
        }
        //at this point- the sound doesn't exist so throw error and return

    }
}
