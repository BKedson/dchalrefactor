using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The IController interface defines behavior for both challenge and logic controllers.
public interface IController
{
    // Executes an action based on input
    void TakeAction(Action action);

    // Maps the input recieved by the controller to the correct action
    void InputToAction();

}
