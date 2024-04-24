using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] indicators;
    private bool indicatorChange;
    // Start is called before the first frame update
    void Start()
    {
        indicatorChange = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(indicatorChange){
            for(int i =0; i<indicators.Length; i++){
                indicators[i].SetActive(!indicators[i].activeSelf);
            }
            
            StartCoroutine(BlinkIndicator());
            indicatorChange = false;
        }
    }

    private IEnumerator BlinkIndicator(){
        yield return new WaitForSeconds(0.5f);
        indicatorChange = true;
    }
}
