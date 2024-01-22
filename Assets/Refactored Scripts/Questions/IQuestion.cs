using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IQuestion interface is the main abstraction for math problems across the different challenges.
public interface IQuestion
{
    // Checks the solution against the input
    bool IsCorrect(double sol);

    // Generates a new question of this type
    void GenerateQuestion();

}
