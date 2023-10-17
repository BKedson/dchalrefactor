using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneController : MonoBehaviour
{
    public float floatSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        float floatRate = Time.deltaTime * floatSpeed;
        transform.Translate(0,0,floatRate);
    }
}
