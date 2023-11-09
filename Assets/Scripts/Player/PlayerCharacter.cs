using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerCharacter : MonoBehaviour {
	private int _health;
	[SerializeField]private GameObject healthUI;
	[SerializeField]private GameObject death;
	[SerializeField] Pack backpack = null;
	public GameObject gunUI;
	public GameObject gunHUD;
	public GameObject swordUI;
	public GameObject swordHUD;
	public GameObject controllerUI;
	public bool start;
	//stores the Wing Number where the player is
	public int currentWingNumber; //we can use this to know where the player is and if they fail, keep them in this wing
	//stores whether the player has obtained a key to advance in the game - the player loses the key while teleporting initially and has to get it back by solving the challenges
	public bool hasDoorKey; //we can use this to open doors 
	//stores whether the player has finished the current level
	public bool hasFinishedLevel; //use this for flagging tp from mazes

	void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}
	void Start() {
		start = true;
		_health = 2;
		//has not Yet finshed level
		hasFinishedLevel = false;
	}

	private void OnTriggerEnter(Collider collision){
		if(collision.CompareTag("gun") || collision.CompareTag("sword") || collision.CompareTag("controller")){
			this.GetComponent<MouseLook>().enabled = false; 
			transform.GetChild(1).GetComponent<MouseLook>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

			if(collision.CompareTag("gun")){
				gunUI.SetActive(true);
				gunHUD.SetActive(true);
			}

			if(collision.CompareTag("sword")){
				swordUI.SetActive(true);
				swordHUD.SetActive(true);
			}

			if(collision.CompareTag("controller")) controllerUI.SetActive(true);

			//adding item to the backpack
			backpack.AddItem(collision.gameObject);
			collision.gameObject.SetActive(false);
		}
	}

	public void Hurt(int damage) {
		_health -= damage;
		var textComp = healthUI.GetComponent<Text>();
		string hp = "";
		for(int i = 0; i<_health;i++){
			hp = hp + "*";
		}
		if(_health > 0){
			textComp.text = "Health: " + _health + " " + hp;
		}
		else{
			textComp.text = "Health: " + _health + " " + hp;
			death.SetActive(true);
		}
		Debug.Log("Health: " + _health);
	}
}
