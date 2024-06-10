using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using TMPro;
using System.Linq;

public class TutorialText : MonoBehaviour
{

    /*
    This script is an overhaul of the original TextboxBehavior script.
    This overhaul is designed to decrease code complexity and make the tutorial more intuitive.
    Eventually after switching to a scene-based foundry, there will be invisible boundaries
        along with the tutorial messages to avoid the player from skipping sections and prevent
        the need for the moevement script to be disabled.
    */

    //variables for keeping track of messages and which message is the last in each cycle
    [SerializeField] private string[] messages;
    private int numMessages;
    private int currMessage;
    private bool last;
    private int[] lastMessages = {1, 2, 3, 4, 5, 8, 9, 10, 11};

    //variables for the tutorial text and its open/collapsed container
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject textBoxContainer;
    [SerializeField] private GameObject collapsedContainer;

    //variables for text change audio cues
    public AudioClip continueSound;
    private AudioSource audioSource;

    //script variables for referencing certain player events under the Player
    public GameObject player;
    private PlayerMovement moveScript;
    private PlayerCollectibles collectScript;
    private CompletionSound completeScript;
    private PlayerCharacter deathScript;
    

    void Awake()
    {
        //setting up tutorial start
        numMessages = messages.Length;
        currMessage = 0;
        last = false;

        //automatically display tutorial text container
        textBoxContainer.SetActive(true);
        collapsedContainer.SetActive(false);

        if (text) {
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }

        //sound source assignment
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = continueSound;

        //all external script references and their assignment to variables
        moveScript = player.GetComponent<PlayerMovement>();
        moveScript.enabled = false;

        collectScript = player.GetComponent<PlayerCollectibles>();
        collectScript.OnCollection += OnCollection;

        completeScript = player.GetComponent<CompletionSound>();
        completeScript.OnCompletion += OnCompletion;

        deathScript = player.GetComponent<PlayerCharacter>();
        deathScript.OnDeath += OnDeath;
    }

    void Update()
    {
        //calls helper to check if a message is the last in its cycle
        TextCheck();

        if (currMessage == 10 || currMessage == 11) {
            StartCoroutine(WaitToDeactivate());
        }

        //handles t press interactions
        if (Input.GetKeyDown("t")){
            audioSource.time = 0.20f;
            audioSource.Play();

            // if the current message is the last in its cycle then don't load another message
            if (currMessage == numMessages - 1 || last == true) {
                Collapse();
            }
            else {
                currMessage++;
            }

        }

        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];

        //calls helper to check if a message should be activated based on in-game actions
        MessageCheck();
        
    }

    //helper to expand and collapse tutorial
    public void Collapse() {
        textBoxContainer.SetActive(!textBoxContainer.activeSelf);
        collapsedContainer.SetActive(!collapsedContainer.activeSelf);
    }

    //helper to read in if players have pressed certain key(s) and to update the tutorial
    //takes in key(s) and desired next message
    public void KeyInput(string[] keys, int nextMessage) {
        foreach (string key in keys) {
            if (Input.GetKeyDown(key) && moveScript.enabled == true) {
                NoInput(nextMessage);
                break;
            }
        }
    }

    //helper to update displayed message
    public void NoInput(int nextMessage) {
        currMessage = nextMessage;
        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        audioSource.Play();

        if (!textBoxContainer.activeSelf) {
            Collapse();
        }
    }

    //helper to check if a certain message is activated for the purpose of key presses or actions
    public void MessageCheck() {
        if (currMessage == 1) {
            KeyInput(new string[]{"w", "a", "s", "d"}, 2);
        }
        if (currMessage == 2) {
            KeyInput(new string[]{"e"}, 3);
        }
        if (currMessage == 3) {
            KeyInput(new string[]{"a", "d"}, 4);
        }
        if (currMessage == 4 && player.activeSelf) {
            NoInput(5);
        }
        if (currMessage == 5) {
            KeyInput(new string[]{"w"}, 6);
        }
    }

    //helper to check if the current message is the last in its cycle
    //player movement is disabled when it is not the last message to esnure players read
    public void TextCheck() {
        if (lastMessages.Contains(currMessage)) {
            last = true;
            moveScript.enabled = true;
        }
        else {
            last = false;
            moveScript.enabled = false;
        }
    }

    //series of event subscriptions to trigger certain tutorial messages
    private void OnCollection(string name) {
        NoInput(9);
    }

    private void OnCompletion() {
        NoInput(10);
    }

    private void OnDeath() {
        NoInput(11);
    }

    //helper to reset event subscriptions
    void OnDestroy() {
        collectScript.OnCollection -= OnCollection;
        completeScript.OnCompletion -= OnCompletion;
        deathScript.OnDeath -= OnDeath;

        /*
        moveScript = null;
        collectScript = null;
        completeScript = null;
        deathScript = null;
        */
    }

    //helper to deactivate the tutorial
    private IEnumerator WaitToDeactivate(){
        yield return new WaitForSeconds(8.0f);
        textBoxContainer.SetActive(false);
        collapsedContainer.SetActive(false);
    }

}
