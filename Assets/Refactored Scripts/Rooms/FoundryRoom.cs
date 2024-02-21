using System.Collections;
using UnityEngine;

public class FoundryRoom : BaseRoom
{
    [SerializeField] private FoundryManager foundryManager;
    [SerializeField] private Animator door1Animator;
    [SerializeField] private Animator door2Animator;
    [SerializeField] private GameObject surveillanceCam;

    private bool doorOpened = false;
    private GameObject playerRef;

    // Start is called before the first frame update
    void Awake()
    {
        playerRef = GameObject.Find("Player");
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

    public void OnElevatorArrival()
    {
        StartCoroutine("LoadWindowChallenge");
    }

    IEnumerator LoadWindowChallenge()
    {
        // Transition start
        yield return new WaitForSeconds(0);

        playerRef.SetActive(false);

        surveillanceCam.SetActive(true);

        // Transition end
    }

    public void OnWindowChallengeFinish()
    {
        StartCoroutine("QuitWindowChallenge");
    }

    IEnumerator QuitWindowChallenge()
    {
        // Transition start
        yield return new WaitForSeconds(0);

        surveillanceCam.SetActive(false);

        playerRef.SetActive(true);

        // Transition end
    }
}
