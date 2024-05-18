using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BillboardText : MonoBehaviour
{
    private BaseEnemy enemyScript;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        enemyScript = gameObject.GetComponentInParent<BaseEnemy>();
    }   

    void Update() {
        if (enemyScript != null)
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "" + gameObject.GetComponentInParent<BaseEnemy>().GetStrength();
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
