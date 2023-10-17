using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    //public Transform tpTarget;
    //public GameObject player;
    void OnTriggerEnter(Collider other)
    {
        //player.transform.position = tpTarget.transform.position;
        //change the position of the collider
        //if the collider is a player
        if(other.CompareTag("Player")){
            other.transform.position = new Vector3(2.77f, 0, -3);
        }
    }
}
