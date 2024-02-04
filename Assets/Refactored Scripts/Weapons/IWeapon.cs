using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IWeapon interface is the main abstraction for weapons.
// Weapons in this case include the anything the player uses to interact with enemies.
public interface IWeapon
{
    // Defines the attack of the weapon
    void Attack();

}