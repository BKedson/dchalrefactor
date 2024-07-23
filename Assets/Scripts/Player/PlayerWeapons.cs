using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Animations.PlayerMovement;

namespace dchalrefactor.Scripts.Player
{
    public class PlayerWeapons : MonoBehaviour
    {
        //stores an array of this Player Holder's weapons
        public GameObject[] Weapons;
        //stores an instance of the PlayerWeaponsData class to map Weapon names to indices
        private PlayerWeaponsData playerWeaponsData;
        private PlayerMovement playerMovement;

        public bool weaponEquipped;
        // Start is called before the first frame update
        void Start()
        {
            //initialize the player weapons data
            playerWeaponsData = new PlayerWeaponsData();
            playerMovement = GetComponentInParent<PlayerMovement>();
            //Deactivates all weapons at start
            DeactivateAllWeapons(); 
        }

        public void ActivateWeapon(string name)
        {
            //get the index
            int index = playerWeaponsData.RetrieveWeaponIndex(name);
            //set the corresponding weapon active
            Weapons[index].SetActive(true);
            //Set the Player Animations class to Equipped
            gameObject.GetComponent<PlayerAnimations>().EquipWeapon(true);
            // Unset attack trigger so player doesn't attack as weapon is equipped
            gameObject.GetComponent<PlayerAnimations>().CancelAttack();
            // Let tutorial know the weapon was collected
            GameObject.Find("Tutorial Manager").GetComponent<TextboxBehavior>().SwordEquipped();

            playerMovement.weaponEquipped = true;
        }

        public void DeactivateWeapon(string name)
        {
            //get the index
            int index = playerWeaponsData.RetrieveWeaponIndex(name);
            //set the corresponding weapon inactive
            Weapons[index].SetActive(false);
            //Set the Player Animations class to NOT EQUIPPED
            gameObject.GetComponent<PlayerAnimations>().EquipWeapon(false);

            playerMovement.weaponEquipped = false;
        }

        public void DeactivateAllWeapons()
        {
            //set every weapon inactive
            foreach(GameObject weapon in Weapons)
            {
                weapon.SetActive(false);
            }
            //Set the Player Animations class to NOT EQUIPPED
            gameObject.GetComponent<PlayerAnimations>().EquipWeapon(false);
        }
    }
}
