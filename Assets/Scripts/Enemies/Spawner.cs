using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;

// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

// Spawners spawn a set number of additional enemies for the player to fight. If they are destroyed, they will stop producing enemies.
public class Spawner : BaseEnemy
{

    [SerializeField] private GameObject enemyPrefab;
    private int remainingSpawns;
    private int maxSpawns = 5;
    private float spawnFrequency = 5f;

    // Start is called before the first frame update
    void Start()
    {
        remainingSpawns = Math.Min(strength, UnityEngine.Random.Range(2, maxSpawns));
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

    public void startSpawns() {
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// TEMPORARY SCRIPT FOR DEATH----------------------------------------------------------------------------------------------------------------
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        //if it is a weapon
        if (other.gameObject.name == "DamageCollider")
        {
            Destroy(gameObject);
        }
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

    IEnumerator SpawnLoop() {
        SpawnEnemy();
        remainingSpawns--;

        yield return new WaitForSeconds(spawnFrequency);

        if (remainingSpawns > 0) {
            StartCoroutine(SpawnLoop());
        }
    }

    // Damages the player
    void SpawnEnemy() {
        int enemyStrength = strength / remainingSpawns;
        Instantiate(enemyPrefab, transform.position, Quaternion.Euler(0, 180, 0)).GetComponent<BaseEnemy>().SetStrength(enemyStrength);
        strength -= enemyStrength;
        Debug.Log("enemy spawned");
    }
}
