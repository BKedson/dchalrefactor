using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : BaseObstacle
{
    [SerializeField] float cooldown = 1f;
    bool inSpikes = true;

    // Start is called before the first frame update
    void Start()
    {
        // Set inherited fields
        damage = 1;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Damages the player when they enter and every [cooldown] seconds afterwards
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.Equals(player)) {
            inSpikes = true;
            StartCoroutine(DamageLoop());
        }
    }

    // Stops damaging the player when they leave the spikes
    void OnTriggerExit(Collider col) {
        if (col.gameObject.Equals(player)) {
            inSpikes = false;
        }
    }

    // Continues damaging the player as long as they remain in the spikes
    IEnumerator DamageLoop() {
        Stab();
        yield return new WaitForSeconds(cooldown);
        if (inSpikes) {
            StartCoroutine(DamageLoop());
        }
    }

    // Damages the player
    void Stab() {
        player.GetComponent<PlayerCharacter>().Hurt(damage);
        Debug.Log("stab!");
    }
}
