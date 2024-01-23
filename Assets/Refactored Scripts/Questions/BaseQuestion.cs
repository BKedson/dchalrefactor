using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IQuestion interface.
// BaseQuestion should be extended by the different question types.
public abstract class BaseQuestion : MonoBehaviour, IQuestion
{
    // The subject for this question
    protected internal Subject subject;

    // The solution for this Question
    protected internal double solution;

    // How difficult the question should be
    protected internal int questionLevel;

    public abstract bool IsCorrect(double sol);

    internal abstract void GenerateQuestion();

    void IQuestion.GenerateQuestion()
    {

    }
}
