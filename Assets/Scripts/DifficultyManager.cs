using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private GameObject descriptionText;
    [SerializeField] private GameObject easyButton;
    [SerializeField] private GameObject mediumButton;
    [SerializeField] private GameObject hardButton;
    private GameObject gameManager;

    //For constructing UI feedback on change
    private String easyDescription;
    private String mediumDescription;
    private String hardDescription;

    private String addDescription;
    private String subractDescription;
    private String multiplyDescription;
    private String divideDescription;

    // 0 is addition, 1 is subtraction, 2 is multiplication, 3 is division
    // Changed on button press of difficulty menu screen
    private int operand;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        easyDescription = " is easy.";
        mediumDescription = " is medium.";
        hardDescription = " is hard.";

        addDescription = "The addition";
        subractDescription = "The subtraction";
        multiplyDescription = "The multiplication";
        divideDescription = "The division";

        descriptionText.GetComponent<TextMeshProUGUI>().text = "";

        operand = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeOperand(int op){
        operand = op;
    }

    // Changes the difficulty of the game and sets the corresponding description text
    public void ChangeDifficulty(int diff) {
        Difficulty difficulty = (Difficulty) diff;

        String operandDescription = "";


        switch(operand){
            case 0:
                gameManager.GetComponent<GameManager>().ChangeAddDifficulty(difficulty);
                operandDescription = addDescription;
                break;
            case 1:
                gameManager.GetComponent<GameManager>().ChangeSubtractDifficulty(difficulty);
                operandDescription = subractDescription;
                break;
            case 2:
                gameManager.GetComponent<GameManager>().ChangeMultiplyDifficulty(difficulty);
                operandDescription = multiplyDescription;
                break;
            case 3:
                gameManager.GetComponent<GameManager>().ChangeDivideDifficulty(difficulty);
                operandDescription = divideDescription;
                break;
            default:
                gameManager.GetComponent<GameManager>().ChangeAddDifficulty(difficulty);
                operandDescription = addDescription; 
                break;
        }
        
        // Set UI text to describe change made
        switch (difficulty) {
            case Difficulty.Easy:
                descriptionText.GetComponent<TextMeshProUGUI>().text = operandDescription + easyDescription;
                break;
            case Difficulty.Medium:
                descriptionText.GetComponent<TextMeshProUGUI>().text = operandDescription + mediumDescription;
                break;
            case Difficulty.Hard:
                descriptionText.GetComponent<TextMeshProUGUI>().text = operandDescription + hardDescription;
                break;
            default:
                break;
        }
    }
}
