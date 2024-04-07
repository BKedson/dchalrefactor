using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IEnemy interface is the main abstraction for enemies in combat encounters.
public interface IEnemy
{
    // When this enemy is hit
    void OnHit();

    void SetStrength(int stren);

    int Strength();
}
