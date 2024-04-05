using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{

    //The amount of damage delt on hit
    protected internal int Damage;
    public abstract void Attack();

    public abstract void Attack(bool can);
}