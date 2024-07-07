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
    private int maxComplexity = 6;
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
    //0 for add, 1 for subtract, 2 for multiply, 3 for divide, -1 for random
    private int subjectSetting = 0;

    // The solution and list of enemy strengths for the current question
    private double currQuestionSol;
    public List<int> currEnemyStrengths;

    // Track player invincibility
    [SerializeField] private bool isInvincible;

    // Track player cursor size
    private int cursorSize;
    private CursorBehavior playerCursor = null;

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
        //update character index in the manager
        UpdateCurrentCharacter();
    }

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = PlayerGameDataController.Instance.IsInvincible;
        cursorSize = PlayerGameDataController.Instance.CursorSize;
        addQuestionComplexity = PlayerGameDataController.Instance.AdditionQuestionComplexity;
        subQuestionComplexity = PlayerGameDataController.Instance.SubtractionQuestionComplexity;
        multQuestionComplexity = PlayerGameDataController.Instance.MultiplicationQuestionComplexity;
        divQuestionComplexity = PlayerGameDataController.Instance.DivisionQuestionComplexity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Resets everything marked as DoNotDestroy
    public void Restart(){
        Time.timeScale = 1;

        player = GameObject.Find("Player");

        if (player) {
            UpdateCurrentCharacter();
            EnablePlayerSound();
            // player.GetComponentInChildren<PlayerWeapons>().DeactivateAllWeapons();
            player.GetComponent<PlayerCharacter>().Reset();
            player.GetComponent<PlayerCharacter>().InitializePlayer();
            player.GetComponent<PlayerMovement>().Reset();
            player.GetComponent<PlayerCollectibles>().GetActiveCharacterWeapons().DeactivateAllWeapons();
        }

        if (DungeonGenerator._instance) {
            DungeonGenerator._instance.ResetLv();
        } else {
            ResetPlayerPos();
        }
    }

    public void NewGame() {
        correctStreak = 0;
        incorrectStreak = 0;

    }

    public void ResetPlayerPos() {
        player = GameObject.Find("Player");

        if (player) {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position =  new Vector3(0f, 0.2f, 2f);
            player.GetComponent<MouseLook>().FaceForward();
            // Re-enable CharacterController
            player.GetComponent<CharacterController>().enabled = true;
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
                    addQuestionComplexity = Math.Min(maxComplexity, addQuestionComplexity + 1);
                    PlayerGameDataController.Instance.AdditionQuestionComplexity = addQuestionComplexity;
                } else {
                    addQuestionComplexity = Math.Max(0, addQuestionComplexity - 1);
                    PlayerGameDataController.Instance.AdditionQuestionComplexity = addQuestionComplexity;
                }
            break;
            case Subject.Subtraction:
                if (correct) {
                    subQuestionComplexity = Math.Min(maxComplexity, subQuestionComplexity + 1);
                    PlayerGameDataController.Instance.SubtractionQuestionComplexity = subQuestionComplexity;
                } else {
                    subQuestionComplexity = Math.Max(0, subQuestionComplexity - 1);
                    PlayerGameDataController.Instance.SubtractionQuestionComplexity = subQuestionComplexity;
                }
            break;
            case Subject.Multiplication:
                if (correct) {
                    multQuestionComplexity = Math.Min(maxComplexity, multQuestionComplexity + 1);
                    PlayerGameDataController.Instance.MultiplicationQuestionComplexity = multQuestionComplexity;
                } else {
                    multQuestionComplexity = Math.Max(0, multQuestionComplexity - 1);
                    PlayerGameDataController.Instance.MultiplicationQuestionComplexity = multQuestionComplexity;
                }
            break;                
            case Subject.Division:
                if (correct) {
                    divQuestionComplexity = Math.Min(maxComplexity, divQuestionComplexity + 1);
                    PlayerGameDataController.Instance.DivisionQuestionComplexity = divQuestionComplexity;
                } else {
                    divQuestionComplexity = Math.Max(0, divQuestionComplexity - 1);
                    PlayerGameDataController.Instance.DivisionQuestionComplexity = divQuestionComplexity;
                }
            break;
        }
    }

        // Resets complexities to match current difficulty settings
    public void ResetComplexity() {
        ResetAddComplexity();
        ResetSubComplexity();
        ResetMultComplexity();
        ResetDivComplexity();
    }

    private void ResetAddComplexity() {
        switch (addDifficulty) {
            case Difficulty.Easy:
                addQuestionComplexity = 0;
                break;
            case Difficulty.Medium:
                addQuestionComplexity = 3;
                break;
            case Difficulty.Hard:
                addQuestionComplexity = maxComplexity - 2;
                break;
        }
        PlayerGameDataController.Instance.AdditionQuestionComplexity = addQuestionComplexity;
    }
    private void ResetSubComplexity() {
        switch (subtractDifficulty) {
            case Difficulty.Easy:
                subQuestionComplexity = 0;
                break;
            case Difficulty.Medium:
                subQuestionComplexity = 3;
                break;
            case Difficulty.Hard:
                subQuestionComplexity = maxComplexity - 2;
                break;
        }
        PlayerGameDataController.Instance.SubtractionQuestionComplexity = subQuestionComplexity;
    }
    private void ResetMultComplexity() {
        switch (multiplyDifficulty) {
            case Difficulty.Easy:
                multQuestionComplexity = 0;
                break;
            case Difficulty.Medium:
                multQuestionComplexity = 3;
                break;
            case Difficulty.Hard:
                multQuestionComplexity = maxComplexity - 2;
                break;
        }
        PlayerGameDataController.Instance.MultiplicationQuestionComplexity = multQuestionComplexity;
    }
    private void ResetDivComplexity() {
        switch (divideDifficulty) {
            case Difficulty.Easy:
                divQuestionComplexity = 0;
                break;
            case Difficulty.Medium:
                divQuestionComplexity = 3;
                break;
            case Difficulty.Hard:
                divQuestionComplexity = maxComplexity - 2;
                break;
        }
        PlayerGameDataController.Instance.DivisionQuestionComplexity = divQuestionComplexity;
    }

    public void DisablePlayerSound() {
        player = GameObject.Find("Player");

        if (player) {
            player.GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    public void EnablePlayerSound() {
        player = GameObject.Find("Player");

        if (player) {
            player.GetComponentInChildren<AudioListener>().enabled = true;
        }
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
        isInvincible = inv;
        PlayerGameDataController.Instance.IsInvincible = isInvincible;
    }

    public bool IsInvincible(){
        return isInvincible;
    }

    public void SetCursorSize(int size){
        cursorSize = size;
        PlayerGameDataController.Instance.CursorSize = cursorSize;
        if(playerCursor != null){
            playerCursor.UpdateCursor(cursorSize);
        }
    }

    public int GetCursorSize(){
        return cursorSize;
    }

    public void SetPlayerCursor(CursorBehavior cursor){
        playerCursor = cursor;
    }

    public void SetMusicVolume(float vol){
        PlayerGameDataController.Instance.MusicVolume = vol;
    }

    public void SetSFXVolume(float vol){
        PlayerGameDataController.Instance.SFXVolume = vol;
    }

    public float GetMusicVolume(){
        return PlayerGameDataController.Instance.MusicVolume;
    }

    public float GetSFXVolume(){
        return PlayerGameDataController.Instance.SFXVolume;
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

    public int GetSubjectSetting() {
        return subjectSetting;
    }

    public void SetSubjectSetting(int subject) {
        subjectSetting = subject;
        Debug.Log("Subject setting: " + subject);
    }

    public int GetCorrectStreak(){
        return correctStreak;
    }

    //-------------------------------------------------------------------
    public int GetCurrentCharacter()
    {
        return currentCharacter;
    }

    public void UpdateCurrentCharacter()
    {
        currentCharacter = globalCharacterData.RetrieveCharacterIndex(PlayerGameDataController.Instance.CurrentCharacter);
        player = GameObject.Find("Player");

        if (player) {
            player.GetComponent<PlayerMovement>().Reset();
        }
    }
}
