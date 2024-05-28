using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsBehavior : MonoBehaviour
{
    private bool scroll;
    private float defaultSpeed = 50.0f;
    private float scrollSpeed;
    private float creditsLength;
    private float startHeight;
    // Start is called before the first frame update
    void Start()
    {
        scroll = true;
        scrollSpeed = defaultSpeed;
        creditsLength = gameObject.GetComponent<RectTransform>().rect.height;
        startHeight = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if up arrow is pressed, scroll faster
        if(Input.GetKey(KeyCode.UpArrow)){
            scrollSpeed = defaultSpeed*3;

        //if down arrow is pressed, reverse scroll
        }else if(Input.GetKey(KeyCode.DownArrow)){
            scrollSpeed = -defaultSpeed*3;

        //return to default scroll
        }else{
            scrollSpeed = defaultSpeed;
        }

        if(scroll){

            Vector3 currPosition = gameObject.transform.position;

            //reset height of credits when scroll is complete
            if (currPosition.y > creditsLength + Screen.height){ 
                gameObject.transform.position = new Vector3(currPosition.x, startHeight, currPosition.z);

            //reset height of credits when reverse scroll is complete
            }else if(currPosition.y < startHeight){
                gameObject.transform.position = new Vector3(currPosition.x, creditsLength + Screen.height - 1, currPosition.z);
            }else{ 
                gameObject.transform.position = new Vector3(currPosition.x, currPosition.y + scrollSpeed*Time.deltaTime, currPosition.z);
            }
            
        }
    }

}
