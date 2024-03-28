using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Data Controller
public class PlayerGameDataController : MonoBehaviour
{
    //Stores a reference to the PlayerDataManager for this player
    public PlayerDataManager dataManager;
    //Stores the ID data associated with this player
    public string FirstName { get; set; }
    public string NickName  { get; set; }
    public int CodeNumber   { get; set; }

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
    }
}
