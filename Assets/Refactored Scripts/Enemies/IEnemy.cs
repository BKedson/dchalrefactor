using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IEnemy interface is the main abstraction for enemies in combat encounters.
public interface IEnemy
{
    // Controls movement for this enemy
    void Move();
}
