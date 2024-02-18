using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WindowChallenge defines the basic functionality for Foundry window challenges,
// which give the player a window into the combat room and ask the player to assess enemy strength
public class WindowChallenge : BaseChallenge
{
    // Start is called before the first frame update
    void Start()
    {
        question = new WindowQuestion();
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
