using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FoundryChallenge defines the basic functionality for foundry challenges,
// which ask the player to construct a weapon/tool to match the total strength of the enemies in the combat room
public class FoundryChallenge : BaseChallenge
{
    // Start is called before the first frame update
    void Start()
    {
        question = new FoundryQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Abort()
    {

    }

    public override void Begin()
    {

    }

    public override void Complete()
    {

    }

    public override void Fail()
    {

    }
}
