using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawners spawn a set number of additional enemies for the player to fight. If they are destroyed, they will stop producing enemies.
public class Spawner : BaseEnemy
{

    [SerializeField] private int remainingSpawns = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // How does move work for a spawner?
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

    public override int Strength() {
        return strength;
    }
}
