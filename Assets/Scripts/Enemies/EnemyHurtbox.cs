using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    private int damage;
    private float cooldown = 1f;
    private bool hitting = false;
    private float lastDamage;

    // Start is called before the first frame update
    void Start()
    {
        damage = gameObject.GetComponentInParent<BaseEnemy>().GetDamage();
        lastDamage = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //damage player
	public void OnTriggerEnter(Collider col){
		if(col.CompareTag("Player") && Time.time >= lastDamage + cooldown){
			hitting = true;
			StartCoroutine(DamageLoop(col));
		}
	}

	public void OnTriggerExit(Collider col){
		if (col.CompareTag("Player")) {
            hitting = false;
        }
	}

    IEnumerator DamageLoop(Collider col) {
        Hit(col);
        yield return new WaitForSeconds(cooldown);
        if (hitting) {
            StartCoroutine(DamageLoop(col));
        }
    }

    // Damages the player
    void Hit(Collider col) {
        col.gameObject.GetComponent<PlayerCharacter>().Hurt(damage);
        lastDamage = Time.time;
        // Debug.Log("I have hit the player");
    }
}
