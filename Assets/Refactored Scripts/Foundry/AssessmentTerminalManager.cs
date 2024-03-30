using System.Collections;
using UnityEngine;

public class AssessmentTerminalManager : BaseInteractable
{
    [SerializeField] private GameObject surveillanceCam;
    [SerializeField] private GameObject separatorWall;

    private GameObject playerRef;

    // Start is called before the first frame update
    void Awake()
    {
        playerRef = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnInteract()
    {
        StartCoroutine("StartAssessmentChallenge");
    }

    public void OnQuitAssessmentChallenge()
    {
        StartCoroutine("QuitAssessmentChallenge");
    }

    IEnumerator StartAssessmentChallenge()
    {
        // Transition start
        yield return new WaitForSeconds(0 /*To be changed*/);

        playerRef.SetActive(false);
        surveillanceCam.SetActive(true);

        // Transition end
        yield return new WaitForSeconds(0 /*To be changed*/);
    }

    IEnumerator QuitAssessmentChallenge()
    {
        // Transition start
        yield return new WaitForSeconds(0 /*To be changed*/);

        surveillanceCam.SetActive(false);
        playerRef.SetActive(true);

        // Transition end
        yield return new WaitForSeconds(0 /*To be changed*/);

        separatorWall.SetActive(false);
    }
}
