using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dchalrefactor.Scripts.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    bool canChangeControls = true;
    private int addQuestionComplexity = 0;
    private int subQuestionComplexity = 0;
    private int multQuestionComplexity = 0;
    private int divQuestionComplexity = 0;
    private GameObject player;
    private Vector3 playerSpawnPoint;

    // How many questions has the player gotten right/wrong in a row?
    private int correctStreak = 0;
    private int incorrectStreak = 0;

    // How many right or wrong questions warrant a bump or decrease in diffiuclty?
    private int wrongAnswerThreshold = 2;
    private int rightAnswerThreshold = 2;
    private bool alreadyWrong = false;

    //Difficulty menu can set different difficulties for different operands
    Difficulty addDifficulty = Difficulty.Easy;
    Difficulty subtractDifficulty = Difficulty.Easy;
    Difficulty multiplyDifficulty = Difficulty.Easy;
    Difficulty divideDifficulty = Difficulty.Easy;

    private Subject currSubject;

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
        // isInvincible = false;
        // PlayerPrefs.SetInt("invincibility", 0);
        isInvincible = PlayerGameDataController.Instance.IsInvincible;
        addQuestionComplexity = PlayerGameDataController.Instance.AdditionQuestionComplexity;
        subQuestionComplexity = PlayerGameDataController.Instance.SubtractionQuestionComplexity;
        multQuestionComplexity = PlayerGameDataController.Instance.MultiplicationQuestionComplexity;
        divQuestionComplexity = PlayerGameDataController.Instance.DivisionQuestionComplexity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart(){
        player = GameObject.Find("Player");

        if (player) {
            // player.transform.position = playerSpawnPoint;
            // player.GetComponentInChildren<PlayerWeapons>().DeactivateAllWeapons();
            player.GetComponent<PlayerCharacter>().Reset();
            currentCharacter = globalCharacterData.RetrieveCharacterIndex(PlayerGameDataController.Instance.CurrentCharacter);
            player.GetComponent<PlayerCharacter>().InitializePlayer();
            // player.GetComponent<PlayerMovement>().Reset();
        }

        Time.timeScale = 1;

        if (DungeonGenerator._instance) {
            DungeonGenerator._instance.ProceedLv();
        }

        correctStreak = 0;
    }

    public void ChangeSkin() {
        player = GameObject.Find("Player");
        
        if (player) {
            player.GetComponent<PlayerCharacter>().ChangeActiveSkin();
            player.GetComponent<PlayerMovement>().Reset();
        }
    }

    public void Save() {
        Debug.Log("Game saved");
        PlayerGameDataController.Instance.SaveGameData();
    }

    // GETTERS AND SETTERS

    public void SetInvincibility(bool inv){
        // if(inv){
        //     PlayerPrefs.SetInt("invincibility", 1);
        // }else{
        //     PlayerPrefs.SetInt("invincibility", 0);
        // }
        isInvincible = inv;
        PlayerGameDataController.Instance.IsInvincible = isInvincible;
    }

    public bool IsInvincible(){
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
        switch (currSubject) {
            case Subject.Addition:
                return addQuestionComplexity;
            break;
            case Subject.Subtraction:
                return subQuestionComplexity;
            break;
            case Subject.Multiplication:
                return multQuestionComplexity;
            break;                
            case Subject.Division:
                return divQuestionComplexity;
            break;
            default:
                return addQuestionComplexity;
            break;
        }
    }

    public void WrongAnswer() {
        correctStreak = 0;

        if (!alreadyWrong) {
            incorrectStreak++;
            alreadyWrong = true;
        }

        // Decreases complexity for future problems if the player is struggling
        if (incorrectStreak % wrongAnswerThreshold == 0) {
            UpdateComplexity(false);
        }
    }

    public void RightAnswer() {
            correctStreak++;
            incorrectStreak = 0;

            alreadyWrong = false;

            // Increases complexity for future problems if the player is easily answering questions
            if (correctStreak % rightAnswerThreshold == 0) {
                UpdateComplexity(true);
            }
    }

    private void UpdateComplexity(bool correct) {
        switch (currSubject) {
            case Subject.Addition:
                if (correct) {
                    addQuestionComplexity = Math.Min(9, addQuestionComplexity + 1);
                    PlayerGameDataController.Instance.AdditionQuestionComplexity = addQuestionComplexity;
                } else {
                    addQuestionComplexity = Math.Max(0, addQuestionComplexity - 1);
                    PlayerGameDataController.Instance.AdditionQuestionComplexity = addQuestionComplexity;
                }
            break;
            case Subject.Subtraction:
                if (correct) {
                    subQuestionComplexity = Math.Min(9, subQuestionComplexity + 1);
                    PlayerGameDataController.Instance.SubtractionQuestionComplexity = subQuestionComplexity;
                } else {
                    subQuestionComplexity = Math.Max(0, subQuestionComplexity - 1);
                    PlayerGameDataController.Instance.SubtractionQuestionComplexity = subQuestionComplexity;
                }
            break;
            case Subject.Multiplication:
                if (correct) {
                    multQuestionComplexity = Math.Min(9, multQuestionComplexity + 1);
                    PlayerGameDataController.Instance.MultiplicationQuestionComplexity = multQuestionComplexity;
                } else {
                    multQuestionComplexity = Math.Max(0, multQuestionComplexity - 1);
                    PlayerGameDataController.Instance.MultiplicationQuestionComplexity = multQuestionComplexity;
                }
            break;                
            case Subject.Division:
                if (correct) {
                    divQuestionComplexity = Math.Min(9, divQuestionComplexity + 1);
                    PlayerGameDataController.Instance.DivisionQuestionComplexity = divQuestionComplexity;
                } else {
                    divQuestionComplexity = Math.Max(0, divQuestionComplexity - 1);
                    PlayerGameDataController.Instance.DivisionQuestionComplexity = divQuestionComplexity;
                }
            break;
        }
    }

    public double GetCurrQuestionSol() {
        return currQuestionSol;
    }

    public void SetCurrQuestionSol(double sol) {
        currQuestionSol = sol;
    }

    public void SetSpawnPoint(Vector3 newSpawn) {
        playerSpawnPoint = newSpawn;
    }

    public List<int> GetCurrEnemyStrengths()
    {
        return currEnemyStrengths;
    }

    public void SetCurrEnemyStrengths(List<int> enemyStrengths) {
        currEnemyStrengths = enemyStrengths;
    }

    public void SetCurrSubject(Subject subject) {
        currSubject = subject;
    }

    public int GetCorrectStreak(){
        return correctStreak;
    }

    //-------------------------------------------------------------------
    public int GetCurrentCharacter()
    {
        return currentCharacter;
    }
}
