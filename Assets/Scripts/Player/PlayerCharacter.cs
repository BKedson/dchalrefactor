using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class PlayerCharacter : MonoBehaviour {
	private int _health;
	[SerializeField]private GameObject healthUI;
	[SerializeField]private GameObject death;
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
		DontDestroyOnLoad(this.gameObject);
	}
	void Start() {
		//Deactivate all the characters-------------------------------------------------
		foreach(GameObject character in characters)
		{
			character.SetActive(false);
		}
		//Activate the current character -----------------------------------------------
		characters[(GameManager.manager.GetCurrentCharacter())].SetActive(true);
		//------------------------------------------------------------------------------
		//start = true;
		_health = 3;
		//has not Yet finshed level
		//hasFinishedLevel = false;
	}

	//use this and add an indicator on run
	public void Hurt(int damage) {
		_health -= damage;
		var textComp = healthUI.GetComponentInChildren<TMP_Text>();
		string hp = "";
		for(int i = 0; i<_health;i++){
			hp = hp + "*";
		}
		if(_health > 0){
			textComp.text = "Health " + _health + " " + hp;
		}
		else{
			textComp.text = "Health " + _health + " " + hp;
			death.SetActive(true);
			Time.timeScale = 0;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}
		Debug.Log("Health: " + _health);
	}
}
