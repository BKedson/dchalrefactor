using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WanderingAI : MonoBehaviour {
	public float speed = 3.0f;
	private Animator anim;
	private bool _alive;
	public bool broken;
	bool beingHit = false;
	public NavMeshAgent navMeshAgent;
	Vector3 playLastPos = Vector3.zero;
	public float initWaitTime = 4f;
	public float initRotate = 2f;
	public float viewAng = 110f;
	public float viewRad = 15f;
	public LayerMask player;
	public LayerMask obstacles;
	public float rotSpeed = 70f;
	private Vector3 minSize = new Vector3(0.7f,0.7f,0.7f);
	Vector3 playerPos;
	float newDestinationCD = 0.5f;
	float waitTime;
	float rotateTime;
	bool inRange;
	bool near;
	bool patrolling;
	private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
	bool seen;
	
	void Start() {
		_alive = true;
		playerPos = Vector3.zero;
		patrolling = true;
		seen = false;
		inRange = false;	
		Move(speed);

		anim = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.isStopped = false;
		if(!broken){
			anim.SetBool("Patrol", true);
		}
	}
	
	void Update() {
		if(_alive){//near check at start
			//if near then attack
			anim.ResetTrigger("Attack");
			View(); // continuously check if player is in line of sight
			if(!patrolling && newDestinationCD <= 0){
				newDestinationCD = 0.5f;
				chase();
			}
			else if(!isWandering) {
				Move(speed);
				StartCoroutine(patrol());
			}
			if(GameObject.FindGameObjectWithTag("Player") && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)<=2.0f){
				anim.SetTrigger("Attack");
				isWandering = false;
				anim.SetBool("Rechase", true);
				waitTime -= Time.deltaTime;
			}
			if (isRotatingRight == true){
				anim.SetBool("Spotted", false);
				anim.SetBool("Patrol", false);
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
			}
			if (isRotatingLeft == true){
				anim.SetBool("Spotted", false);
				anim.SetBool("Patrol", false);
				transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
			}
			if (isWalking == true){
				anim.SetBool("Spotted", false);
				anim.SetBool("Patrol", true);
				transform.position += transform.forward * speed * Time.deltaTime;
			}
			newDestinationCD -= Time.deltaTime;
		}
	}

	private void chase(){
		near = false;
		playLastPos = Vector3.zero;
		 // when seen start sprint anim and adjust speed, set target to player
		anim.SetBool("Spotted", true);
		Move(5.0f);
		navMeshAgent.SetDestination(playerPos);
		
		if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
			if(waitTime <= 0 && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f){
				patrolling = true;
				near = false;
				Move(speed); //when out of range return to patrol behavior
				rotateTime = initRotate;
				waitTime = initWaitTime;
			}
			else{
				if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)<=2.0f){
					anim.SetTrigger("Attack");
					isWandering = false;
					anim.SetBool("Rechase", true);
					waitTime -= Time.deltaTime;
				}
			}
		}
	}

	IEnumerator patrol(){
		// if player outside of line of sight
		int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 5);
        int walkTime = Random.Range(1, 6);
		
		isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
		anim.SetBool("Patrol", false);
		anim.SetBool("Spotted", false);
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1) {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2){
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
	}

	void Stop(){ // stop mving
		navMeshAgent.isStopped = true;
		navMeshAgent.speed = 0;
		isWandering = false;
		anim.ResetTrigger("Attack");
		anim.SetBool("Rechase", true);
	}
/*
	void LookingForP(Vector3 player){
		navMeshAgent.SetDestination(player);
		if(Vector3.Distance(transform.position, player) <= 0.3f){
			if(waitTime <=0){
				inRange = false;
				Move(speed);
				waitTime = initWaitTime;
				rotateTime = initRotate;
			}
			else{
				Stop();
				waitTime -= Time.deltaTime;
			}
		}
	}
*/
	void View(){ // check if player is in line of sight
		Collider[] pInRange = Physics.OverlapSphere(transform.position, viewRad, player);
		for(int i = 0; i < pInRange.Length;i++){
			Transform pPos = pInRange[i].transform;
			Vector3 toPlayer = (pPos.position - transform.position).normalized;
			if(Vector3.Angle(transform.forward, toPlayer) < viewAng/2){ // if player is within range of bot's forward view vectors
				float distToPlayer = Vector3.Distance(transform.position, pPos.position);
				if(!Physics.Raycast(transform.position, toPlayer, distToPlayer, obstacles)){ // if there are no obstacles between bot and player set varaibles accordingly
					inRange = true;
					patrolling = false;
					isWandering = false;
					isRotatingLeft = false;
					isRotatingRight = false;
					isWalking = false;
				}
				else{ // else not in range
					inRange = false;
				}
			}
			if(Vector3.Distance(transform.position, pPos.position)> viewRad){ // if player outside of max view angle
				inRange = false; // not in range
			}
			if(Vector3.Distance(transform.position, pPos.position) <= 10){
				patrolling = false;
				isWandering = false;
				isRotatingLeft = false;
				isRotatingRight = false;
				isWalking = false;
				inRange = true;
			}
		
			if(inRange){ // if player spotted set new player position to player current position
				playerPos = pPos.transform.position;
			}
		}
	}

	void Move(float speed){//speed adjuster
		navMeshAgent.isStopped = false;
		navMeshAgent.speed = speed;
	}

	public void SetAlive(bool alive) {
		_alive = alive;
	}

	//shrink to death
	public void Shrink(){
		if(this.gameObject.transform.localScale.x > minSize.x){
			this.gameObject.transform.localScale *= 0.99f;
		}
	}


	public void OnDestroy(){
		if (GameObject.FindGameObjectsWithTag("enemy").Count() < 1)
		{
			// if (DungeonGenerator._instance) {
			// 	DungeonGenerator._instance.ProceedLv();
			// }

			GameObject transitionManager = GameObject.Find("Transition Manager");
			if (transitionManager) {
				transitionManager.GetComponent<TransitionManager>().RestartLevel();
			}	
		}
	}


}
