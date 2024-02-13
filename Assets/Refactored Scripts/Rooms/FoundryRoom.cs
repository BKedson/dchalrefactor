using UnityEngine;

public class FoundryRoom : BaseRoom
{
    [SerializeField] private FoundryManager foundryManager;
    [SerializeField] private Animator door1Animator;
    [SerializeField] private Animator door2Animator;

    private bool doorOpened = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!doorOpened) door1Animator.SetBool("Door Opened", foundryManager.weaponForged);

        doorOpened = foundryManager.weaponForged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door1Animator.SetBool("Door Opened", false);
            door2Animator.SetBool("Door Opened", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            door1Animator.SetBool("Door Opened", false);
            door2Animator.SetBool("Door Opened", false);
        }
    }
}
