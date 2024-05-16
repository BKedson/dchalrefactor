using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private GameObject descriptionText;
    [SerializeField] private GameObject easyButton;
    [SerializeField] private GameObject mediumButton;
    [SerializeField] private GameObject hardButton;

    [SerializeField] private GameObject[] addButtons;
    [SerializeField] private GameObject[] subtractButtons;
    [SerializeField] private GameObject[] multiplyButtons;
    [SerializeField] private GameObject[] divideButtons;

    private GameManager gameManager;

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

    //locally keep track of last set difficulty
    private Difficulty localDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.manager;

        easyDescription = " is easy.";
        mediumDescription = " is medium.";
        hardDescription = " is hard.";

        addDescription = "The addition";
        subractDescription = "The subtraction";
        multiplyDescription = "The multiplication";
        divideDescription = "The division";

        descriptionText.GetComponent<TextMeshProUGUI>().text = "";

        // Highlight current difficulty settings
        operand = 0;
        localDifficulty = GameManager.manager.GetAddDifficulty();
        HighlightButton();

        operand = 1;
        localDifficulty = GameManager.manager.GetSubtractDifficulty();
        HighlightButton();

        operand = 2;
        localDifficulty = GameManager.manager.GetMultiplyDifficulty();
        HighlightButton();

        operand = 3;
        localDifficulty = GameManager.manager.GetDivideDifficulty();
        HighlightButton();
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
        localDifficulty = (Difficulty) diff;

        String operandDescription = "";


        switch(operand){
            case 0:
                gameManager.ChangeAddDifficulty(difficulty);
                operandDescription = addDescription;
                break;
            case 1:
                gameManager.ChangeSubtractDifficulty(difficulty);
                operandDescription = subractDescription;
                break;
            case 2:
                gameManager.ChangeMultiplyDifficulty(difficulty);
                operandDescription = multiplyDescription;
                break;
            case 3:
                gameManager.ChangeDivideDifficulty(difficulty);
                operandDescription = divideDescription;
                break;
            default:
                gameManager.ChangeAddDifficulty(difficulty);
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

    public void HighlightButton(){

        switch(operand){
            case 0:
                for(int i=0; i<addButtons.Length; i++){
                    if((int)localDifficulty == i){
                        addButtons[i].GetComponent<Image>().color = new Color(0.208f, 0.337f, 0.569f, 1.0f);
                    }else{
                        addButtons[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                    
                }

                break;
            case 1:
                for(int i=0; i<subtractButtons.Length; i++){
                    if((int)localDifficulty == i){
                        subtractButtons[i].GetComponent<Image>().color = new Color(0.208f, 0.337f, 0.569f, 1.0f);
                    }else{
                        subtractButtons[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                }
                break;
            case 2:
                for(int i=0; i<multiplyButtons.Length; i++){
                    if((int)localDifficulty == i){
                        multiplyButtons[i].GetComponent<Image>().color = new Color(0.208f, 0.337f, 0.569f, 1.0f);
                    }else{
                        multiplyButtons[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                }
                break;
            case 3:
                for(int i=0; i<divideButtons.Length; i++){
                    if((int)localDifficulty == i){
                        divideButtons[i].GetComponent<Image>().color = new Color(0.208f, 0.337f, 0.569f, 1.0f);
                    }else{
                        divideButtons[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                }
                break;
            default:

                break;
        }

        switch (localDifficulty) {
            case Difficulty.Easy:
                
                break;
            case Difficulty.Medium:
                
                break;
            case Difficulty.Hard:
                
                break;
            default:
                break;
        }
    }
}
