using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IObstacle interface.
// BaseObstacle should be extended by any interactable obstacles in combat challenges.
public abstract class BaseObstacle : MonoBehaviour, IObstacle
{
    // The amount of damage this obstacle deals
    protected internal int damage;

    // The player object this obstacle will damage
    protected internal GameObject player;
    // public abstract void Activated();
}
