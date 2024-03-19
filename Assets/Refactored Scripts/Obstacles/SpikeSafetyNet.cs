using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpikeSafetyNet : MonoBehaviour
{
    private GameObject player;
    // How long the player will remain in the pit of spikes before being teleported back to safety
    private float cooldown = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Teleports the player back to safety after a brief cooldown
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.Equals(player)) {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport() {
        yield return new WaitForSeconds(cooldown);
        player.transform.position = new Vector3(0, 5, 0);
    }
}
