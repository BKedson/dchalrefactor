using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class temporarily stores a character selection and when a game is started, it transfers that selection to the PlayerGameDataController
public class CharacterSelection : MonoBehaviour
{
    //stores the selected character
    public string selectedCharacter;

    //Updated the character selection
    public void SetSelectedCharacter(string character)
    {
        selectedCharacter = character;
    }
    //returns the current selected character
    public string GetCurrentCharacterSelection()
    {
        return selectedCharacter;
    }

    //saves the selection to the PlayerGameDataController
    public void SaveGlobalCharacterSelection()
    {
        PlayerGameDataController.Instance.CurrentCharacter = selectedCharacter;
        PlayerGameDataController.Instance.SaveGameData();
    }
}
