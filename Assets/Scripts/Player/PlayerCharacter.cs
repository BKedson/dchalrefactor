using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using dchalrefactor.Scripts.Animations.PlayerMovement;
public class PlayerCharacter : MonoBehaviour {
	private int _health;
	[SerializeField]private GameObject healthUI;
	[SerializeField]private GameObject death;

	private int startingHealth = 3;

	private static PlayerCharacter player;
	//[SerializeField] PlayerInventory backpack = null;
	//public GameObject gunUI;
	//public GameObject gunHUD;
	//public GameObject swordUI;
	//public GameObject swordHUD;
	//public GameObject controllerUI;
	//public bool start;
	//stores the Wing Number where the player is
	//public int currentWingNumber; //we can use this to know where the player is and if they fail, keep them in this wing
	//stores whether the player has obtained a key to advance in the game - the player loses the key while teleporting initially and has to get it back by solving the challenges
	/// <summary>
	///public bool hasDoorKey; //we can use this to open doors 
	/// </summary>
	//stores whether the player has finished the current level
	//public bool hasFinishedLevel; //use this for flagging tp from mazes

	public GameObject[] characters;

	void Awake(){
        if(player) {
            DestroyImmediate(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            player = this;
        }
	}
	void Start() 
	{
		InitializePlayer();
	}

	public void InitializePlayer() {
				//Deactivate all the characters-------------------------------------------------
		foreach(GameObject character in characters)
		{
			// Debug.Log("setting false");
			character.SetActive(false);
		}
		//Activate the current character -----------------------------------------------
		GetActiveCharacter().SetActive(true);
		//setting true
		// Debug.Log($"setting true {GameManager.manager.currentCharacter}");
		//------------------------------------------------------------------------------
		//start = true;
		_health = startingHealth;
		//has not Yet finshed level
		//hasFinishedLevel = false;
	}

	//returns the current active player character
	public GameObject GetActiveCharacter()
	{
		return characters[(GameManager.manager.GetCurrentCharacter())] as GameObject;
	}

	//use this and add an indicator on run
	public void Hurt(int damage) {
		if(GameManager.manager && !GameManager.manager.IsInvincible()){
			_health -= damage;

			DisplayHealth();

			if (_health <= 0){
				death.SetActive(true);
				Time.timeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.Confined;
			}
		
			// Debug.Log("Health: " + _health);
		}
	}

	// Resets player data
	public void Reset() {
		FullHealth();
		DisableHealthUI();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Tells the transition manager to restart the scene
	public void Restart() {
		Time.timeScale = 1;
		
		GameObject transitionManager = GameObject.Find("Transition Manager");
		if (transitionManager) {
			transitionManager.GetComponent<TransitionManager>().RestartLevel();
		}
		
		death.SetActive(false);
	}

	public void FullHealth() {
		_health = startingHealth;
		DisplayHealth();
	}

	public void DisplayHealth() {
			var textComp = healthUI.GetComponentInChildren<TMP_Text>();
			string hp = "";
			for(int i = 0; i<_health;i++){
				hp = hp + "*";
			}

			textComp.text = "Health " + _health + " " + hp;
	}

	public void DisableHealthUI() {
		healthUI.SetActive(false);
	}

	public void EnableHealthUI() {
		healthUI.SetActive(true);
	}

	public void ChangeActiveSkin() {
		GetActiveCharacter().SetActive(true);
		Debug.Log("CHARACTER CHANGED SKINS");
	}
}
