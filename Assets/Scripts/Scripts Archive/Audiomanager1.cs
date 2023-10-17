using UnityEngine.Audio;
using UnityEngine;

public class Audiomanager1 : MonoBehaviour
{
    //reference to the GameObject contianing the canvas group
    public GameObject canvasImage;
    //stores how much of the transition effect has hapened using the alpha value of canvasImage
    public float completion;
    //stores whether we are at the beginning of loading the scene
    //to make sure the sound reducing effect does not trigger at beginning of scene load
    public static bool atStart = true;

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

    //void Start plays the sound on Start
    void Start()
    {
        play("Theme");
    }

    //void update
    void Update(){
        if(!atStart){
             //set the update method to get the current alpha of the canvas and set the volume to the level of completion of the animation
            completion = canvasImage.GetComponent<CanvasGroup>().alpha;
            //sound volume
            sounds[0].source.volume = 1 - completion;
        }
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
