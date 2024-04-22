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
    //Stores the ID data associated with this player
    public string FirstName { get; set; }
    public string NickName  { get; set; }
    public int CodeNumber   { get; set; }
    public int AdditionDifficulty { get; set; }
    public int SubtractionDifficulty { get; set; }
    public int MultiplicationDifficulty { get; set; }
    public int DivisionDifficulty { get; set; }
    public string CurrentCharacter { get; set;}

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
        data.CurrentCharacter = CurrentCharacter;
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
        CurrentCharacter = data.CurrentCharacter;
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
        data.CurrentCharacter = "DC_Woman_2";
        return data;
    }
}
