// using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;

// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class CombatRoom : BaseRoom
{
    // Level object prefabs
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject twoWayRaisedPlatformPrefab;
    [SerializeField] private GameObject fourPointRampMapPrefab;
    [SerializeField] private GameObject cPlatformRightPrefab;
    [SerializeField] private GameObject cPlatformLeftPrefab;
    [SerializeField] private GameObject cPlatformCenterPrefab;
    [SerializeField] private GameObject uFloorPrefab;
    [SerializeField] private GameObject spikeSafetyNetPrefab;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private GameObject referenceObject;

    private GameManager gameManager;
    private GameObject player;
    private float wallHeight = 2.5f;
    private float wallThickness = 1.2f;
    private int numEnemies = 5;
    private List<int> enemyStrengths = new List<int>();

    // Selects the next spawned room, 1-indexed (for editor testing)
    public int resetRoom = -1;
    // The coordinate reference for spawning objects and enemies in the level
    private float baseX = 0;
    private float baseZ = 0;
    // The minimum and maximum difficulty room that can spawn with the current settings 
    private int easiestRoom = 1;
    private int hardestRoom = 7;

    // The number of enemies that still need to be spawned in the level
    int numEnemiesToSpawn;

    // The number of different spawn locations
    int numSpawns;

    // The number of spawns per location
    int spawnsPerLoc;

    // The number of extra spawns
    int remainder;

    // TODO: Stop enemies from spawning in walls

    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.manager)
        {
            gameManager = GameManager.manager;
        }
        else
        {
            // Error
        }


        player = GameObject.Find("Player");

        if (referenceObject == null)
        {
            referenceObject = player;
        }

        baseX = referenceObject.transform.position.x;
        baseZ = referenceObject.transform.position.z + 8f;

        GenerateNewRoom();
    }

    // Update is called once per frame
    void Update()
    {
        // Editor testing
        if (resetRoom != -1)
        {
            // baseX = player.transform.position.x;
            // baseZ = player.transform.position.z;
            Reset();
        }
    }

    // Editor testing
    private void Reset()
    {
        GameObject[] destructibles = GameObject.FindGameObjectsWithTag("EditorOnly");

        for (int i = 0; i < destructibles.Length; i++)
        {
            Destroy(destructibles[i].gameObject);
        }

        enemyStrengths = gameManager.GetCurrEnemyStrengths();
        numEnemies = enemyStrengths.Count;
        Debug.Log("Num enemies: " + numEnemies);

        switch (resetRoom)
        {
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

    // Randomly generates a new room based on the current settings
    public void GenerateNewRoom()
    {
        int roomType = UnityEngine.Random.Range(easiestRoom, hardestRoom + 1);

        enemyStrengths = gameManager.GetCurrEnemyStrengths();
        numEnemies = enemyStrengths.Count;

        switch (roomType)
        {
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
    private void InstantiateRoom1()
    {
        InstantiateRoom1Enemies();
        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom1Enemies()
    {
        numEnemiesToSpawn = numEnemies;

        surface.BuildNavMesh();

        SpawnEnemies(0f, 1f, 17f, numEnemies, enemyPrefab);
    }

    // Creates Room 2, a simple room with walls and enemies on the left and right and enemies in the center
    private void InstantiateRoom2()
    {
        Instantiate(wallPrefab, new Vector3(7, wallHeight / 2, 7 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(-7, wallHeight / 2, 7 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);

        surface.BuildNavMesh();

        InstantiateRoom2Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom2Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(0f, 1f, 17f, Math.Min(remainder, 1), spawnerPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-13f, 1f, 1f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(13f, 1f, 1f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(0f, 1f, 17f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-13f, 1f, 1f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(13f, 1f, 1f, spawnsPerLoc, enemyPrefab);
    }


    // Creates Room 3, a simple room with spikes and enemies in the center
    private void InstantiateRoom3()
    {
        Instantiate(spikePrefab, new Vector3(0, 0, 15 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(18, 0.5f, 1f);

        surface.BuildNavMesh();

        InstantiateRoom3Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom3Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(0f, 1f, 10f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-4.3f, 1f, 23f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(4.3f, 1f, 23f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(0f, 1f, 10f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-4.3f, 1f, 23f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(4.3f, 1f, 23f, spawnsPerLoc, enemyPrefab);
    }

    // Creates Room 4, a maze-like room with long walls and traps
    private void InstantiateRoom4()
    {
        Instantiate(wallPrefab, new Vector3(-5, wallHeight / 2, 10 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(5, wallHeight / 2, 20 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        Instantiate(wallPrefab, new Vector3(-5, wallHeight / 2, 30 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);

        surface.BuildNavMesh();

        InstantiateRoom4Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom4Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-10f, 1f, 20f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(10f, 1f, 10f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(10f, 1f, 30f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(-10f, 1f, 20f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(10f, 1f, 10f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(4.3f, 1f, 30f, spawnsPerLoc, enemyPrefab);
    }

    // Creates Room 5, a room with spikes and a raised platform in the center
    private void InstantiateRoom5()
    {
        Instantiate(twoWayRaisedPlatformPrefab, new Vector3(2.3f, -0.1f, 15 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Instantiate(spikePrefab, new Vector3(0, 0, 10 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(-10, 0, 17 + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(10, 0, 17 + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        Instantiate(spikePrefab, new Vector3(0, 0, 25 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);

        surface.BuildNavMesh();

        InstantiateRoom5Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom5Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-0.35f, 2.3f, 16.74f, Math.Min(remainder, 1), spawnerPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-10f, 1f, 5f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(10f, 1f, 5f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(-0.35f, 2.3f, 16.74f, spawnsPerLoc, spawnerPrefab);
        SpawnEnemies(-10f, 1f, 5f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(10f, 1f, 5f, spawnsPerLoc, enemyPrefab);
    }

    // Creates room 6, a room with a large raised platform in the center and enemies in the corners of the platform
    private void InstantiateRoom6()
    {
        Instantiate(fourPointRampMapPrefab, new Vector3(-10.2f, 1.5f, 1.9f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        surface.BuildNavMesh();

        InstantiateRoom6Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom6Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 4;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        float y = 2.6f;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-7.4f, y, 5.5f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(8.6f, y, 5.5f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-7.4f, y, 26.9f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(8.6f, y, 26.9f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(-7.4f, y, 5.5f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(8.6f, y, 5.5f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-7.4f, y, 26.9f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(8.6f, y, 26.9f, spawnsPerLoc, enemyPrefab);
    }

    // Creates room 7, a room with enemies on raised platforms on both the left and right
    private void InstantiateRoom7()
    {
        Instantiate(cPlatformRightPrefab, new Vector3(4.4f, -1f, 28f + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        Instantiate(cPlatformLeftPrefab, new Vector3(-3f, -1f, 8.1f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        surface.BuildNavMesh();

        InstantiateRoom7Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom7Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 6;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        float y = 2.5f;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-10.5f, y, 14.3f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-10.5f, y, 20f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-5.14f, y, 25.5f, Math.Min(remainder, 1), spawnerPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(11.7f, y, 14.3f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(11.7f, y, 20f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(7.14f, y, 25.5f, Math.Min(remainder, 1), spawnerPrefab);
        }

        SpawnEnemies(-10.5f, y, 14.3f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-10.5f, y, 20f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-5.14f, y, 25.5f, spawnsPerLoc, spawnerPrefab);
        SpawnEnemies(11.7f, y, 14.3f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(11.7f, y, 20f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(7.14f, y, 25.5f, spawnsPerLoc, spawnerPrefab);
    }

    // Creates room 8, a complex room with a pit of spikes in the center, traps and enemies along the sides, and enemies on a platform across the room
    private void InstantiateRoom8()
    {
        Instantiate(twoWayRaisedPlatformPrefab, new Vector3(3.5f, 2.1f, -2.2f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(2f, 2f, 2f);
        Instantiate(cPlatformCenterPrefab, new Vector3(-10f, 1.86f, 30f + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        Instantiate(uFloorPrefab, new Vector3(-15f, -0.1f, 36f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        Instantiate(spikePrefab, new Vector3(-0.1f, 0, 19 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);
        Instantiate(spikeSafetyNetPrefab, new Vector3(-0.1f, 0, 19 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);

        surface.BuildNavMesh();

        InstantiateRoom8Enemies();

        StartCoroutine(Rebuild());
    }

    private void InstantiateRoom8Enemies()
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 4;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-10.1f, 3.1f, 15.2f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(10f, 3.1f, 15.2f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-5.75f, 5.6f, 32f, Math.Min(remainder, 1), enemyPrefab);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(5.75f, 5.6f, 32f, Math.Min(remainder, 1), enemyPrefab);
        }

        SpawnEnemies(-10.1f, 3.1f, 15.2f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(10f, 3.1f, 15.2f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(-5.75f, 5.6f, 32f, spawnsPerLoc, enemyPrefab);
        SpawnEnemies(5.75f, 5.6f, 32f, spawnsPerLoc, enemyPrefab);
    }

    // Spawns enemies at general (x, y, z) locations
    private void SpawnEnemies(float x, float yPos, float z, int timesToSpawn, GameObject prefabToSpawn)
    {
        float xPos;
        float zPos;

        for (int i = 0; i < timesToSpawn; i++)
        {
            xPos = baseX + x + UnityEngine.Random.Range(-1, 1);
            zPos = baseZ + z + UnityEngine.Random.Range(-2, 2);

            Instantiate(prefabToSpawn, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, 180, 0)).GetComponent<BaseEnemy>().SetStrength(enemyStrengths[numEnemiesToSpawn - 1]);

            numEnemiesToSpawn--;
        }
    }

    IEnumerator Rebuild()
    {
        yield return new WaitForSeconds(0.000001f);
        surface.BuildNavMesh();
    }
}
