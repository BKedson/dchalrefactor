using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowAnswerFeedback : MonoBehaviour
{
    [SerializeField] private GameObject[] layerOne;
    [SerializeField] private GameObject[] layerTwo;
    // Use layer three to temp deactivate other UI elements, including other feedback
    [SerializeField] private GameObject[] layerThree;
    //this is just to change the text, put the rest of the answer streak UI in layer one
    [SerializeField] private GameObject answerStreakText;
    private GameManager gameManager;

    void Awake(){
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void WrongAnswerUI(){
        StartCoroutine(FlashingSequence());
    }

    public void RightAnswerUI(){
        int streak = gameManager.GetCorrectStreak();
        answerStreakText.GetComponent<TextMeshProUGUI>().text = streak + "x correct answer streak!";
        StartCoroutine(FlashingSequence());
    }

    public IEnumerator FlashingSequence(){

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(true);
        }
        for(int i=0; i<layerThree.Length; i++){
            layerThree[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(true);
        }
        yield return new WaitForSeconds(2.0f);

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(false);
        }
        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(false);
        }
        //only reactivate input elements
        if(layerThree.Length >= 2){
            for(int i=0; i<2; i++){
            layerThree[i].SetActive(true);
        }
        }
        
    }
}
