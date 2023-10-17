using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class doorHandler : MonoBehaviour
{
    //stores the GameObject door
    public GameObject doorRef;
    //stores the door object
    public Animator doorAnim;
    //stores a reference to the SoundEffectsManager
    public SoundEffectsManager manager;
    // Start is called before the first frame update
    void Start()
    {
        doorAnim = doorRef.GetComponent<Animator>();
        //open the doors at start for dramatic effect
        doorAnim.SetBool("openDoor",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //open Door script
    public void openDoor(){
        //open the door
        doorAnim.SetBool("openDoor", true);
        //opening door sound
        manager.play("DoorClosing");
    }

    //close Door script
    public void closeDoor(){
        //close the door
        doorAnim.SetBool("openDoor", false);
        doorAnim.SetTrigger("closeDoor");
        //closing door sound
        manager.play("DoorClosing");
    }
}
