using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IPickupable interface defines behavior for objects that can be picked up.
public interface IPickupable
{
    void OnPickup();

    void OnPutDown();

}
