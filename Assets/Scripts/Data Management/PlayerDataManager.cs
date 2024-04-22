using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class PlayerDataManager : MonoBehaviour
{
    //Stores this Player's credentials
    private string playerUsername;
    private string playerPassword;
    //stores login game data and session game date
    public PlayerGameData loginGameData;
    public PlayerGameData sessionGameData;
    //Stores a reference to the PlayerDataManager for this player
    public PlayerGameDataController dataController;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();   
    }

    //INTERMEDIATE HELPER METHODS------------------------------------------
    //Used to check the sign up - registration operation
    public async Task<bool> IsRegisrationSuccessful(string username, string password)
    {
        try
        {
            await SignUpWithUsernamePassword(username, password);
            //retrieve and store the username and password - this happens when registration is successful
            playerUsername = username;
            playerPassword = password;
            return true;
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            return false;
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            return false;
        }
    }
    //Used to check the sign in - login operation
    public async Task<bool> IsLoginSuccessful(string username, string password)
    {
        try
        {
            await SignInWithUsernamePassword(username, password);
            //retrieve and store the username and password - this happens when logging in is successful
            playerUsername = username;
            playerPassword = password;
            return true;
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            //Debug.LogException(ex);
            return false;
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            //Debug.LogException(ex);
            return false;
        }
    }

    //CLOUD AND DATA INTERATIONS----------------------------------------------------
    //Saves data from the session data file to the Cloud Storage
    public async void SaveGameDataToCloud()
    {
        PlayerGameData data = sessionGameData;
        //creates a key - username
        string key = playerUsername;
        //creates the value, the json data file
        string value = JsonUtility.ToJson(data);
        //creates the dictionary
        var dataToSave = new Dictionary<string,object>{{key, value}};
        //saves the data to the cloud
        await CloudSaveService.Instance.Data.ForceSaveAsync(dataToSave);
    }
    //Retrieves data from the online storage to the login data file
    public async void RetrieveGameDataFromCloud()
    {
        //should save to the login file
        PlayerGameData data = new PlayerGameData();
        //stores the json file from the cloud 
        Dictionary<string,string> dataToLoad = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{playerUsername});
        //Obtain the data from the specific key
        string jsonFile = dataToLoad[playerUsername].ToString();
        //Serialize from json to object
        loginGameData = JsonUtility.FromJson<PlayerGameData>(jsonFile);
        //add to session
        sessionGameData = loginGameData;
        //unpack to controller
        dataController.UnpackGameDataFile(loginGameData);
    }

    public void InitializeGameLoginData(string firstName, string nickName, int codeNumber)
    {
        //load default file
        loginGameData = dataController.DefaultGameDataFile(firstName, nickName, codeNumber);
        //send a version of the file to the cloud
        sessionGameData = loginGameData;
        SaveGameDataToCloud();
        //unpack to controller
        dataController.UnpackGameDataFile(loginGameData);
    }

    //Signs Up a new user
    public async Task SignUpWithUsernamePassword(string username, string password)
    {
        //sign up with the username with password
        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        Debug.Log("SignUp is successful.");
    }

    //Signs In an old user
    public async Task SignInWithUsernamePassword(string username, string password)
    {
        //sign in with the username with password
        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        Debug.Log("SignIn is successful.");
    }
}
