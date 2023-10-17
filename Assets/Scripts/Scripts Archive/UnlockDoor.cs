using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class UnlockDoor : MonoBehaviour
{
    //stores the integers to be tested
    public int firstNumberCheck;
    public int secondNumberCheck;
    //stores the sign to be checked
    public string operatorCheck;
    public bool waiting;
    public GameObject colliderCheck;
    Animator openDoorAnim;
    public GameObject doorReference;

    // Start is called before the first frame update
    void Start()
    {
        openDoorAnim = doorReference.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called for a correct answer
    public void correctAnswer(){
        //set correct Answer of the collider to true.
        //colliderCheck.correctAnswer = true;
        //open the Door actions
        openDoorAnim.SetBool("openDoor",true);

    }

    //called for a wrong answer
    public void wrongAnswer(){}
}
