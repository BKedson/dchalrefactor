using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IInteractable interface. Unlike IEnemy, BaseEnemy keeps track of the player GameObject.
// BaseInteractable should be extended by any interactable objects in all challenges.
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    internal GameObject player;

    public abstract void onInteract();
}
