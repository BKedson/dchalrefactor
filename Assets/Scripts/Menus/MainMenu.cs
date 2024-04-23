using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //stores the reference to the continue button
    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerGameDataController.Instance.CheckIfNewUser())
        {
            //if the user is new, they should not have the option to continue
            continueButton.SetActive(false);
        }
    }
    // sets the game type depending on whether the user chose to continue or start New Game
    public void SetGameType(bool gameType) 
    {
        PlayerGameDataController.Instance.IndicateNewGame(gameType);
    }
}
