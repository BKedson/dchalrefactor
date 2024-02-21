using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WindowController))]
// The terminal window the player uses to calculate the number of enemies in the next room.
public class Terminal : BaseInteractable
{
    private WindowController winController;

    // Start is called before the first frame update
    void Start()
    {
        winController = GetComponent<WindowController>();
    }

    public override void OnInteract()
    {
        winController.InputToAction();
    }
}
