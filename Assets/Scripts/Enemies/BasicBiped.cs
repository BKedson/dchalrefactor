using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BasicBipeds are the simplest enemies the player will encounter. They move around on two feet and have a melee attack.
public class BasicBiped : BaseEnemy
{
   // Start is called before the first frame update
    void Start()
    {
        // Example for finding and setting the player object
        player = GameObject.Find("Player");
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// TEMPORARY SCRIPT FOR DEATH----------------------------------------------------------------------------------------------------------------
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        //if it is a weapon
        if (other.gameObject.tag == "sword")
        {
            Destroy(gameObject);
        }
    }

    // How does move work for a biped?
    internal override void Move()
    {

    }

    internal override void Attack()
    {

    }

    public override void OnHit()
    {

    }

    internal override void Death()
    {

    }


    // GETTERS AND SETTERS
    public override void SetStrength(int stren) {
        strength = stren;
    }

    public override void SetDamage(int dam) {
        damage = dam;
    }

    public override int GetDamage() {
        return damage;
    }

    public override int GetStrength() {
        return strength;
    }

}
