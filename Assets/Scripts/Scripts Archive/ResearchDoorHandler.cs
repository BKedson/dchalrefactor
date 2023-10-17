using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchDoorHandler : MonoBehaviour
{
    //stores a reference to the door
    public GameObject doorReference;
    //On begin Overlap
    void OnTriggerEnter(Collider other){
        //if it is a player
        if(other.GetComponent<Collider>().tag == "Player" && other.gameObject.GetComponent<PlayerCharacter>().hasDoorKey){
            //open the door
            doorReference.GetComponent<doorHandler>().openDoor();
            //prompt the Enemy Count
            GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("EnemyCountPrompt");
        }
    }

    //On end Overlap
    void OnTriggerExit(Collider other){
        //if it is a player
        if(other.GetComponent<Collider>().tag == "Player" && other.gameObject.GetComponent<PlayerCharacter>().hasDoorKey){
            //close the door
            doorReference.GetComponent<doorHandler>().closeDoor();
        }
    }
}
