// using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class CombatRoom : BaseRoom
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject twoWayRaisedPlatformPrefab;
    [SerializeField] private GameObject fourPointRampMapPrefab;
    [SerializeField] private GameObject cPlatformRightPrefab;
    [SerializeField] private GameObject cPlatformLeftPrefab;
    [SerializeField] private GameObject cPlatformCenterPrefab;
    [SerializeField] private GameObject uFloorPrefab;
    [SerializeField] private GameObject spikeSafetyNetPrefab;

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
        player.transform.position = new Vector3(0, 5, 0);

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
            case 4:
                InstantiateRoom4();
                break;
            case 5:
                InstantiateRoom5();
                break;
            case 6:
                InstantiateRoom6();
                break;
            case 7:
                InstantiateRoom7();
                break;
            case 8:
                InstantiateRoom8();
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
            case 4:
                InstantiateRoom4();
                break;
            case 5:
                InstantiateRoom5();
                break;
            case 6:
                InstantiateRoom6();
                break;
            case 7:
                InstantiateRoom7();
                break;
            case 8:
                InstantiateRoom8();
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

    // Creates Room 4, a maze-like room with long walls and traps
    private void InstantiateRoom4() {
        Instantiate(wallPrefab, new Vector3(-5, wallHeight/2, 10 + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(5, wallHeight/2, 20 + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(-5, wallHeight/2, 30 + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(10, 1, 10 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-10, 1, 20 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(10, 1, 30 + playerZero), Quaternion.Euler(0, 0, 0));
    }

    // Creates Room 5, a complex room with raised platforms
    private void InstantiateRoom5() {
        Instantiate(twoWayRaisedPlatformPrefab, new Vector3(2.3f, -0.1f, 15 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Instantiate(spikePrefab, new Vector3(0, 0, 10 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(-10, 0, 17 + playerZero), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(10, 0, 17 + playerZero), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(0, 0, 25 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(10, 1, 5 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-10, 1, 5 + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-0.35f, 2.3f, 16.74f + playerZero), Quaternion.Euler(0, 0, 0));
    }

    private void InstantiateRoom6() {
        Instantiate(fourPointRampMapPrefab, new Vector3(-10.2f, 1.7f, 1.9f + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(-7.4f, 2.6f, 5.5f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(8.6f, 2.6f, 5.5f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-7.4f, 2.6f, 26.9f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(8.6f, 2.6f, 26.9f + playerZero), Quaternion.Euler(0, 0, 0));
    }

    private void InstantiateRoom7() {
        Instantiate(cPlatformRightPrefab, new Vector3(4.4f, -1f, 28f + playerZero), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        Instantiate(cPlatformLeftPrefab, new Vector3(-3f, -1f, 8.1f + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        float onPlatformEnemyHeight = 2.5f;

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(-10.5f, onPlatformEnemyHeight, 14.3f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-10.5f, onPlatformEnemyHeight, 20f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-5.14f, onPlatformEnemyHeight, 25.5f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(11.7f, onPlatformEnemyHeight, 14.3f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(11.7f, onPlatformEnemyHeight, 20f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(7.14f, onPlatformEnemyHeight, 25.5f + playerZero), Quaternion.Euler(0, 0, 0));

    }

    
    private void InstantiateRoom8() {
        Instantiate(twoWayRaisedPlatformPrefab, new Vector3(3.5f, 2.1f, -2.2f + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(2f, 2f, 2f);
        Instantiate(cPlatformCenterPrefab, new Vector3(-10f, 1.86f, 30f + playerZero), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        Instantiate(uFloorPrefab, new Vector3(-15f, -0.1f, 36f + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        Instantiate(spikePrefab, new Vector3(-0.1f, 0, 19 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);
        Instantiate(spikeSafetyNetPrefab, new Vector3(-0.1f, 0, 19 + playerZero), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);

        // Replace with enemy spawn functionality
        Instantiate(enemyPrefab, new Vector3(-10.1f, 3.1f, 15.2f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-10.1f, 3.1f, 15.2f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(-5.75f, 5.6f, 32f + playerZero), Quaternion.Euler(0, 0, 0));
        Instantiate(enemyPrefab, new Vector3(5.75f, 5.6f, 32f + playerZero), Quaternion.Euler(0, 0, 0));
    }
    
}
