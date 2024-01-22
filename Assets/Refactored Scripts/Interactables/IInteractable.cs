using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IInteractable interface is the main abstraction for interactable objects in all challenges.
public interface IInteractable
{
    // Defines behavior for this object when the player interacts with it.
    void OnInteract();
}
