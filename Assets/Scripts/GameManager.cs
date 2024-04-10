using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    bool canChangeControls = true;
    Difficulty globalDifficulty = Difficulty.Easy;
    private int questionComplexity = 0;

    // The solution and list of enemy strengths for the current question
    private double currQuestionSol;
    public List<int> currEnemyStrengths;

    private void Awake()
    {
        if(manager) {
            DestroyImmediate(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // GETTERS AND SETTERS

    public void ChangeDiffuclty(Difficulty difficulty) {
        globalDifficulty = difficulty;
        PlayerPrefs.SetInt("difficulty", (int)globalDifficulty);
    }

    public Difficulty GetDifficulty(){
        globalDifficulty = (Difficulty) PlayerPrefs.GetInt("difficulty");
        return globalDifficulty;
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

    public void SetQuestionComplexity(int complexity) {
        questionComplexity = complexity;
    }

    public double GetCurrQuestionSol() {
        return currQuestionSol;
    }

    public void SetCurrQuestionSol(double sol) {
        currQuestionSol = sol;
    }

    public List<int> GetCurrEnemyStrengths() {
        return currEnemyStrengths;
    }

    public void SetCurrEnemyStrengths(List<int> enemyStrengths) {
        currEnemyStrengths = enemyStrengths;
    }
}
