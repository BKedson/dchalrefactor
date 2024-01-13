using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IObstacle interface.
// BaseObstacle should be extended by any interactable obstacles in combat challenges.
public abstract class BaseObstacle : MonoBehaviour, IObstacle
{
    public abstract void activated();
}
