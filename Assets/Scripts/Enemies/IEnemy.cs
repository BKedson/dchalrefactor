using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IEnemy interface is the main abstraction for enemies in combat encounters.
public interface IEnemy
{
    // When this enemy is hit
    void OnHit();
    int GetStrength();
    int GetDamage();
    void SetStrength(int stren);
    void SetDamage(int dam);
}
