using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private BaseWeapon cur;
    private Animator anim;
    public int currentWeapon = -1;
    public SoundEffectsManager manager;
    public bool inResearch;
    public RayShooter inp;
    public PlayerInventory stuff;
    public BoxCollider swordHitBox;

    // Start is called before the first frame update
    void Start()
    {
        inResearch = true;
        inp = gameObject.transform.parent.GetComponentInChildren(typeof(RayShooter)) as RayShooter;
        stuff = gameObject.GetComponentInParent<PlayerInventory>();
        //anim = sword.GetComponent<Animator>();
        //swordHitBox = sword.GetComponent<BoxCollider>();
        selectWeapon(-1);
    }

    public void swap(int i){
        
        if(i == 1 && stuff.checkIfCollected(1)){
            selectWeapon(0);
            inp.hasRay = false;
            inp.hasSword = true;
            inp.hasFreeze = false;
            inp.hasHack = false;
            currentWeapon = 0;
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

    public void currentAttack(bool start){
        if(currentWeapon != -1){
            cur.Attack(start);
        }
    }

    private void selectWeapon(int swap){
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }

        if(swap == -1)  return;
        cur = transform.GetChild(swap).gameObject.GetComponent<BaseWeapon>();
        transform.GetChild(swap).gameObject.SetActive(true);
    }
}
