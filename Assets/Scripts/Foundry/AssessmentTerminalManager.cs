using System;
using System.Collections;
using TMPro;
using UnityEngine;

// This class controls the terminal which the player interacts with to bring up the window question
public class AssessmentTerminalManager : BaseInteractable
{

    public AudioClip correctSound;
    private AudioSource audioSource;

    [SerializeField] private GameObject surveillanceCam;  // The camera of the window question
    [SerializeField] private TMP_InputField inputField;  // The input field of the window question
    // The wall between this terminal and the foundry
    // It is removed after the window question is finished
    [SerializeField] private GameObject separatorWall;
    [SerializeField] private WindowQuestion windowQuestion;  // The window question

    private GameObject playerRef;  // A reference to the player

    [SerializeField] private GameObject wrongAnswerOverlay; // Give player feedback

    private bool open; // Verify if the assessment terminal is actually open, for fixing a bug that double submits the question

    void Awake()
    {
        playerRef = GameObject.Find("Player");

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = correctSound;

        open = false;
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
        open = true;

        // Start transition blackscreen
        TransitionUIManager._instance.StartTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetStartTransitionSpan());

        // Switch to the surveillance camera view
        playerRef.SetActive(false);
        surveillanceCam.SetActive(true);

        // Prepare UI
        Cursor.lockState = CursorLockMode.None;
        inputField.Select();
        inputField.ActivateInputField();

        // Remove transition blackscreen
        TransitionUIManager._instance.EndTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetEndTransitionSpan());
    }

    IEnumerator QuitAssessmentChallenge()
    {
        open = false;
        audioSource.Play();

        // Start transition blackscreen
        TransitionUIManager._instance.StartTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetStartTransitionSpan());

        // Switch to the player camera view
        surveillanceCam.SetActive(false);
        playerRef.SetActive(true);

        // Resume UI settings
        Cursor.lockState = CursorLockMode.Locked;

        // Remove the wall to reveal the foundry
        separatorWall.SetActive(false);

        // Remove transition blackscreen
        TransitionUIManager._instance.EndTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetEndTransitionSpan());

    }

    // Check question
    public void OnSubmit()
    {
        // Check if assessment terminal is open.
        // This prevents accidental submission after the assessment is completed,
        // like if the input field is still selected and then is deselected by clicking on something else
        if(open){
            // Prevents accidental submission like clicking off of the input field before typing an answer
            if(inputField.text != ""){
                int ans = Int32.Parse(inputField.text);

                // Correct answer
                if (windowQuestion.IsCorrect(ans))
                {
                    StartCoroutine("QuitAssessmentChallenge");
                }
                // Incorrect answer
                else
                {
                    inputField.text = "";
                    wrongAnswerOverlay.GetComponent<WrongAnswerFeedback>().WrongAnswerUI();
                    inputField.Select();
                    inputField.ActivateInputField();
                }
            }
            
        }
        
    }
}
