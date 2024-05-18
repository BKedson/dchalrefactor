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
    [SerializeField] private GameObject combatArea1Prefab;
    [SerializeField] private GameObject combatArea2Prefab;
    [SerializeField] private GameObject combatArea3Prefab;
    [SerializeField] private GameObject combatArea4Prefab;
    [SerializeField] private GameObject combatArea5Prefab;
    [SerializeField] private GameObject combatArea6Prefab;
    [SerializeField] private GameObject combatArea7Prefab;
    [SerializeField] private GameObject combatArea8Prefab;

    // [SerializeField] private GameObject spikePrefab;
    // [SerializeField] private GameObject twoWayRaisedPlatformPrefab;
    // [SerializeField] private GameObject fourPointRampMapPrefab;
    // [SerializeField] private GameObject cPlatformRightPrefab;
    // [SerializeField] private GameObject cPlatformLeftPrefab;
    // [SerializeField] private GameObject cPlatformCenterPrefab;
    // [SerializeField] private GameObject uFloorPrefab;
    // [SerializeField] private GameObject spikeSafetyNetPrefab;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private GameObject referenceObject;

    private Transform foundryParent;

    // The minimum and maximum difficulty room that can spawn with the current settings 
    private int easiestRoom = 2;
    private int hardestRoom = 7;

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
    private float baseY = -0.083f;
    private float baseZ = 0;
    private int yAngle = 90;
    private int referenceThickness = 4;

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
            foundryParent = player.transform;
        } else {
            foundryParent = referenceObject.GetComponentInParent<FoundryRoom>().gameObject.transform;
        }

        baseX = referenceObject.transform.position.x;
        //(33.878 - 30.278f)
        baseZ = referenceObject.transform.position.z - referenceThickness / 2 - 3.6f;

        int additionalShift = referenceThickness / 2;

        if (yAngle == 0 || yAngle % 180 == 0) {
            baseZ += additionalShift;
        } else {
            baseX += additionalShift;
        }

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
        GameObject combatArea1 = Instantiate(combatArea1Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);
        
        numEnemiesToSpawn = numEnemies;

        // surface.BuildNavMesh();

        SpawnEnemies(5f, 1f, 13f, numEnemies, enemyPrefab, combatArea1);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    // Creates Room 2, a simple room with walls and enemies on the left and right and enemies in the center
    private void InstantiateRoom2()
    {
        // Instantiate(wallPrefab, new Vector3(7, wallHeight / 2, 7 + baseZ), Quaternion.Euler(0, 180 + yAngle, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);
        // Instantiate(wallPrefab, new Vector3(-7, wallHeight / 2, 7 + baseZ), Quaternion.Euler(0, 180  + yAngle, 0)).transform.localScale = new Vector3(8, wallHeight, wallThickness);

        GameObject combatArea2 = Instantiate(combatArea2Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom2Enemies(combatArea2);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom2Enemies(GameObject combatArea2)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(5f, 1f, 14f, Math.Min(remainder, 1), spawnerPrefab, combatArea2);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(1f, 1f, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea2);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(11f, 1f, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea2);
        }

        SpawnEnemies(0f, 1f, 20f, spawnsPerLoc, enemyPrefab, combatArea2);
        SpawnEnemies(-11f, 1f, 5f, spawnsPerLoc, enemyPrefab, combatArea2);
        SpawnEnemies(11f, 1f, 5f, spawnsPerLoc, enemyPrefab, combatArea2);
    }


    // Creates Room 3, a simple room with spikes and enemies in the center
    private void InstantiateRoom3()
    {
        // Instantiate(spikePrefab, new Vector3(0, 0, 15 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(18, 0.5f, 1f);
        GameObject combatArea3 = Instantiate(combatArea3Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom3Enemies(combatArea3);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom3Enemies(GameObject combatArea3)
    {

        SpawnEnemies(5f, 1f, 13f, numEnemies, enemyPrefab, combatArea3);


        // numEnemiesToSpawn = numEnemies;
        // numSpawns = 3;
        // spawnsPerLoc = numEnemies / numSpawns;
        // remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        // if (remainder != 0)
        // {
        //     SpawnEnemies(0f, 1f, 10f, Math.Min(remainder, 1), enemyPrefab, combatArea3);
        //     remainder--;
        //     remainder = Math.Max(0, remainder);
        //     SpawnEnemies(-4.3f, 1f, 23f, Math.Min(remainder, 1), enemyPrefab, combatArea3);
        //     remainder--;
        //     remainder = Math.Max(0, remainder);
        //     SpawnEnemies(4.3f, 1f, 23f, Math.Min(remainder, 1), enemyPrefab, combatArea3);
        // }

        // SpawnEnemies(0f, 1f, 10f, spawnsPerLoc, enemyPrefab, combatArea3);
        // SpawnEnemies(-4.3f, 1f, 23f, spawnsPerLoc, enemyPrefab, combatArea3);
        // SpawnEnemies(4.3f, 1f, 23f, spawnsPerLoc, enemyPrefab, combatArea3);
    }

    // Creates Room 4, a maze-like room with long walls and traps
    private void InstantiateRoom4()
    {
        // Instantiate(wallPrefab, new Vector3(-5, wallHeight / 2, 10 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        // Instantiate(wallPrefab, new Vector3(5, wallHeight / 2, 20 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        // Instantiate(wallPrefab, new Vector3(-5, wallHeight / 2, 30 + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(20, wallHeight, wallThickness);
        GameObject combatArea4 = Instantiate(combatArea4Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom4Enemies(combatArea4);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom4Enemies(GameObject combatArea4)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-4f, 1f, 14f, Math.Min(remainder, 1), enemyPrefab, combatArea4);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(13f, 1f, 9.5f, Math.Min(remainder, 1), enemyPrefab, combatArea4);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(14f, 1f, 20f, Math.Min(remainder, 1), enemyPrefab, combatArea4);
        }

        SpawnEnemies(-4f, 1f, 14f, spawnsPerLoc, enemyPrefab, combatArea4);
        SpawnEnemies(13f, 1f, 9.5f, spawnsPerLoc, enemyPrefab, combatArea4);
        SpawnEnemies(14f, 1f, 20f, spawnsPerLoc, enemyPrefab, combatArea4);
    }

    // Creates Room 5, a room with spikes and a raised platform in the center
    private void InstantiateRoom5()
    {
        // Instantiate(twoWayRaisedPlatformPrefab, new Vector3(2.3f, -0.1f, 15 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // Instantiate(spikePrefab, new Vector3(0, 0, 10 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);
        // Instantiate(spikePrefab, new Vector3(-10, 0, 17 + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        // Instantiate(spikePrefab, new Vector3(10, 0, 17 + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(5f, 0.5f, 1f);
        // Instantiate(spikePrefab, new Vector3(0, 0, 25 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 1f);

        GameObject combatArea5 = Instantiate(combatArea5Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom5Enemies(combatArea5);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom5Enemies(GameObject combatArea5)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 3;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(5.5f, 2.5f, 12.5f, Math.Min(remainder, 1), spawnerPrefab, combatArea5);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-1f, 1f, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea5);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(5f, 1f, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea5);
        }

        SpawnEnemies(5.5f, 2.5f, 12.5f, spawnsPerLoc, spawnerPrefab, combatArea5);
        SpawnEnemies(-1f, 1f, 7f, spawnsPerLoc, enemyPrefab, combatArea5);
        SpawnEnemies(5f, 1f, 7f, spawnsPerLoc, enemyPrefab, combatArea5);
    }

    // Creates room 6, a room with a large raised platform in the center and enemies in the corners of the platform
    private void InstantiateRoom6()
    {
        // Instantiate(fourPointRampMapPrefab, new Vector3(-10.2f, 1.5f, 1.9f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        GameObject combatArea6 = Instantiate(combatArea6Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom6Enemies(combatArea6);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom6Enemies(GameObject combatArea6)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 6;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        float y = 2.6f;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(7f, y, 14f, Math.Min(remainder, 1), enemyPrefab, combatArea6);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-3.5f, y, 11f, Math.Min(remainder, 1), spawnerPrefab, combatArea6);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(15f, y, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea6);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(15.5f, y, 16f, Math.Min(remainder, 1), enemyPrefab, combatArea6);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(16.5f, y, 26f, Math.Min(remainder, 1), spawnerPrefab, combatArea6);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(7f, y, 22f, Math.Min(remainder, 1), enemyPrefab, combatArea6);
        }

        SpawnEnemies(7f, y, 14f, spawnsPerLoc, enemyPrefab, combatArea6);
        SpawnEnemies(-3.5f, y, 11f, spawnsPerLoc, enemyPrefab, combatArea6);
        SpawnEnemies(15f, y, 7f, spawnsPerLoc, spawnerPrefab, combatArea6);
        SpawnEnemies(15.5f, y, 16f, spawnsPerLoc, enemyPrefab, combatArea6);
        SpawnEnemies(16.5f, y, 26f, spawnsPerLoc, enemyPrefab, combatArea6);
        SpawnEnemies(7f, y, 22f, spawnsPerLoc, enemyPrefab, combatArea6);
    }

    // Creates room 7, a room with enemies on raised platforms on both the left and right
    private void InstantiateRoom7()
    {
        // Instantiate(cPlatformRightPrefab, new Vector3(4.4f, -1f, 28f + baseZ), Quaternion.Euler(0, 180, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        // Instantiate(cPlatformLeftPrefab, new Vector3(-3f, -1f, 8.1f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        GameObject combatArea7 = Instantiate(combatArea7Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom7Enemies(combatArea7);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom7Enemies(GameObject combatArea7)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 4;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        float y = 2.5f;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(11f, y, 7f, Math.Min(remainder, 1), enemyPrefab, combatArea7);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(11f, y, 17.5f, Math.Min(remainder, 1), spawnerPrefab, combatArea7);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-2.5f, y, 11.5f, Math.Min(remainder, 1), enemyPrefab, combatArea7);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(0.5f, y, 21.5f, Math.Min(remainder, 1), spawnerPrefab, combatArea7);
        }

        SpawnEnemies(11f, y, 7f, spawnsPerLoc, enemyPrefab, combatArea7);
        SpawnEnemies(11f, y, 17.5f, spawnsPerLoc, spawnerPrefab, combatArea7);
        SpawnEnemies(-2.5f, y, 11.5f, spawnsPerLoc, enemyPrefab, combatArea7);
        SpawnEnemies(0.5f, y, 21.5f, spawnsPerLoc, spawnerPrefab, combatArea7);
    }

    // Creates room 8, a complex room with a pit of spikes in the center, traps and enemies along the sides, and enemies on a platform across the room
    private void InstantiateRoom8()
    {
        // Instantiate(twoWayRaisedPlatformPrefab, new Vector3(3.5f, 2.1f, -2.2f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(2f, 2f, 2f);
        // Instantiate(cPlatformCenterPrefab, new Vector3(-10f, 1.86f, 30f + baseZ), Quaternion.Euler(0, 90, 0)).transform.localScale = new Vector3(1f, 1f, 1f);
        // Instantiate(uFloorPrefab, new Vector3(-15f, -0.1f, 36f + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(1f, 1f, 1f);

        // Instantiate(spikePrefab, new Vector3(-0.1f, 0, 19 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);
        // Instantiate(spikeSafetyNetPrefab, new Vector3(-0.1f, 0, 19 + baseZ), Quaternion.Euler(0, 0, 0)).transform.localScale = new Vector3(10f, 0.5f, 35f);

        GameObject combatArea8 = Instantiate(combatArea8Prefab, new Vector3(baseX, baseY, baseZ), Quaternion.Euler(0, yAngle, 0), foundryParent);

        // surface.BuildNavMesh();

        InstantiateRoom8Enemies(combatArea8);

        surface.BuildNavMesh();

        // StartCoroutine(Rebuild());
    }

    private void InstantiateRoom8Enemies(GameObject combatArea8)
    {
        numEnemiesToSpawn = numEnemies;
        numSpawns = 4;
        spawnsPerLoc = numEnemies / numSpawns;
        remainder = numEnemies % numSpawns;

        // Assigns extra enemies to some spawns if there cannot be an even number of enemies per spawn
        if (remainder != 0)
        {
            SpawnEnemies(-10.1f, 3.1f, 15.2f, Math.Min(remainder, 1), enemyPrefab, combatArea8);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(10f, 3.1f, 15.2f, Math.Min(remainder, 1), enemyPrefab, combatArea8);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(-5.75f, 5.6f, 32f, Math.Min(remainder, 1), enemyPrefab, combatArea8);
            remainder--;
            remainder = Math.Max(0, remainder);
            SpawnEnemies(5.75f, 5.6f, 32f, Math.Min(remainder, 1), enemyPrefab, combatArea8);
        }

        SpawnEnemies(-10.1f, 3.1f, 15.2f, spawnsPerLoc, enemyPrefab, combatArea8);
        SpawnEnemies(10f, 3.1f, 15.2f, spawnsPerLoc, enemyPrefab, combatArea8);
        SpawnEnemies(-5.75f, 5.6f, 32f, spawnsPerLoc, enemyPrefab, combatArea8);
        SpawnEnemies(5.75f, 5.6f, 32f, spawnsPerLoc, enemyPrefab, combatArea8);
    }

    // Spawns enemies at general (x, y, z) locations
    private void SpawnEnemies(float x, float y, float z, int timesToSpawn, GameObject prefabToSpawn, GameObject parent)
    {
        float xPos;
        float yPos;
        float zPos;
        Transform parentTransform = parent.transform;

        for (int i = 0; i < timesToSpawn; i++)
        {
            xPos = x + UnityEngine.Random.Range(-1.0f, 1.0f);
            yPos = y;
            zPos = z + UnityEngine.Random.Range(-2.0f, 2.0f);

            GameObject currEnemy = Instantiate(prefabToSpawn, new Vector3(0, 0, 0), Quaternion.Euler(0, 180, 0), parentTransform);
            currEnemy.transform.localPosition = new Vector3(xPos, yPos, zPos);
            currEnemy.transform.localScale = new Vector3(1f, 1f, 1f);
            currEnemy.GetComponent<BaseEnemy>().SetStrength(enemyStrengths[numEnemiesToSpawn - 1]);

            numEnemiesToSpawn--;
        }
    }

    IEnumerator Rebuild()
    {
        yield return new WaitForSeconds(0.000001f);
        surface.BuildNavMesh();
    }

    // GETTERS AND SETTERS

    public void SetYAngle(int newAngle) {
        yAngle = newAngle;
    }
}
