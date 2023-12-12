using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WeaponController : MonoBehaviour
{

    public GameObject sword;
    public GameObject shrinkRay;
    public GameObject controller;
    public bool canAttack = true;
    private Animator anim;
    public int currentWeapon = 0;
    public SoundEffectsManager manager;
    public bool inResearch;
    public RayShooter inp;
    public PlayerInventory stuff;
    public BoxCollider swordHitBox;

    // Start is called before the first frame update
    void Start()
    {
        inResearch = true;
        inp = gameObject.GetComponentInParent(typeof(RayShooter)) as RayShooter;
        stuff = gameObject.GetComponentInParent<PlayerInventory>();
        anim = sword.GetComponent<Animator>();
        swordHitBox = sword.GetComponent<BoxCollider>();
        selectWeapon(-1);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWeapon == 0) canAttack = true;
        else canAttack = false;


        if(Input.GetMouseButtonUp(0)){
            if(canAttack)   swordAttack();
        }
        else{
            anim.SetBool("Attack", false);
        }
    }
    public void swap(int i){
        
        if(i == 1 && stuff.checkIfCollected(1)){
            selectWeapon(0);
            inp.hasRay = false;
            inp.hasSword = true;
            inp.hasFreeze = false;
            inp.hasHack = false;
            Debug.Log("im dead");
        }  

        if(i == 2 && stuff.checkIfCollected(2)){
               selectWeapon(1);
            inp.hasRay = true;
            inp.hasSword = false;
            inp.hasFreeze = false;
            inp.hasHack = false;
            
        }

        if(i == 3 && stuff.checkIfCollected(3)){
            selectWeapon(2);
            inp.hasRay = false;
            inp.hasSword = false;
            inp.hasFreeze = false;
            inp.hasHack = false;
           
        }  
    }

    public void swordAttack(){
        //if sword is active
        if(transform.GetChild(0).gameObject.activeSelf){
            //play animation
            swordHitBox.enabled = true;
            anim.SetBool("Attack", true);
            //play sword sound
        }

        //if gun is active
        if(transform.GetChild(1).gameObject.activeSelf){
            //play laser sound 
        }

    }

    public void selectWeapon(int swap){
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }

        if(swap == -1)  return;
        transform.GetChild(swap).gameObject.SetActive(true);
    }
}
