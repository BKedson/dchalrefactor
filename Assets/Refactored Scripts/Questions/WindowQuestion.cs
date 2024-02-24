using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WindowQuestions are simple applied math problems that use assessing enemy strength to represent arithmetic problems.
public class WindowQuestion : BaseQuestion
{
    private List<int> enemyStrengths;
    private int numEnemies = 1;
    private int minEnemies = 1;
    private int maxEnemies = 5;
    private int minEnemyStrength = 1;
    private int maxEnemyStrength = 9;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsCorrect(double sol)
    {
        return true;
    }


    internal override void GenerateQuestion()
    {
        numEnemies = Random.Range(minEnemies, maxEnemies + 1);

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

        Debug.Log(solution);
    }

    // All enemies have different strengths, must be added together
    private void GenerateAdditionQuestion() {
        // Generate individual enemy strengths and store the results
        for (int i = 0; i < numEnemies; i++) {
            enemyStrengths.Add(Random.Range(minEnemyStrength, maxEnemyStrength + 1));
        }

        foreach (int enemyStrength in enemyStrengths) {
            solution += enemyStrength;
        }
    }

    // How does this work?
    private void GenerateSubtractionQuestion() {
    }

    // All enemies have the same strength and can be added or multiplied, multiplication is faster
    private void GenerateMultiplicationQuestion() {
        int enemyStrength = Random.Range(minEnemyStrength, maxEnemyStrength + 1);

        for (int i = 0; i < numEnemies; i++) {
            enemyStrengths.Add(enemyStrength);
        }

        solution = enemyStrength * numEnemies;
    }

    // How does this work?
    private void GenerateDivisionQuestion() {

    }
}
