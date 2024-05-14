using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnswerFeedback : MonoBehaviour
{
    [SerializeField] private GameObject[] layerOne;
    [SerializeField] private GameObject[] layerTwo;

    public void WrongAnswerUI(){

        StartCoroutine(FlashingSequence());
    }

    public void RightAnswerUI(){
        StartCoroutine(FlashingSequence());
    }

    public IEnumerator FlashingSequence(){

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(true);
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

    }
}
