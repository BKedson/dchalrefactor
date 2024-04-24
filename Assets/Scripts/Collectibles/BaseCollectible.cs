using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCollectible : MonoBehaviour
{
    //stores the name of this Collectible
    private string name;

    //Start
    void Start()
    {
        name = GetCollectibleName();
    }

    //returns the name of this collectible
    public abstract string GetCollectibleName();
}
