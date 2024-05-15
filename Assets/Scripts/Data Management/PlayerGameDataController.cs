using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Data Controller
public class PlayerGameDataController : MonoBehaviour
{
    // Singleton instance
    private static PlayerGameDataController _instance;

    public static PlayerGameDataController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerGameDataController>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerGameDataController");
                    _instance = singletonObject.AddComponent<PlayerGameDataController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            Debug.Log("Instance returned of controller");
            return _instance;
        }
    }
    public PlayerDataManager dataManager;
    //-------------------------------------------------USER DATA-----------------------------------------------------------------
    //Stores the ID data associated with this player
    public string FirstName;
    public string NickName;
    public int CodeNumber;
    public int AdditionDifficulty;
    public int SubtractionDifficulty;
    public int MultiplicationDifficulty;
    public int DivisionDifficulty;
    public string CurrentCharacter;
    public bool IsInvincible;
    public int AdditionQuestionComplexity;
    public int SubtractionQuestionComplexity;
    public int MultiplicationQuestionComplexity;
    public int DivisionQuestionComplexity;    
    //---------------------------------------------------IN-GAME LOGIC-----------------------------------------------------------
    public bool IsNewUser;
    public bool IsNewGame;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy surplus instances.
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Persist this singleton instance.
        }
        //Indicate new user
        IndicateNewUser(true);
    }
    //---------------------------------------------------------------------
    //INITIALIZE DATA FOR THE CONTROLLER - from manager to controller
    public void InitializeGameData()
    {
        UnpackGameDataFile(dataManager.loginGameData);
    }

    //---------------------------------------------------------------------
    //SAVE DATA TO THE MANAGER - from this controller class file to manager
    public void SaveGameData()
    {
        //
        Debug.Log("Data saved");
        dataManager.sessionGameData = PackGameDataFile();
        dataManager.SaveGameDataToCloud();
    }
    //---------------------------------------------------------------------
    //PACK DATA (PACKAGING) - from controller variables to class file
    //Method to create a data Package
    public PlayerGameData PackGameDataFile()
    {
        PlayerGameData data = new PlayerGameData();
        data.FirstName = FirstName;
        data.NickName = NickName;
        data.CodeNumber = CodeNumber;
        data.AdditionDifficulty = AdditionDifficulty;
        data.SubtractionDifficulty = SubtractionDifficulty;
        data.MultiplicationDifficulty = MultiplicationDifficulty;
        data.DivisionDifficulty = DivisionDifficulty;
        data.AdditionQuestionComplexity = AdditionQuestionComplexity;
        data.SubtractionQuestionComplexity = SubtractionQuestionComplexity;
        data.MultiplicationQuestionComplexity = MultiplicationQuestionComplexity;
        data.DivisionQuestionComplexity = DivisionQuestionComplexity;
        data.CurrentCharacter = CurrentCharacter;
        data.IsInvincible = IsInvincible;
        return data;
    }

    //--------------------------------------------------------------------------
    //UNPACK DATA (unboxing) - from class file to controller variables---------
    //Method to unpack a data Package to controller
    public void UnpackGameDataFile(PlayerGameData data)
    {
        FirstName = data.FirstName;
        NickName = data.NickName;
        CodeNumber = data.CodeNumber;
        AdditionDifficulty = data.AdditionDifficulty;
        SubtractionDifficulty = data.SubtractionDifficulty;
        MultiplicationDifficulty = data.MultiplicationDifficulty;
        DivisionDifficulty = data.DivisionDifficulty;
        AdditionQuestionComplexity = data.AdditionQuestionComplexity;
        SubtractionQuestionComplexity = data.SubtractionQuestionComplexity;
        MultiplicationQuestionComplexity = data.MultiplicationQuestionComplexity;
        DivisionQuestionComplexity = data.DivisionQuestionComplexity;
        CurrentCharacter = data.CurrentCharacter;
        IsInvincible = data.IsInvincible;
    }

    public PlayerGameData DefaultGameDataFile(string firstName, string nickName, int codeNumber)
    {
        PlayerGameData data = new PlayerGameData();
        data.FirstName = firstName;
        data.NickName = nickName;
        data.CodeNumber = codeNumber;
        data.AdditionDifficulty = 1;
        data.SubtractionDifficulty = 1;
        data.MultiplicationDifficulty = 1;
        data.DivisionDifficulty = 1;
        data.AdditionQuestionComplexity = 0;
        data.SubtractionQuestionComplexity = 0;
        data.MultiplicationQuestionComplexity = 0;
        data.DivisionQuestionComplexity = 0;
        data.CurrentCharacter = "DC_Woman_2";
        data.IsInvincible = false;
        return data;
    }

    //--------------------------------------------------------------------------------------------------
    public void IndicateNewUser(bool type)
    {
        IsNewUser = type;
    }
    public bool CheckIfNewUser()
    {
        return IsNewUser;
    }

    public void IndicateNewGame(bool type)
    {
        IsNewGame = type;
    }

    public bool CheckIfNewGame()
    {
        return IsNewGame;
    }
}
