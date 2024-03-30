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
    //INTERMEDIATE HELPER METHODS------------------------------------------
    //Used to check the sign up - registratin operation
    public bool IsRegisrationSuccessful(string username, string password)
    {
        try
        {
            SignUpWithUsernamePassword(username, password);
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
        return false;
    }
    //Used to check the sign in - login operation
    public bool IsLoginSuccessful(string username, string password)
    {
        try
        {
            SignInWithUsernamePassword(username, password);
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
        return false;
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
