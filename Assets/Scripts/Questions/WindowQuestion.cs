using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// WindowQuestions are simple applied math problems that use assessing enemy strength to represent arithmetic problems.
public class WindowQuestion : BaseQuestion
{
    private GameManager gameManager;
    // The number and strength of enemies associated with this question
    private List<int> enemyStrengths = new List<int>();
    // The number of enemies (and hence, the number of splits) for this question
    private int numEnemies;
    // Ensures the solution is divisible by solutionFactor to help scale question complexity
    private int addSolutionFactor = 1;
    private int multSolutionFactor = 1;

    // 0-9 scale that describes how likely a solution is to break down into complex parts that require carrying
    private int questionComplexity = 0;

    // How many right or wrong questions warrant a bump or decrease in diffiuclty?
    private int wrongAnswerThreshold = 2;
    private int rightAnswerThreshold = 2;
    private int correctStreak = 0;
    private int incorrectStreak = 0;

    // Does the question require the player to carry digits?
    private  bool noCarry = true;

    // The minimum and maximum number of enemies for the question on the current diffiuclty and settings
    private int minEnemies = 2;
    private int maxEnemies = 3;

    // The difficulty of the question
    private Difficulty difficulty = Difficulty.Easy;

    // Console testing
    public int testGenerateDifficultyAddition = -1;
    public int testGeneratDifficultyMultiplication = -1;
    public int testGeneratDifficultySubtraction = -1;

    void Start()
    {
        // Find the game manager script
        GameObject gameManagerObject = GameObject.Find("Game Manager"); 
        if (gameManagerObject) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        } else {
            // Error
        }

        subject = subject == null ? Subject.Addition : subject;
        difficulty = gameManager.GetDifficulty();
        if (gameManager.GetQuestionComplexity() == null) {
            SetInitialComplexity();
        } else {
            questionComplexity = gameManager.GetQuestionComplexity();
            SetParameters();
        }

        //GenerateQuestion();
    }

    void Update()
    {
        // Console testing
        if (testGenerateDifficultyAddition >= 0) {
            subject = Subject.Addition;
            difficulty = (Difficulty) testGenerateDifficultyAddition;
            SetInitialComplexity();
            GenerateQuestion();
            testGenerateDifficultyAddition = -1;
        } else if (testGeneratDifficultyMultiplication >= 0) {
            subject = Subject.Multiplication;
            difficulty = (Difficulty) testGeneratDifficultyMultiplication;
            SetInitialComplexity();
            GenerateQuestion();
            testGeneratDifficultyMultiplication = -1;
        }   else if (testGeneratDifficultySubtraction >= 0) {
            subject = Subject.Subtraction;
            difficulty = (Difficulty) testGeneratDifficultySubtraction;
            SetInitialComplexity();
            GenerateQuestion();
            testGeneratDifficultySubtraction = -1;
        }
    }

    // Did the player correctly calculate the solution? 
    public override bool IsCorrect(double sol)
    {
        if (sol == solution) {
            correctStreak++;
            incorrectStreak = 0;

            // Increases complexity for future problems if the player is easily answering questions
            if (correctStreak >= rightAnswerThreshold) {
                questionComplexity = Math.Min(9, questionComplexity + 1);
                gameManager.SetQuestionComplexity(questionComplexity);
            }

            return true;
        }

        correctStreak = 0;
        incorrectStreak++;

        // Decreases complexity for future problems if the player is struggling
        if (incorrectStreak >= wrongAnswerThreshold) {
            questionComplexity = Math.Max(0, questionComplexity - 1);
            gameManager.SetQuestionComplexity(questionComplexity);
        }

        return false;
    }

    // Sets data from the GameManager if not set in Start, then generates a question
    public void GenerateInitialQuestion() {
        // Find the game manager script
        GameObject gameManagerObject = GameObject.Find("Game Manager"); 
        if (gameManagerObject) {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        } else {
            // Error
        }

        subject = subject == null ? Subject.Addition : subject;
        difficulty = gameManager.GetDifficulty();
        if (gameManager.GetQuestionComplexity() == null) {
            SetInitialComplexity();
        } else {
            questionComplexity = gameManager.GetQuestionComplexity();
            SetParameters();
        }

        GenerateQuestion();
    }

    public override void GenerateQuestion() {   
        numEnemies = UnityEngine.Random.Range(minEnemies, maxEnemies + 1);
        
        enemyStrengths.Clear();

        switch(subject) {
            case Subject.Addition:
                GenerateAdditionQuestion();
                break;
            case Subject.Subtraction:
                GenerateSubtractionQuestion();
                break;
            case Subject.Multiplication:
                GenerateMultiplicationQuestion();
                break;
            case Subject.Division:
                GenerateDivisionQuestion();
                break;
            default:
                GenerateAdditionQuestion();
                break;
        }

        gameManager.SetCurrQuestionSol(solution);
        gameManager.SetCurrEnemyStrengths(enemyStrengths);

        // Console testing
        // Debug.Log(difficulty + " Solution: " + solution + "\nEnemies: ");
        // foreach (int enemyStrength in enemyStrengths) {
        //    Debug.Log(enemyStrength);
        // }
    }

    // All enemies have different strengths, must be added together
    private void GenerateAdditionQuestion() {
        solution = addSolutionFactor * UnityEngine.Random.Range(1, 10*(questionComplexity + 1));
        
        if (noCarry && solution % 10 < 3) {
            solution += 3;
        }

        int remainingSolution = (int) solution;

        // Generate individual enemy strengths and store the results
        for (int i = numEnemies; i > 1; i--) {
            int enemyStrength;
            if (noCarry) {
                int ones = UnityEngine.Random.Range(1, (remainingSolution % 10) - (i - 1));
                int tens = 10 * UnityEngine.Random.Range(0, (remainingSolution / 10));
                enemyStrength = ones + tens;
                Debug.Log(tens + " + " + ones);
            } else {
                enemyStrength = UnityEngine.Random.Range(1, remainingSolution - (i - 1));
            }
            enemyStrengths.Add(enemyStrength);
            remainingSolution -= enemyStrength;
        }
        enemyStrengths.Add(remainingSolution);
    }

    // There is one large enemy and the other enemeis subtract from it
    private void GenerateSubtractionQuestion() {
        // The strength of the large enemy
        int largeEnemy = addSolutionFactor * UnityEngine.Random.Range(1, 10*(questionComplexity + 1));

        if (noCarry && largeEnemy % 10 < 3) {
            solution += 3;
        }

        enemyStrengths.Add(largeEnemy);

        int remainingLargeEnemy = largeEnemy;

        // Generate individual enemy strengths and store the results
        for (int i = numEnemies; i > 1; i--) {
            int enemyStrength;
            if (noCarry) {
                int ones = UnityEngine.Random.Range(1, (remainingLargeEnemy % 10) - (i - 1));
                int tens = 10 * UnityEngine.Random.Range(0, (remainingLargeEnemy / 10));
                enemyStrength = ones + tens;
                Debug.Log(tens + " + " + ones);
            } else {
                enemyStrength = UnityEngine.Random.Range(1, remainingLargeEnemy - (i - 1));
            }
            enemyStrengths.Add(enemyStrength);
            remainingLargeEnemy -= enemyStrength;
        }

        // After all of the small enemies have been subtrated, what remains is the solution
        solution = remainingLargeEnemy;
    }

    // All enemies have the same strength and can be added or multiplied, multiplication is faster
    private void GenerateMultiplicationQuestion() {
        if (noCarry) {
            GenerateNoCarryMultiplicationQuestion();
        } else {
            solution = multSolutionFactor * numEnemies * UnityEngine.Random.Range(1, 10);

            int enemyStrength = (int) solution / numEnemies;

            for (int i = 0; i < numEnemies; i++) {
                enemyStrengths.Add(enemyStrength);
            }
        }
    }

    // Multiplication without carrying must follow one of a few possible layouts
    private void GenerateNoCarryMultiplicationQuestion() {
        numEnemies = UnityEngine.Random.Range(2, 5);
        int enemyStrength;

        if (numEnemies == 4) {
            enemyStrength = 2;
        } else if (numEnemies == 3) {
            enemyStrength = UnityEngine.Random.Range(2, 4);
        } else {
            enemyStrength = UnityEngine.Random.Range(2, 5);
        }

        solution = enemyStrength * numEnemies;

        for (int i = 0; i < numEnemies; i++) {
            enemyStrengths.Add(enemyStrength);
        }
    }

    // How does this work?
    private void GenerateDivisionQuestion() {

    }

    // Set initial question complexity and based on difficulty and adjust variables based on that complexity
    private void SetInitialComplexity () {
        switch (difficulty) {
            case Difficulty.Easy:
                questionComplexity = 0;
                break;
            case Difficulty.Medium:
                questionComplexity = 3;
                break;
            case Difficulty.Hard:
                questionComplexity = 7;
                break;
        }
        SetParameters();
    }

    // Adjusts solution generation based on complexity
    private void SetParameters() {
        // Easy questions have few (1-3) enemies, small and round solutions (ex. 10), and require no carrying
        if (questionComplexity < 3) {
            minEnemies = 2;
            maxEnemies = 3;
            addSolutionFactor = 3;
            multSolutionFactor = 1;
            noCarry = true;
        } 
        // Medium questions (complexity 3-5) have a medium number of enemies (3-5), large or complex solutions (ex. 17 or 100), and may require carrying
        else if (questionComplexity < 7) {
            minEnemies = 3;
            maxEnemies = (questionComplexity / 2) + 2;
            addSolutionFactor = 10;
            multSolutionFactor = 2;
            noCarry = false;
        }
        // Hard questions have a large number of enemies (5+), large and complex solutions (ex. 117), and require carrying 
        else {
            minEnemies = 3;
            maxEnemies = questionComplexity - 2;
            addSolutionFactor = UnityEngine.Random.Range(6, 11);
            multSolutionFactor = 3;
            noCarry = false;
        }
    }

    // GETTERS AND SETTERS
    public void SetSubject(Subject sub) {
        subject = sub;
    }

    public List<int> GetEnemyStrengths() {
        return enemyStrengths;
    }

    public double Solution() {
        return solution;
    }
}
