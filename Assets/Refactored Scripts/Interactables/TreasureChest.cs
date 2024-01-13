using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A locked treasure chest that is only unlocked when the player completes a SimpleQuestion.
public class TreasureChest : BaseInteractable
{
    // The question the player must solve to unlock the door
    private SimpleQuestion question;
    private bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onInteract()
    {
        
    }
}
