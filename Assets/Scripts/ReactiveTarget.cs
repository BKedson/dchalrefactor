using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class ReactiveTarget : MonoBehaviour {

	public bool isDead;
	public float deathAnimLength = 1.0f;
	public float deathAngle = 30.0f;
	private bool startAnim = false;
	private float currentDeathTimer = 0.0f;
	private float angleCovered = 0.0f;
	public int tier;
	public TMP_Text operandOne;
	public TMP_Text operandTwo;
	public TMP_Text operation;
	public GameObject answer = null;
	decimal parseOne;
	decimal parseTwo;
	decimal answerParse;
	void Start(){
		answer = GameObject.Find("Canvas");
	}
	void Update(){
		if(startAnim){
			startAnim = false;
			currentDeathTimer = deathAnimLength;
		}

		if(currentDeathTimer > 0){
			currentDeathTimer = currentDeathTimer - Time.deltaTime;
			float angleToRotate = Time.deltaTime * deathAngle;
			float angleleft = deathAngle - angleCovered;

			if(angleleft < angleToRotate){
				angleToRotate = angleleft;
				currentDeathTimer = -1;
			}
			angleCovered += angleToRotate;
			transform.Translate(0,angleToRotate,0);
		}
	}

	public void ReactToHit() {	
		if(answer == null) answer = GameObject.Find("Canvas");
		parseOne = int.Parse(operandOne.text);
		parseTwo = int.Parse(operandTwo.text);
		if(answer.transform.Find("Answer UI").GetChild(0).GetComponent<TMP_InputField>().text.Any(char.IsDigit)){
			answerParse = decimal.Parse(answer.transform.Find("Answer UI").GetChild(0).GetComponent<TMP_InputField>().text);
		}
		else return;
		
		if(operation.text == "x"){
			if(parseOne * parseTwo == answerParse){
				startAnim = true;
				StartCoroutine(Die());
			}
		}
		else if(operation.text == "÷"){
			if(BitConverter.GetBytes(decimal.GetBits(parseOne/parseTwo)[3])[2] > 5){
				if(Math.Round(parseOne/ parseTwo, 5) == answerParse){
					startAnim = true;
					StartCoroutine(Die());
				}
			}
			else{
				if(parseOne/parseTwo == answerParse){
					startAnim = true;
					StartCoroutine(Die());
				}
			}
		}
		else if(operation.text == "-"){
			if(parseOne - parseTwo == answerParse){
				startAnim = true;
				StartCoroutine(Die());
			}
		}
		else if(operation.text == "+"){
			if(parseOne + parseTwo == answerParse){
				startAnim = true;
				StartCoroutine(Die());
			}
		}
		
	}

	private IEnumerator Die() {
		isDead = true;
		yield return new WaitForSeconds(1.5f);
		Destroy(this.gameObject);
	}
}
