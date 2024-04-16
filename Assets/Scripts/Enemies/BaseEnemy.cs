using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IEnemy interface. Unlike IEnemy, BaseEnemy keeps track of the player GameObject.
// BaseEnemy should be extended by any enemies in combat encounters.
public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    protected internal GameObject player;
    protected internal int strength;
    protected internal int damage;

    // Controls movement for this enemy
    internal abstract void Move();

    // Triggers an attack for this enemy
    internal abstract void Attack();

    // When this enemy dies
    internal abstract void Death();

    public abstract void OnHit();
    public abstract int GetStrength();
    public abstract int GetDamage();
    public abstract void SetDamage(int dam);
    public abstract void SetStrength(int stren);

}
