using UnityEngine;

// This script controls the overall behavior of a level - a pair of foundry room and combat area, and the connecting pathway
// It mainly focus on the foundry room. Logics related to the cambat area can be integrated into this script if necessary
// This script is added to the base transform of the "Foundry And Combat Room" prefab
// The associated trigger collider is placed on the pathway between the doors to close the first door before opening the second
public class FoundryRoom : BaseRoom
{
    [SerializeField] private FoundryManager foundryManager;  // The foundry in the foundry room
    [SerializeField] private Animator door1Animator;  // The animator of the first (closer to spawn) door of the pathway
    [SerializeField] private Animator door2Animator;  // The animator of the first (closer to combat area) door of the pathway
    [SerializeField] private GameObject surveillanceCam;  // The surveillance camera in the combat area
    // Someone added this here and I do not know what it is for  -- Nofer
    [SerializeField] private GameObject cameraHelper;

    //private bool doorOpened = false;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // When player moves past the first door, the first one is closed and the second one is opened
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door1Animator.SetBool("Door Opened", false);
            door2Animator.SetBool("Door Opened", true);
        }
    }

    // Close the second door after the player leaves the pathway
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            door1Animator.SetBool("Door Opened", false);
            door2Animator.SetBool("Door Opened", false);
        }
    }

    // Unlock the pathway when a weapon is successfully unlocked
    public void OnWeaponForged()
    {
        door1Animator.SetBool("Door Opened", true);
        cameraHelper.gameObject.SetActive(false);
    }
}
