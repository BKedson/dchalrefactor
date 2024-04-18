using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IQuestion interface.
// BaseQuestion should be extended by the different question types.
public abstract class BaseQuestion : MonoBehaviour, IQuestion
{
    // The subject for this question
    [SerializeField] public Subject subject { get; protected set; }

    // The solution for this Question
    protected internal double solution;

    // How difficult the question should be
    protected internal int questionLevel;

    public void SetSubject(Subject s) { subject = s; }

    public abstract bool IsCorrect(double sol);

    public abstract void GenerateQuestion();

    void IQuestion.GenerateQuestion()
    {

    }
}
