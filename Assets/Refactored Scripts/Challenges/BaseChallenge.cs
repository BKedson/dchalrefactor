using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An abstract class that implements the IChallenge interface.
// BaseChallenge should be extended by all objects that have challenges associated with them.
public abstract class BaseChallenge : MonoBehaviour, IChallenge
{
    // The question for this challenge
    protected internal BaseQuestion question;

    public abstract void Begin();
    public abstract void Fail();
    public abstract void Abort();
    public abstract void Complete();
}
