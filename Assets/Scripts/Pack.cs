using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{
    public bool[] items = new bool[3];
    //reference to sound effects manager
    public SoundEffectsManager manager;

    //public List<GameObject> getItems { get => items; }

    public void AddItem(GameObject itemtoAdd){
        //if the item is a sword
        if(itemtoAdd.tag == "sword"){
            //set the corresponding boolean in the array to true
            items[0] = true;
            //play the achievement
            manager.play("Achievement");
        }
        //if the item is a gun
        if(itemtoAdd.tag == "gun"){
            //set the corresponding boolean in the array to true
            items[1] = true;
            //play the achievement
            manager.play("Achievement");
        }
        //if the item is a controller
        if(itemtoAdd.tag == "controller"){
            //set the corresponding boolean in the array to true
            items[2] = true;
            //play the achievement
            manager.play("Achievement");
        }
    }

    public bool checkIfCollected(int target){
        //check if the boolean is true at the weapon location
        return items[target-1];

        /*
        foreach(GameObject t in items){
            if(target == 1 && t.tag == "sword") return true;

            if(target == 2 && t.tag == "gun") return true;

            if(target == 3 && t.tag == "controller") return true;
        }
        */
    }
}
