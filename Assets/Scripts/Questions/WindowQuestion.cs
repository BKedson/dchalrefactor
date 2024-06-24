using System;
using System.Collections.Generic;
using UnityEngine;

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
    private int divSolutionFactorMax = 1;

    // 0-9 scale that describes how likely a solution is to break down into complex parts that require carrying
    private int questionComplexity = 0;

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

    //for generating intake question
    private int foundrySolution = -1;

    // Are we making a tutorial question?
    [SerializeField] private bool tutorial;

    // Track last generated divisor for division questions
    private int lastDivisor = 1;

    // Keep track of subject settings
    public int subjectSetting;

    void Start()
    {
        // Find the game manager script
        if (GameManager.manager) {
            gameManager = GameManager.manager;
        } else {
            // Error
        }

        subjectSetting = gameManager.GetSubjectSetting();
        
        switch(subjectSetting){
            case 1:
                subject = Subject.Subtraction;
                break;
            case 2: 
                subject = Subject.Multiplication;
                break;
            case 3:
                subject = Subject.Division;
                break;
            default:
                subject = Subject.Addition;
                break;
        }

        switch(subject) {
                case Subject.Addition:
                    difficulty = gameManager.GetAddDifficulty();
                    break;
                case Subject.Subtraction:
                    difficulty = gameManager.GetSubtractDifficulty();
                    break;
                case Subject.Multiplication:
                    difficulty = gameManager.GetMultiplyDifficulty();
                    break;
                case Subject.Division:
                    difficulty = gameManager.GetDivideDifficulty();
                    break;
                default:
                    difficulty = gameManager.GetAddDifficulty();
                    break;
        }

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
        // if (testGenerateDifficultyAddition >= 0) {
        //     subject = Subject.Addition;
        //     difficulty = (Difficulty) testGenerateDifficultyAddition;
        //     SetInitialComplexity();
        //     GenerateQuestion();
        //     testGenerateDifficultyAddition = -1;
        // } else if (testGeneratDifficultyMultiplication >= 0) {
        //     subject = Subject.Multiplication;
        //     difficulty = (Difficulty) testGeneratDifficultyMultiplication;
        //     SetInitialComplexity();
        //     GenerateQuestion();
        //     testGeneratDifficultyMultiplication = -1;
        // }   else if (testGeneratDifficultySubtraction >= 0) {
        //     subject = Subject.Subtraction;
        //     difficulty = (Difficulty) testGeneratDifficultySubtraction;
        //     SetInitialComplexity();
        //     GenerateQuestion();
        //     testGeneratDifficultySubtraction = -1;
        // }
    }

    // Did the player correctly calculate the solution? 
    public override bool IsCorrect(double sol)
    {
        if (sol == solution) {
            gameManager.RightAnswer();
            return true;
        }

        gameManager.WrongAnswer();
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

        subjectSetting = gameManager.GetSubjectSetting();
        
        switch(subjectSetting){
            case 1:
                subject = Subject.Subtraction;
                break;
            case 2: 
                subject = Subject.Multiplication;
                break;
            case 3:
                subject = Subject.Division;
                break;
            default:
                subject = Subject.Addition;
                break;
        }

        switch(subject) {
                case Subject.Addition:
                    difficulty = gameManager.GetAddDifficulty();
                    break;
                case Subject.Subtraction:
                    difficulty = gameManager.GetSubtractDifficulty();
                    break;
                case Subject.Multiplication:
                    difficulty = gameManager.GetMultiplyDifficulty();
                    break;
                case Subject.Division:
                    difficulty = gameManager.GetDivideDifficulty();
                    break;
                default:
                    difficulty = gameManager.GetAddDifficulty();
                    break;
        }
        
        if (gameManager.GetQuestionComplexity() == null) {
            SetInitialComplexity();
        } else {
            questionComplexity = gameManager.GetQuestionComplexity();
            SetParameters();
        }

        GenerateQuestion();
    }

    public void GenerateIntakeQuestion(int sol){
        foundrySolution = sol;
        GenerateQuestion();
    }

    public override void GenerateQuestion() {   
        //retain window numEnemies if generating foundry question, otherwise generate new numEnemies
        if (foundrySolution <= 0){
            numEnemies = UnityEngine.Random.Range(minEnemies, maxEnemies + 1);
        }
        
        
        enemyStrengths.Clear();

        if (tutorial) {
            foundrySolution = 3;
            noCarry = true;
            numEnemies = 2;
            GenerateAdditionQuestion();
        } else {
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
        if (foundrySolution > 0) {
            solution = foundrySolution;
        } else {
            solution = addSolutionFactor * UnityEngine.Random.Range(1, 10*(questionComplexity + 1));
        }
        
        
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
                //Debug.Log(tens + " + " + ones);
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

        //if solution is given, generate large enemy then subtract the solution and generate remaining subtracting enemies
        if(foundrySolution > 0){
            solution = foundrySolution;

            //generate question based on solution

            // The strength of the large enemy
            int largeEnemy = addSolutionFactor * UnityEngine.Random.Range((int)solution, 10*(questionComplexity + 1));

            enemyStrengths.Add(largeEnemy);

            int remainingLargeEnemy = largeEnemy - (int)solution;

            // Generate all but one enemy strengths and store the results
            for (int i = numEnemies; i > 2; i--) {
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

            //make last enemy remaining amount to get to the solution

            enemyStrengths.Add(remainingLargeEnemy);

        //otherwise, generate large enemy and subtracting enemies before setting the solution to the remainder
        } else {
            // The strength of the large enemy
            int largeEnemy = addSolutionFactor * UnityEngine.Random.Range(1, 10*(questionComplexity + 1));

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
                enemyStrengths.Add(-1*enemyStrength);
                remainingLargeEnemy -= enemyStrength;
            }

            // After all of the small enemies have been subtrated, what remains is the solution
            solution = remainingLargeEnemy;
        }
    }

    // All enemies have the same strength and can be added or multiplied, multiplication is faster
    private void GenerateMultiplicationQuestion() {
        if (noCarry) {
            GenerateNoCarryMultiplicationQuestion();
        } else {
            //this will simply generate the same multiplication question as in the window, unsure if that is intended
            if (foundrySolution > 0){
                solution = foundrySolution;

                int enemyStrength = (int) solution / numEnemies;

                enemyStrengths.Add(enemyStrength);
                enemyStrengths.Add(numEnemies);
            } else {
                solution = multSolutionFactor * numEnemies * UnityEngine.Random.Range(1, 10);

                int enemyStrength = (int) solution / numEnemies;
                
                for (int i = 0; i < numEnemies; i++) {
                    enemyStrengths.Add(enemyStrength);
                }
            }

            
        }
    }

    // Multiplication without carrying must follow one of a few possible layouts
    private void GenerateNoCarryMultiplicationQuestion() {
        
        int enemyStrength;

        //generate question based on given answer
        if(foundrySolution > 0) {
            //this will simply generate the same multiplication question as in the window, unsure if that is intended
            solution = foundrySolution;
            enemyStrength = (int)(solution/numEnemies);

            enemyStrengths.Add(enemyStrength);
            enemyStrengths.Add(numEnemies);
            
        } else {
            numEnemies = UnityEngine.Random.Range(2, 5);

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
        

        
    }

    private void GenerateDivisionQuestion() {
        //for intake question, find valid divisor based on solution to window question
        if(foundrySolution > 0){
            solution = foundrySolution;
            enemyStrengths.Add((int)solution*lastDivisor);
            enemyStrengths.Add(lastDivisor);

        //for window question, generate addition question that is definitely divisible by a divisior
        }else{
            lastDivisor = UnityEngine.Random.Range(2, divSolutionFactorMax);
            solution = lastDivisor * UnityEngine.Random.Range(1, 10*(questionComplexity + 1));

            int remainingSolution = (int) solution;

            // Generate individual enemy strengths and store the results
            for (int i = numEnemies; i > 1; i--) {
                int enemyStrength;
                if (noCarry) {
                    int ones = UnityEngine.Random.Range(1, (remainingSolution % 10) - (i - 1));
                    int tens = 10 * UnityEngine.Random.Range(0, (remainingSolution / 10));
                    enemyStrength = ones + tens;
                    //Debug.Log(tens + " + " + ones);
                } else {
                    enemyStrength = UnityEngine.Random.Range(1, remainingSolution - (i - 1));
                }
                enemyStrengths.Add(enemyStrength);
                remainingSolution -= enemyStrength;
            }
            enemyStrengths.Add(remainingSolution);
        }
        
    }

    // Set initial question complexity and based on difficulty and adjust variables based on that complexity
    private void SetInitialComplexity () {
        // temporarily keep complexity low --> FIX THIS LATER **************************************************************************
        questionComplexity = 0;
        // switch (difficulty) {
        //     case Difficulty.Easy:
        //         questionComplexity = 0;
        //         break;
        //     case Difficulty.Medium:
        //         questionComplexity = 3;
        //         break;
        //     case Difficulty.Hard:
        //         questionComplexity = 7;
        //         break;
        // }
        SetParameters();
    }

    // Adjusts solution generation based on complexity
    private void SetParameters() {
        // Easy questions have few (1-3) enemies, small and round solutions (ex. 10), and require no carrying
        if (questionComplexity < 3) {
            minEnemies = 2;
            maxEnemies = Math.Max(2, questionComplexity + 1);
            addSolutionFactor = 3;
            multSolutionFactor = 1;
            divSolutionFactorMax = Math.Max(2, questionComplexity + 1);
            noCarry = true;
        } 
        // Medium questions (complexity 3-5) have a medium number of enemies (3-5), large or complex solutions (ex. 17 or 100), and may require carrying
        else if (questionComplexity < 7) {
            minEnemies = 3;
            maxEnemies = (questionComplexity + 1 ) / 2 + 1;
            addSolutionFactor = 2;
            multSolutionFactor = 2;
            divSolutionFactorMax = questionComplexity;
            noCarry = false;
        }
        // Hard questions have a large number of enemies (5+), large and complex solutions (ex. 117), and require carrying 
        else {
            minEnemies = 3;
            maxEnemies = 6;
            addSolutionFactor = UnityEngine.Random.Range(3, 7);
            divSolutionFactorMax = questionComplexity + 2;
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

    // public double GetSolution() {
    //     int ans = 0;
    //     foreach (int s in enemyStrengths) Debug.Log(s);
    //     foreach (int strength in enemyStrengths)
    //     {
    //         ans += strength;
    //     }
    //     return ans;
    // }

    public int GetSolution() {
        return (int)solution;
    }

    public void ToggleTutorial() {
        tutorial = !tutorial;
    }

    public bool Tutorial() {
        return tutorial;
    }
}
