using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SwordWeapon defines our basic melee weapon

public class SwordWeapon :  BaseWeapon
{

    private Animator anim;
    private AnimatorClipInfo[] currrentClip;
    private BoxCollider swordHitBox;

    void Start(){
        Damage = 100;
        anim = GetComponentInParent<Animator>();
        swordHitBox = GetComponentInChildren<BoxCollider>();
        swordHitBox.enabled = false;
    }

    void Update(){
        currrentClip = anim.GetCurrentAnimatorClipInfo(0);
        if(currrentClip[0].clip.name == "Idle_Normal|Idle_Action" ||
        currrentClip[0].clip.name == "Idle_Normal|Idle_Sword" ||
        currrentClip[0].clip.name == "Idle_Normal|Sword_Attack_Special" ||
        currrentClip[0].clip.name == "Idle_Normal|Sword_Attack_Slash" ||
        currrentClip[0].clip.name == "Idle_Normal|Sword_Attack_Upclose"){
            enableCollider();
        }
        else{disableCollider();}
    }

    public override void Attack()
    {
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
            enableCollider();
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
		swordHitBox.enabled = false;
	}

    public void enableCollider(){
		swordHitBox.enabled = true;
	}
}