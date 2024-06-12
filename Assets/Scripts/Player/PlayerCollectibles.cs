using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Player;
using System;

public class PlayerCollectibles : MonoBehaviour
{

    public AudioClip collectSound;
    private AudioSource audioSource;

    //event reference for the tutorial
    public event Action<string> OnCollection;

    //stores a reference to the player character class
    public PlayerCharacter playerCharacter;
    //------------------------------------------------------COLLECTIBLES----------------------------------------------------------------------------
    //---------------------------------------------------------WEAPONS-----------------------------------------------
    //Stores a reference to the PlayerWeapons class
    private PlayerWeapons playerWeapons;
    //----------------------------------------------------------------------------------------------------------------------------------------------
    void Start ()
    {
        //Initialize the playerWeapons variable with the corresponding active character
        playerWeapons = GetActiveCharacterWeapons();

        audioSource = GetComponent<AudioSource>();
    }

    public PlayerWeapons GetActiveCharacterWeapons()
    {
        return playerCharacter.GetActiveCharacter().GetComponent<PlayerWeapons>() as PlayerWeapons;
    }

    public void CollectWeapon(string name) //collects a weapon and activates it
    {
        playerWeapons = GetActiveCharacterWeapons();
        //Deactivate all weapons and then activate the pickup
        playerWeapons.DeactivateAllWeapons();
        //activate using the attached playerWeapons class
        playerWeapons.ActivateWeapon(name);

        audioSource.PlayOneShot(collectSound);

        OnCollection?.Invoke(name);
    }
}
    
