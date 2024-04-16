using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BillboardText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void Update() {
        if (gameObject.GetComponentInParent<BasicBiped>() != null)
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "" + gameObject.GetComponentInParent<BasicBiped>().GetStrength();
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
