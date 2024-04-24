using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Player
{
    public class PlayerWeaponsData
    {
        //stores a hashTable for all the character mappings
        private Hashtable weaponMappings;
        // Start is called before the first frame update
        public PlayerWeaponsData()
        {
            weaponMappings = new Hashtable();
            //Initialize character mappings
            InitializeWeaponMappings();
        }

        //Generate Default character mappings
        private void InitializeWeaponMappings()
        {
            //add the default character mappings
            weaponMappings.Add("Sword",0);
            weaponMappings.Add("LaserGun",1);
        }

        //Retrieves the index related to this character
        public int RetrieveWeaponIndex(string name)
        {
            Debug.Log($"Index retrieved {name}");
            return (int)weaponMappings[name];
        }
    }
}
