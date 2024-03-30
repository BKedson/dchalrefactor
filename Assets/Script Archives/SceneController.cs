using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour {
	[SerializeField] private GameObject enemyPrefab = null;
	public List<GameObject> _enemies;
	public List<GameObject> spawnPoints;
	public int enemiesToAdd = 14;
	GameObject[] doors;

	void Start() {
		_enemies = new List<GameObject>();
		SpawnEnemyGroups();
	}

	
	public GameObject SpawnEnemy(Vector3 position){
		float angle = Random.Range(0, 360);
		GameObject _enemy = Instantiate(enemyPrefab, position, Quaternion.Euler(0,angle,0)) as GameObject;
		return _enemy;
	}

	public void SpawnEnemyGroups(){
		PointsAndScoreController.Instance.resetEnemyPoints();
		for(int i = 0; i < enemiesToAdd;i++ ){
				if(i < 2){
					_enemies.Add(SpawnEnemy(gameObject.transform.position));	
				}
				else if(i < 5){
					_enemies.Add(SpawnEnemy(spawnPoints.ElementAt(0).transform.position));	
				}
				else if(i < 8){
					_enemies.Add(SpawnEnemy(spawnPoints.ElementAt(1).transform.position));	
				}
				else if(i < 11){
					_enemies.Add(SpawnEnemy(spawnPoints.ElementAt(2).transform.position));	
				}	
				else{
					_enemies.Add(SpawnEnemy(spawnPoints.ElementAt(3).transform.position));	
				}
			}
	}
}
