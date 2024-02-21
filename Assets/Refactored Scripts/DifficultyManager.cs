using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private GameObject descriptionText;
    [SerializeField] private GameObject easyButton;
    [SerializeField] private GameObject mediumButton;
    [SerializeField] private GameObject hardButton;
    private GameObject gameManager;

    private String easyDescription;
    private String mediumDescription;
    private String hardDescription;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        easyDescription = "The game is easy.";
        mediumDescription = "The game is medium.";
        hardDescription = "The game is hard.";

        descriptionText.GetComponent<TextMeshProUGUI>().text = easyDescription;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Changes the difficulty of the game and sets the corresponding description text
    public void ChangeDiffuclty(int diff) {
        Difficulty difficulty = (Difficulty) diff;

        gameManager.GetComponent<GameManager>().ChangeDiffuclty(difficulty);

        switch (difficulty) {
            case Difficulty.Easy:
                descriptionText.GetComponent<TextMeshProUGUI>().text = easyDescription;
                break;
            case Difficulty.Medium:
                descriptionText.GetComponent<TextMeshProUGUI>().text = mediumDescription;
                break;
            case Difficulty.Hard:
                descriptionText.GetComponent<TextMeshProUGUI>().text = hardDescription;
                break;
            default:
                break;
        }
    }
}
