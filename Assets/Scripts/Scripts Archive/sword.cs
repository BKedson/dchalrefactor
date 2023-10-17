using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
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
