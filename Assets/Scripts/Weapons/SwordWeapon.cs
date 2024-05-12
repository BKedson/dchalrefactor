using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SwordWeapon defines our basic melee weapon

public class SwordWeapon :  BaseWeapon
{

    private Animator anim;
    private BoxCollider swordHitBox;

    void Start(){
        Damage = 100;
        anim = GetComponent<Animator>();
        swordHitBox = GetComponent<BoxCollider>();
    }

    public override void Attack()
    {
        Debug.Log("swing1");
        //if sword is active
        if(gameObject.activeSelf){
            //play animation
            swordHitBox.enabled = true;
            anim.SetBool("Attack", true);
        }
    }

    public override void Attack(bool can)
    {
        //if sword is active
        if(gameObject.activeSelf & can){
            //play animation
            swordHitBox.enabled = true;
            anim.SetBool("Attack", true);
        }
        else {
            stopSwing();
        }
    }

    public void stopSwing(){anim.SetBool("Attack", false);}

    public void OnTriggerEnter(Collider other){
        Debug.Log("I am a sword and I have hit: " + other.name);
		//call the death function
		if(other.CompareTag("enemy")){
			//calling the death function on this actor
			Destroy(other.gameObject);
		}
	}
    public void disableCollider(){
		GetComponent<BoxCollider>().enabled = false;
	}
}