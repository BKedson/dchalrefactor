using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private string[] messages;
    private int numMessages;
    private int currMessage;
    private int section;
    private bool last;

    [SerializeField] private GameObject text;
    [SerializeField] private GameObject textBoxContainer;
    [SerializeField] private GameObject collapsedContainer;

    public AudioClip continueSound;
    private AudioSource audioSource;
    

    void Awake()
    {
        numMessages = messages.Length;
        currMessage = 0;
        section = 0;
        last = false;

        textBoxContainer.SetActive(true);
        collapsedContainer.SetActive(false);

        if (text) {
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = continueSound;
    }

    void Update()
    {
        TextCheck();

        if (Input.GetKeyDown("t")){
            audioSource.time = 0.20f;
            audioSource.Play();

            if (currMessage == numMessages - 1 || last == true) {
                Collapse();
            }
            else {
                currMessage++;
            }

        }

        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];

        if (currMessage == 1) {
            PlayerMoved();
        }
        if (currMessage == 2) {
            TerminalOpened();
        }
        if (currMessage == 3) {
            TerminalMoved();
        }

        /*
        if(textBoxContainer.activeSelf){

            if(Input.GetKeyDown("mouse 0")){
                if(section >= 7){
                    clickCount++;
                    if(clickCount > 1){
                        OnAttack();
                    }
                }
            }
        }
        */
        
        
    }

    public void Collapse() {
        textBoxContainer.SetActive(!textBoxContainer.activeSelf);
        collapsedContainer.SetActive(!collapsedContainer.activeSelf);
    }

    public void PlayerMoved(){
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d")) {
            currMessage = 2;
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            audioSource.Play();

            if (!textBoxContainer.activeSelf) {
                Collapse();
            }
        }
    }

    public void TextCheck() {
        if (currMessage == 1 || currMessage == 2 || currMessage == 3 || currMessage == 4) {
            last = true;
        }
        else {
            last = false;
        }
    }

    // TODO: create a key press generic method!

    public void TerminalOpened(){
        if(Input.GetKeyDown("e")) {
                currMessage = 3;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();

            if (!textBoxContainer.activeSelf) {
                Collapse();
            }
        }
        
    }

    public void TerminalMoved(){
        if(Input.GetKeyDown("a") || Input.GetKeyDown("d")) {
                currMessage = 4;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();

            if (!textBoxContainer.activeSelf) {
                Collapse();
            }
        }
        
    }

    /*

    public void TerminalCorrectlySubmitted(){
        if(textBoxContainer.activeSelf){
            if(section < 4){
                section = 4;
                currMessage = 6;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();
            }
        }
        
    }

    public void OrePlaced(){
        if(textBoxContainer.activeSelf){
            if(section < 5){
                section = 5;
                currMessage = 7;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();
            }
        }
        
    }

    public void IntakeCorrectlySubmitted(){
        if(textBoxContainer.activeSelf){
            if(section < 6){
                section = 6;
                currMessage = 10;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();
            }
        }
    }

    public void SwordEquipped(){
        if(textBoxContainer.activeSelf){
            if(section < 7){
                section = 7;
                currMessage = 11;
                text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
                audioSource.Play();
            }
        }
    }

    public void OnAttack(){
        section = 8;
        currMessage = 12;
        text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
        audioSource.Play();
    }

    public void CombatOver(){
        if(textBoxContainer.activeSelf){
            section = 9;
            currMessage = 13;
            text.GetComponent<TextMeshProUGUI>().text = messages[currMessage];
            StartCoroutine(WaitToDeactivate());
            audioSource.Play();
        }
    }

    private IEnumerator WaitToDeactivate(){
        yield return new WaitForSeconds(5.0f);
        
        //check that the tutorial hasn't already restarted 
        if(section == 9){
            textBoxContainer.SetActive(false);
        }
    }
    */

}
