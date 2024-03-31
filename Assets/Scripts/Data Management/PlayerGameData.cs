using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents the Json Serializable class for PlayerGameData
[System.Serializable]
public class PlayerGameData
{
    public string FirstName;
    public string NickName;
    public int CodeNumber;
    public int AdditionDifficulty;
    public int SubtractionDifficulty;
    public int MultiplicationDifficulty;
    public int DivisionDifficulty;
}
