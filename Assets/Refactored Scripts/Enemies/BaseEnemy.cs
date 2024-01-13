using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IEnemy interface. Unlike IEnemy, BaseEnemy keeps track of the player GameObject.
// BaseEnemy should be extended by any enemies in combat encounters.
public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    internal GameObject player;

    public abstract void Move();
}
