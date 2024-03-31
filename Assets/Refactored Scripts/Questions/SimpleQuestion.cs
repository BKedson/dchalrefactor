using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SimpleQuestions are short and clearly defined arithmetic problems that present simple equations to the player and ask them to solve.
public class SimpleQuestion : BaseQuestion
{
    double operand1;
    double operand2;

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

    public override void GenerateQuestion()
    {

    }
}
