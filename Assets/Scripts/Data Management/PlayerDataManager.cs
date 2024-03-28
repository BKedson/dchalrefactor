using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class PlayerDataManager : MonoBehaviour
{
    //stores login game data and session game date
    public PlayerGameData loginGameData;
    public PlayerGameData sessionGameData;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //CLOUD INTERATIONS----------------------------------------------------
    //Saves data from the session data file to the Cloud Storage
    public async void SaveGameDataToCloud()
    {

    }
    //Retrieves data from the online storage to the login data file
    public async void RetrieveGameDataFromCloud()
    {

    }
}
