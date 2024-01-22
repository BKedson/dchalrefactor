using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IChallenge interface is the main abstraction for challenges.
// Challenges handle players interacting with objects that have questions associated with them.
public interface IChallenge
{
    // Begins the challenge
    void Begin();

    // Defines behavior for when the player fails the challenge
    void Fail();

    // Aborts the challenge
    void Abort();

    // Defines behavior for when the challenge is completed successfully
    void Complete();
}
