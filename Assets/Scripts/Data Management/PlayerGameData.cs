using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents the Json Serializable class for PlayerGameData
[System.Serializable]
public class PlayerGameData
{
    //Identity Infrmation-------------------------------------------
    public string FirstName;
    public string NickName;
    public int CodeNumber;
    //Difficulty information-----------------------------------------
    public int AdditionDifficulty;
    public int SubtractionDifficulty;
    public int MultiplicationDifficulty;
    public int DivisionDifficulty;
    public int AdditionQuestionComplexity;
    public int SubtractionQuestionComplexity;
    public int MultiplicationQuestionComplexity;
    public int DivisionQuestionComplexity;

    //Character Information-------------------------------------------
    public string CurrentCharacter;
    //Misc Information------------------------------------------------
    public bool IsInvincible;
    public int CursorSize;
}
