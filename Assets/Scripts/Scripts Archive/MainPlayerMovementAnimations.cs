using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovementAnimations : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //getting a reference to the animator component
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive",true);
    }

    // Update is called once per frame
    void Update()
    {
        bool wPressed = Input.GetKey("w");
        bool leftMousePressed = Input.GetMouseButtonDown(0);
        bool isAlive = animator.GetBool("isAlive");

        //if the player is pressing forward, we should run.
        if (wPressed)
        {
            //set isRunning to true- everything else false
            animator.SetBool("isRunning",true);
            animator.SetBool("isAttacking",false);
            animator.SetBool("isIdle",false);
        }
        //if Attacking
        
        /*
        if(leftMousePressed)
        {
             //set isAttacking to true
            animator.SetBool("isAttacking",true);
            animator.SetBool("isRunning",false);
            animator.SetBool("isIdle",false);
        }
        */

        //if doing nothing
        if((!wPressed) && (!leftMousePressed) && isAlive)
        {
             //set isIdle to true
            animator.SetBool("isIdle",true);
            animator.SetBool("isRunning",false);
            animator.SetBool("isAttacking",false);
        }
    }
}
