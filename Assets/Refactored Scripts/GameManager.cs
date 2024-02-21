using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool canChangeControls = true;
    Difficulty globalDifficulty = Difficulty.Easy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDiffuclty(Difficulty difficulty) {
        globalDifficulty = difficulty;
    }

    // Changes the controls based on player input. If an illegal or duplicate button is selected, nothing happens
    public void ChangeControls(int i) {
        Interaction interaction = (Interaction) i;

        if (!canChangeControls) {
            return;
        }

        canChangeControls = false;

        switch (interaction) {
            case Interaction.Forward:
                // Call a function to change the control button to move the player forward and reset the change controls flag
                break;
            case Interaction.Backward:
                // Backward
                break;
            case Interaction.Left:
                // Left
                break;
            case Interaction.Right:
                // Right
                break;
            case Interaction.Interact:
                // Interact
                break;
            case Interaction.Attack:
                // Attack
                break;
            default:
                break;
        }
    }
}
