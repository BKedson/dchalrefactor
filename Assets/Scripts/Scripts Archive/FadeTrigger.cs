using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    //reference to animator of the cross fade
    public Animator transition;

    public float transitionTime = 1f;

    //when we enter this trigger
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            //---------------------------------------------------
            //Animation code begins and everyhting else runs in the background
            //Play animation
            transition.SetTrigger("Start");
            StartCoroutine(startAnimation());
        }
    }

    //delaying function
    IEnumerator startAnimation(){
        //Play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);
    }
}
