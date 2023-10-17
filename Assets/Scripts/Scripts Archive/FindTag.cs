using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FindTag : MonoBehaviour
{
    public GameObject[] tagged;
    void Awake()
    {
        tagged = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject floor in tagged)
         {
             floor.layer = LayerMask.NameToLayer("Ground");
         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
