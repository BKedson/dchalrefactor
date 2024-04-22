using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dchalrefactor.Scripts.Player
{
    public class PlayerCharacterData
    {
        //stores a hashTable for all the character mappings
        private Hashtable characterMappings;
        // Start is called before the first frame update
        public PlayerCharacterData()
        {
            characterMappings = new Hashtable();
            //Initialize character mappings
            InitializeCharacterMappings();
        }

        //Generate Default character mappings
        private void InitializeCharacterMappings()
        {
            //add the default character mappings
            characterMappings.Add("DC_Woman_2",0);
            characterMappings.Add("DC_Man_1",1);
        }

        //Retrieves the index related to this character
        public int RetrieveCharacterIndex(string name)
        {
            Debug.Log($"Index retrieved {name}");
            return (int)characterMappings[name];
        }
    }
}
