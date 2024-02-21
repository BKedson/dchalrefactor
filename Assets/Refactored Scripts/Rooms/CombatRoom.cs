// using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class CombatRoom : BaseRoom
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spikePrefab;

    // Selects the next spawned room, 1-indexed
    public int resetRoom = -1;
    private GameObject player;
    private float wallHeight = 2.5f;
    private float wallThickness = 1.2f;
    private float playerZero = 0;
    private int easiestRoom = 1;
    private int hardestRoom = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerZero = player.transform.position.z;

        GenerateNewRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (resetRoom != -1) {
            Reset();
        }
    }

    private void Reset() {
        GameObject[] destructibles = GameObject.FindGameObjectsWithTag("EditorOnly");

        for (int i = 0; i < destructibles.Length; i++) {
            Destroy(destructibles[i].gameObject);
        }

        switch(resetRoom) {
            case 1:
                InstantiateRoom1();
                break;
            case 2:
                InstantiateRoom2();
                break;
            case 3:
                InstantiateRoom3();
                break;
            default:
                InstantiateRoom1();
                break;
        } 
        resetRoom = -1;
    }

    // Randomly generates a new room based on the current
    private void GenerateNewRoom() {    
        int roomType = Random.Range(easiestRoom, hardestRoom + 1);
        switch(roomType) {
            case 1:
                InstantiateRoom1();
                break;
            case 2:
                InstantiateRoom2();
                break;
            case 3:
                InstantiateRoom3();
                break;
            default:
                InstantiateRoom1();
                break;
        } 
    }

    // Creates Room 1, a simple room with enemies in the center
    private void InstantiateRoom1() {
        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(0, 1, 17 + playerZero), Quaternion.Euler(0, 0, 0));
    }

    // Creates Room 2, a simple room with walls and enemies on the left and right and enemies in the center
    private void InstantiateRoom2() {
        Instantiate(wallPrefab, new Vector3(7, wallHeight/2, 7 + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(-7, wallHeight/2, 7 + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);
        
        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(0, 1, 17 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-13, 1, 1 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(13, 1, 1 + playerZero), Quaternion.Euler(0, 0, 0));
    }

    // Creates Room 3, a simple room with spikes and enemies in the center
    private void InstantiateRoom3() {
        Instantiate(spikePrefab, new Vector3(0, 0, 15 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(18, 0.5f, 1f);

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(0, 1, 10 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-4.3f, 1, 23 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(4.3f, 1, 23 + playerZero), Quaternion.Euler(0, 0, 0));
    }
}
