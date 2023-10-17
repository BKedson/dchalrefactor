using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneSwap : MonoBehaviour
{
    //reference to animator of the cross fade
    public Animator transitioner;
    //variable to control transition duration
    public float transitionTime = 1f;
    //stores a reference to the SoundEffectsManager
    public SoundEffectsManager manager;
    //stores a reference to the AudioManger2 - the researchMusic
    public Audiomanager2 researchMusicManager;
    //stores a reference to the AudioManager3 which contains the script AudioManagerMain
    public AudioManagerMain gameMusicManager;
    public SceneController sc;//set this and dont change
    private float xPos = 0;
    private float ypos = -38.91979f;
    private float zpos = 0;
    private Vector3 tp;
    public GameObject canvas;
    private AsyncOperation sceneAsync;
    private SphereCollider tpBox;
    private GameObject swordUI;
    private GameObject gunUI;
    private GameObject player;

    //stores the scene that we are going to
    public string target;
    //stores the scene we are leaving from
    public string sceneToDelete;

    public int wingNum;

    void Start()
    {
        tpBox = GetComponent<SphereCollider>();
        canvas = GameObject.Find("Canvas");
        player = GameObject.Find("Player");
        swordUI = canvas.transform.Find("Sword").gameObject;
        gunUI = canvas.transform.Find("Ray").gameObject;
        sc = GameObject.Find("Controller").GetComponent<SceneController>();
        if (wingNum == 1) //addition
        {
            xPos = -2.890938f;
            zpos = 146.1741f;
        }
        else if (wingNum == 2) //subtraction
        {
            xPos = 290.0224f;
            zpos = -21.88322f;
        }
        else if (wingNum == 3) //multiplication
        {
            xPos = -175.56f;
            zpos = -5.7f;
        }
        else if (wingNum == 4) //division
        {
            xPos = 14.15857f;
            zpos = -132.387f;
        }
        else
        {
            xPos = 0;
            ypos = 0;
            zpos = 0;
        }
        //target position for this teleporter
        tp = new Vector3(xPos, ypos, zpos);

    }

    void OnTriggerEnter(Collider other)
    {
        sc._enemies.RemoveAll(s => s == null);
        
        if((other.CompareTag("Player") && other.gameObject.GetComponent<PlayerCharacter>().start) || (other.CompareTag("Player") && sc._enemies.Count <= 0) || (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerCharacter>().hasFinishedLevel))
        {
            Debug.Log(other.gameObject.GetComponent<PlayerCharacter>().hasFinishedLevel + " " + sc._enemies.Count + " " + other.gameObject.GetComponent<PlayerCharacter>().start);
            //sc.isMaze = false;
            
            WeaponController w = other.gameObject.GetComponentInChildren<WeaponController>();
            if(wingNum == 0){
                if(w.stuff.checkIfCollected(1)){swordUI.SetActive(true);}
                if(w.stuff.checkIfCollected(2)){gunUI.SetActive(true);}
                w.inResearch = true;
            }
            else{
                if(other.gameObject.GetComponent<PlayerCharacter>().start == false){
                    sc.SpawnEnemyGroups();
                }
                swordUI.SetActive(false);
                gunUI.SetActive(false);
                w.inResearch = false;
                w.selectWeapon(-1);
            }
            other.gameObject.GetComponent<PlayerCharacter>().start = false;
            other.gameObject.GetComponent<PlayerCharacter>().hasFinishedLevel = false;
            //---------------------------------------------------
            //Play the teleportation sound
            manager.play("TeleportAmbience");
            //Animation code begins and everyhting else runs in the background
            //Play animation
            transitioner.SetTrigger("Start");
            //if the teleporters are one of the four, load level
            Time.timeScale = 0f;
            StartCoroutine(LoadLevel(other.transform, false));
            Time.timeScale = 1f;
        }
    }

    IEnumerator LoadLevel(Transform playerTrans, bool toReload)
    {
        //Time.timeScale = 0.5f;
        //wait
        yield return new WaitForSeconds(transitionTime);
        //by this point, the animation of transition has ended (UIPopup) and now alpha is max.
        //we can stop the research music if this object is not null - that means we are in the research hub
        if (researchMusicManager != null)
        {
            researchMusicManager.stop("CommonsUpbeat");
        }
        //else if it is null - that means we are in the game scene and therefore should stop the gameMusicManager
        else
        {
            //loop to stop all 
            for (int i = 0; i < 3; i++)
            {
                gameMusicManager.stop(gameMusicManager.sounds[i].name);
            }
        }

        //level loading code - BEGIN
        //------------------------------------------------------------------------------------------------------
        //we do not want to laod the ResearchScene
        if (target == "Game")
        {
            //set the game prompt to active
            //canvas.transform.Find("TaskBorder").gameObject.SetActive(true);
            //prompt the player to get 90 points using PromptController
            GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("Get90Points");
            if (SceneManager.GetSceneByName(target).isLoaded) SceneManager.UnloadSceneAsync(target, UnloadSceneOptions.None);
            AsyncOperation scene = SceneManager.LoadSceneAsync(target, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;
            sceneAsync = scene;
            //animation begin
            if (tpBox) tpBox.enabled = false;
            //time pause - so animation must begin before time pause
            Time.timeScale = 0f;
            while (scene.progress < 0.9f)
            {
                Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
                yield return null;
            }
            sceneAsync.allowSceneActivation = true;
            while (!scene.isDone)
            {
                yield return null;
            }

        }
        //--------------------------------------------------------------------------------------------------------
        //level loading code - END
        OnFinishedLoading();
        Time.timeScale = 1f;
        playerTrans.position = tp;
        //unload the Previous scene if it is Game
        if (sceneToDelete == "Game")
        {

            //set the isMaze attribute to true because we are returning to the maze
            //sc.isMaze = true;
            //we are returning to base: IF = they failed
            if (GameObject.Find("Player").GetComponent<PlayerCharacter>().hasFinishedLevel)
            {
                //they must quit and try again
                GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("TryAgain");
            }
            else if (toReload)
            {
                GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("FindTheTeleporters");
            }
            else
            {
                //they can unlock doors
                GameObject.Find("TaskBorder").GetComponent<PromptController>().promptUI("UnlockedDoors");
            }
            //
            if (SceneManager.GetSceneByName(sceneToDelete).isLoaded) SceneManager.UnloadSceneAsync(sceneToDelete);
        }
        //PointsAndScoreController.Instance.ResetPoints();
        if(target == "Game"){
            Destroy(this.transform.parent.gameObject);
        }
    }

    void enableScene(string scene)
    {
        UnityEngine.SceneManagement.Scene sceneToLoad = SceneManager.GetSceneByName(scene);
        if (sceneToLoad.IsValid())
        {
            SceneManager.MoveGameObjectToScene(canvas, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }

    void OnFinishedLoading()
    {
        enableScene(target);
        //done loading so end the transition by triggering End
        transitioner.SetTrigger("End");
        PointsAndScoreController.Instance.inGameScene = target == "Game";
        PointsAndScoreController.Instance.currentWingNum = wingNum;
    }

    public void ReloadGameScene(bool returnToResearch)
    {
        if (returnToResearch)
        {
            target = "ResearchScene";
            sceneToDelete = "Game";
            wingNum = 0;
            switch (PointsAndScoreController.Instance.currentWingNum)
            {
                case 1:
                    //GameObject.Find("Teleport Addition").GetComponentInChildren<SceneSwap>().hasBeenOverlapped = false;
                    //GameObject.Find("Teleport Addition").GetComponentInChildren<SceneSwap>().sc.isMaze = true;
                    GameObject.Find("Teleport Addition").GetComponentInChildren<SceneSwap>().tpBox.enabled = true;
                    break;
                case 2:
                    //GameObject.Find("Teleport Subtraction").GetComponentInChildren<SceneSwap>().hasBeenOverlapped = false;
                    //GameObject.Find("Teleport Subtraction").GetComponentInChildren<SceneSwap>().sc.isMaze = true;
                    GameObject.Find("Teleport Subtraction").GetComponentInChildren<SceneSwap>().tpBox.enabled = true;
                    break;
                case 3:
                    //GameObject.Find("Teleport Multiplication").GetComponentInChildren<SceneSwap>().hasBeenOverlapped = false;
                    //GameObject.Find("Teleport Multiplication").GetComponentInChildren<SceneSwap>().sc.isMaze = true;
                    GameObject.Find("Teleport Multiplication").GetComponentInChildren<SceneSwap>().tpBox.enabled = true;
                    break;
                case 4:
                    //GameObject.Find("Teleport Division").GetComponentInChildren<SceneSwap>().hasBeenOverlapped = false;
                    //GameObject.Find("Teleport Division").GetComponentInChildren<SceneSwap>().sc.isMaze = true;
                    GameObject.Find("Teleport Division").GetComponentInChildren<SceneSwap>().tpBox.enabled = true;
                    break;
            }
            sc.SpawnEnemyGroups();
            PointsAndScoreController.Instance.currentWingNum = 0;
        }
        else
        {
            sceneToDelete = "ResearchScene";
            target = "Game";
            wingNum = PointsAndScoreController.Instance.currentWingNum;
        }

        if (wingNum == 1)
        {
            xPos = -2.890938f;
            ypos = -38.91979f;
            zpos = 146.1741f;
        }
        else if (wingNum == 2)
        {
            xPos = 290.0224f;
            ypos = -38.91979f;
            zpos = -21.88322f;
        }
        else if (wingNum == 3)
        {
            xPos = -175.56f;
            ypos = -38.91979f;
            zpos = -5.7f;
        }
        else if (wingNum == 4)
        {
            xPos = 14.15857f;
            ypos = -38.91979f;
            zpos = -132.387f;
        }
        else
        {
            xPos = 0;
            ypos = 0;
            zpos = 0;
        }
        //target position for this teleporter
        tp = new Vector3(xPos, ypos, zpos);
        //Play animation
        transitioner.SetTrigger("Start");
        StartCoroutine(LoadLevel(GameObject.Find("Player").transform, true));
    }
}
