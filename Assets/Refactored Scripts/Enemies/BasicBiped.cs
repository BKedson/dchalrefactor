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
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log(strength);
    }

    public override int Strength() {
        return strength;
    }

}
