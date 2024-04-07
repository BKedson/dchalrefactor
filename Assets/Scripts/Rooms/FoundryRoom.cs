using UnityEngine;

public class FoundryRoom : BaseRoom
{
    [SerializeField] private FoundryManager foundryManager;
    [SerializeField] private Animator door1Animator;
    [SerializeField] private Animator door2Animator;
    [SerializeField] private GameObject surveillanceCam;

    private bool doorOpened = false;

    // Start is called before the first frame update
    void Awake()
    {

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

    public void OnWeaponForged()
    {
        door1Animator.SetBool("Door Opened", true);
    }
}
