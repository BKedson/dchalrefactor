using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    //reference to this door's box collider
    public GameObject theCollider;
    //stores the corresponding difficulties for the operations
    public int addition_Difficulty;
    public int minus_Difficulty;
    public int multiply_Difficulty;
    public int divide_Difficulty;
    //stores the two integers to be used in the operation and the corresponding UI TMP texts
    public int firstNumber;
    public int secondNumber;
    public TMP_Text operandOne;
    public TMP_Text operandTwo;
    //stores a string representation of the operand: either +, -, ÷, x
    public TMP_Text operation;
    public string operatorSign;
    //stores the diffManager
    public GameObject diffManager;


    void Start()
    {
        generate();
    }

    public void generate(){
        //get the diffManager
        diffManager = GameObject.Find("DifficultyManager");
        //get the operator
        operatorSign = operation.text;
        //get the respective difficulties
        addition_Difficulty = diffManager.GetComponent<DiffManager>().getAdd();
        //addition_Difficulty = 1;
        minus_Difficulty = diffManager.GetComponent<DiffManager>().getSub();
        //minus_Difficulty = 1;
        multiply_Difficulty = diffManager.GetComponent<DiffManager>().getMult();
        //multiply_Difficulty = 1;
        divide_Difficulty = diffManager.GetComponent<DiffManager>().getDiv();
        //divide_Difficulty = 1;


        //DIVISION
        //Case 1: Generating Numbers for Division
        //variables r1, r2 and k(for even integer division)
        if (operatorSign == "÷")
        {
            //k ensures that there is an integer answer
            //r2 numerator, r1 denominator
            int r1 = 1;
            int k = 1;
            if (divide_Difficulty == 1)
            {
                r1 = Random.Range(1, 9);
                k = Random.Range(1, 11);
            }
            else if (divide_Difficulty == 2)
            {
                r1 = Random.Range(1, 9);
                k = Random.Range(100, 1111);
            }
            else if (divide_Difficulty == 3)
            {
                r1 = Random.Range(10, 99);
                k = Random.Range(100, 1001);
            }
            int r2 = r1 * k;
            firstNumber = r2;
            secondNumber = r1;
        }

        //SUBTRACTION
        //Case 2: Generating Numbers for Minus
        //variables r1, r2 and k(for non-negative answers)
        if (operatorSign == "-")
        {
            //k ensures that there is a positive answer
            int s1 = 0;
            int s2 = 0;
            if (minus_Difficulty == 1)
            {
                int n = Random.Range(2, 4);
                for (int i = 0; i < n; i++)
                {
                    int r1 = Random.Range(0, 10);
                    int r2 = Random.Range(0, r1);
                    //using ^ is an XOR operation not an exponent operation - must use Mathf.Pow()
                    r1 = r1 * (int)Mathf.Pow(10, i);
                    r2 = r2 * (int)Mathf.Pow(10, i);
                    s1 += r1;
                    s2 += r2;
                }
            }
            if (minus_Difficulty == 2)
            {
                s1 = Random.Range(0, 10000);
                int k = Random.Range(0, 10000);
                s2 = k + s1;
            }
            if (minus_Difficulty == 3)
            {
                s1 = Random.Range(100000, 1000000);
                int k = Random.Range(100000, 1000000);
                s2 = s1 + k;
            }
            firstNumber = s1;
            secondNumber = s2;
        }

        //ADDITION
        //Case 3: Generating Numbers for Add
        //variables r1, r2 
        if (operatorSign == "+")
        {
            int s1 = 0;
            int s2 = 0;
            if (addition_Difficulty == 1)
            {
                int n = Random.Range(2, 4);
                for (int i = 0; i < n; i++)
                {
                    int r1 = Random.Range(0, 10);
                    int k = 9 - r1;
                    int r2;
                    if (k == 0)
                    {
                        r2 = 0;
                    }
                    else
                    {
                        r2 = Random.Range(0, (k + 1));
                    }
                    //using ^ is an XOR operation not an exponent operation - must use Mathf.Pow()
                    r1 = r1 * (int)Mathf.Pow(10, i);
                    r2 = r2 * (int)Mathf.Pow(10, i);
                    s1 = s1 + r1;
                    s2 = s2 + r2;
                }
            }
            if (addition_Difficulty == 2)
            {
                s1 = Random.Range(1000, 100000);
                s2 = Random.Range(1000, 100000);
            }
            if (addition_Difficulty == 3)
            {
                s1 = Random.Range(100000, 10000000);
                s2 = Random.Range(100000, 10000000);
            }

            firstNumber = s1;
            secondNumber = s2;
        }

        //Case 4: Generating Numbers for times
        //variables r1, r2 
        if (operatorSign == "x")
        {
            int r1 = 0;
            int r2 = 0;
            if (multiply_Difficulty == 1)
            {
                r2 = Random.Range(0, 10);
                if (r2 == 0)
                {
                    r1 = Random.Range(0, 100);
                }
                else
                {
                    int n = Random.Range(1, 4);
                    for (int i = 0; i < n; i++)
                    {
                        int k = Random.Range(0, 9 / r2 + 1);
                        r1 += k * (int)Mathf.Pow(10, i);
                    }
                }
            }
            else if (multiply_Difficulty == 2)
            {
                r1 = Random.Range(100, 10000);
                r2 = Random.Range(0, 10);
            }
            else if (multiply_Difficulty == 3)
            {
                r1 = Random.Range(100, 10000);
                r2 = Random.Range(10, 100);
            }
            firstNumber = r1;
            secondNumber = r2;
        }

        //set the Operands accordingly
        operandOne.text = firstNumber.ToString();
        operandTwo.text = secondNumber.ToString();
        //set them inactive - changes made with the time Update
        operandOne.gameObject.SetActive(false);
        operandTwo.gameObject.SetActive(false);
        operation.gameObject.SetActive(false);
        //send them to the collider
        theCollider.GetComponent<AnswerUICollider>().firstNumberCheck = firstNumber;
        theCollider.GetComponent<AnswerUICollider>().secondNumberCheck = secondNumber;
        //send the operator sign to the collider
        theCollider.GetComponent<AnswerUICollider>().operatorCheck = operatorSign;

    }
}
