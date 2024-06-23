using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectManager : MonoBehaviour
{
    [SerializeField] private GameManagerProxy gameManager;
    [SerializeField] private Button addition;
    [SerializeField] private Button subtraction;
    [SerializeField] private Button multiplication;
    [SerializeField] private Button division;
    [SerializeField] private Button random;

    void Start()
    {
        int subjectSetting = gameManager.GetSubjectSetting();
        switch(subjectSetting){
            case 0:
                addition.Select();
                break;
            case 1:
                subtraction.Select();
                break;
            case 2: 
                multiplication.Select();
                break;
            case 3:
                division.Select();
                break;
            default:
                random.Select();
                break;
        }
    }
}
