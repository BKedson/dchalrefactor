using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IObstacle interface is the main abstraction for hazardous objects in combat challenges.
public interface IObstacle
{
    // Defines the behavior of this obstacle when it is activated
    void activated();
}
