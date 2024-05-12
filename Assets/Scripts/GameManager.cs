using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    bool canChangeControls = true;
    private int questionComplexity = 0;

    // How many questions has the player gotten right/wrong in a row?
    private int correctStreak = 0;
    private int incorrectStreak = 0;

    // How many right or wrong questions warrant a bump or decrease in diffiuclty?
    private int wrongAnswerThreshold = 2;
    private int rightAnswerThreshold = 2;
    private bool alreadyWrong = false;

    //Difficulty menu can set different difficulties for different operands, unsure if this translates to question generation yet
    Difficulty addDifficulty = Difficulty.Easy;
    Difficulty subtractDifficulty = Difficulty.Easy;
    Difficulty multiplyDifficulty = Difficulty.Easy;
    Difficulty divideDifficulty = Difficulty.Easy;

    // The solution and list of enemy strengths for the current question
    private double currQuestionSol;
    public List<int> currEnemyStrengths;

    // Track player invincibility
    [SerializeField] private bool isInvincible;

    //Character Information---------------------------------------------------------------
    public int currentCharacter; //Stores the global character index for this player - information comes from the Player Data Controller
    private PlayerCharacterData globalCharacterData;
    //-------------------------------------------------------------------------------------
    private void Awake()
    {
        if(manager) {
            DestroyImmediate(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        //Initialize the Character Mapping class----------------------------------------------
        globalCharacterData = new PlayerCharacterData();
        //retrieve index 
        currentCharacter = globalCharacterData.RetrieveCharacterIndex(PlayerGameDataController.Instance.CurrentCharacter);
    }

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        PlayerPrefs.SetInt("invincibility", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart(){
        
    }

    // GETTERS AND SETTERS

    public void SetInvincibility(bool inv){
        if(inv){
            PlayerPrefs.SetInt("invincibility", 1);
        }else{
            PlayerPrefs.SetInt("invincibility", 0);
        }
        isInvincible = inv;
    }

    public bool GetInvincibility(){
        return isInvincible;
    }

    //addition difficulty
    public void ChangeAddDifficulty(Difficulty difficulty) {
        addDifficulty = difficulty;
    }
    public Difficulty GetAddDifficulty(){
        return addDifficulty;
    }

    //subtraction difficulty
    public void ChangeSubtractDifficulty(Difficulty difficulty) {
        subtractDifficulty = difficulty;
    }
    public Difficulty GetSubtractDifficulty(){
        return subtractDifficulty;
    }

    //multiplication difficulty
    public void ChangeMultiplyDifficulty(Difficulty difficulty) {
        multiplyDifficulty = difficulty;
    }
    public Difficulty GetMultiplyDifficulty(){
        return multiplyDifficulty;
    }

    //Division difficulty
    public void ChangeDivideDifficulty(Difficulty difficulty) {
        divideDifficulty = difficulty;
    }
    public Difficulty GetDivideDifficulty(){
        return divideDifficulty;
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

    public int GetQuestionComplexity() {
        return questionComplexity;
    }

    public void WrongAnswer() {
        correctStreak = 0;

        if (!alreadyWrong) {
            incorrectStreak++;
            alreadyWrong = true;
        }

        // Decreases complexity for future problems if the player is struggling
        if (incorrectStreak % wrongAnswerThreshold == 0) {
            questionComplexity = Math.Max(0, questionComplexity - 1);
        }

        Debug.Log("Wrong answer streak: " + incorrectStreak + " Complexity: " + questionComplexity);

    }

    public void RightAnswer() {
            correctStreak++;
            incorrectStreak = 0;

            alreadyWrong = false;

            // Increases complexity for future problems if the player is easily answering questions
            if (correctStreak % rightAnswerThreshold == 0) {
                questionComplexity = Math.Min(9, questionComplexity + 1);
            }

            Debug.Log("Right answer streak: " + correctStreak + " Complexity: " + questionComplexity);
    }

    public double GetCurrQuestionSol() {
        return currQuestionSol;
    }

    public void SetCurrQuestionSol(double sol) {
        currQuestionSol = sol;
    }

    public List<int> GetCurrEnemyStrengths()
    {
        return currEnemyStrengths;
    }

    public void SetCurrEnemyStrengths(List<int> enemyStrengths) {
        currEnemyStrengths = enemyStrengths;
    }

    //-------------------------------------------------------------------
    public int GetCurrentCharacter()
    {
        return currentCharacter;
    }
}
