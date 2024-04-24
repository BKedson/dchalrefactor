using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectible : BaseCollectible
{
    //stores the name of this Weapon
    public string WeaponName;
    //Overriding the base class abstract methods
    //Get name
    public override string GetCollectibleName()
    {
        return WeaponName;
    }
}
