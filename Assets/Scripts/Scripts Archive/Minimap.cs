using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    //stores a reference to the player
    public GameObject playerCharacter;
    // Start is called before the first frame update
    void Start()
    {
        //Find the player in the scene
        playerCharacter = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //set this transform position to the position of the playerCharacter
        Vector3 playerPosition = playerCharacter.transform.position; 
        //update playerPosition variable to contain y-position from minimapCamera
        //we want the camera to retain its y-position, not drop down to the player
        playerPosition.y = 0; //this camera's y position
        //update this transform to player position
        transform.position = playerPosition;
    }
}
