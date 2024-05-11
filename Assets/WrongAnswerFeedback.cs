using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongAnswerFeedback : MonoBehaviour
{
    [SerializeField] private GameObject[] layerOne;
    [SerializeField] private GameObject[] layerTwo;

    public void WrongAnswerUI(){

        StartCoroutine(ErrorFlashingSequence());
    }

    public IEnumerator ErrorFlashingSequence(){
        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(false);
        }
        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(true);
        }
        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<layerOne.Length; i++){
            layerOne[i].SetActive(false);
        }
        for(int i=0; i<layerTwo.Length; i++){
            layerTwo[i].SetActive(false);
        }
    }
}
