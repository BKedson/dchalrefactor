using System;
using System.Collections;
using UnityEngine;

public enum MathTopic
{
    Addition, Subtraction, Multiplication, Division, Fraction
}

[Serializable]
struct LootType
{
    [SerializeField] public string name;
    [SerializeField] public GameObject lootPrefab;
    [SerializeField] public float generationChance;
}

struct MapUnit
{
    public bool roomPlanned;
    public int lootTypeIdx;
}

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator _instance;

    [SerializeField] private Transform dungeonRoot;

    [Header("Room Generation")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField, Min(2), Tooltip("Min / Max")] private Vector2Int numRoomPerLv;
    [SerializeField, Min(2)] private int maxRecursionDepth;
    [SerializeField] private float roomOffset;

    [Header("Loot Generation")]
    [SerializeField] LootType[] lootTypes;

    [Header("Exit Generation")]
    [SerializeField] private GameObject exitPrefab;

    [Header("Math Operation")]
    [SerializeField] private MathTopic mathTopic;

    private int numRoomPlanned = 0;
    private MapUnit[,] map;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        InspectorValueAssertion();
    }

    private void InspectorValueAssertion()
    {
        numRoomPerLv.y = Mathf.Max(numRoomPerLv.x, numRoomPerLv.y);
        while (Mathf.Pow(2, maxRecursionDepth) - 1 < numRoomPerLv.x) maxRecursionDepth++;
    }

    public MathTopic GetTopic()
    {
        return mathTopic;
    }

    public void SetTopic(MathTopic op)
    {
        mathTopic = op;
    }

    public void InitializeDungeon()
    {
        StartCoroutine("InitiationCoroutine");
    }

    private IEnumerator InitiationCoroutine()
    {
        PlayerMovement._instance.enabled = false;

        UIController._instance.StartTransition();
        yield return new WaitForSeconds(UIController._instance.GetTransitionAnimLength());

        DestroyDungeon();

        InspectorValueAssertion();

        while (numRoomPlanned < numRoomPerLv.x)
        {
            numRoomPlanned = 0;
            map = new MapUnit[numRoomPerLv.y, numRoomPerLv.y];
            for (int i = 0; i < numRoomPerLv.y; i++)
            {
                for (int j = 0; j < numRoomPerLv.y; j++)
                {
                    map[i, j].roomPlanned = false;
                    map[i, j].lootTypeIdx = -1;
                }
            }
            map[0, 0].roomPlanned = true;
            numRoomPlanned++;
            RecursivePlanRoom(Vector2Int.zero);
        }

        for (int i = 0; i < numRoomPerLv.y; i++)
        {
            for (int j = 0; j < numRoomPerLv.y; j++)
            {
                if (map[i, j].roomPlanned) GenRoom(i, j);
            }
        }

        PlayerMovement._instance.MoveToDungeon();
        PlayerMovement._instance.enabled = true;

        UIController._instance.EndTransition();
    }

    private void RecursivePlanRoom(Vector2Int currPos, int depth = 1)
    {
        if (depth > maxRecursionDepth || numRoomPlanned >= numRoomPerLv.y) return;
        // Front
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.8f)
        {
            Vector2Int temp = currPos;
            temp.y++;
            if (!map[temp.x, temp.y].roomPlanned) { PlanRoom(temp, depth); }
        }
        // Right
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.6f)
        {
            Vector2Int temp = currPos;
            temp.x++;
            if (!map[temp.x, temp.y].roomPlanned) { PlanRoom(temp, depth); }
        }
        // Left
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
        {
            Vector2Int temp = currPos;
            temp.x--;
            if (temp.x >= 0 && !map[temp.x, temp.y].roomPlanned) { PlanRoom(temp, depth); }
        }
        // Back
        if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f)
        {
            Vector2Int temp = currPos;
            temp.y--;
            if (temp.y >= 0 && !map[temp.x, temp.y].roomPlanned) { PlanRoom(temp, depth); }
        }
    }

    private void PlanRoom(Vector2Int temp, int depth)
    {
        map[temp.x, temp.y].roomPlanned = true;
        numRoomPlanned++;

        if (numRoomPlanned == numRoomPerLv.y)
        {
            map[temp.x, temp.y].lootTypeIdx = -2;
        }
        else
        {
            float genChance = UnityEngine.Random.Range(0.0f, 1.0f);
            for (int i = 0; i < lootTypes.Length; i++)
            {
                if (genChance < lootTypes[i].generationChance)
                {
                    map[temp.x, temp.y].lootTypeIdx = i;
                    break;
                }
            }
        }

        RecursivePlanRoom(temp, depth + 1);
    }

    public void DestroyDungeon()
    {
        for (int i = 0; i < dungeonRoot.childCount; i++)
        {
            Destroy(dungeonRoot.GetChild(i).gameObject);
        }

        numRoomPlanned = 0;
    }

    public void GenRoom(int x, int z)
    {
        Vector3 targetPos = new Vector3(x * roomOffset, 0f, z * roomOffset);

        GameObject room = Instantiate(roomPrefab, dungeonRoot);
        room.transform.position = targetPos;

        if (map[x, z].lootTypeIdx == -2)
        {
            Instantiate(exitPrefab, targetPos + Vector3.up, Quaternion.identity, room.transform);
        }
        else if (map[x, z].lootTypeIdx > -1)
            Instantiate(lootTypes[map[x, z].lootTypeIdx].lootPrefab, targetPos + Vector3.up, Quaternion.identity, room.transform);

        // Left
        if (x > 0 && map[x - 1, z].roomPlanned)
        {
            GameObject doorObj = Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 270f, 0f), room.transform);
        }
        // Right
        if (x < numRoomPerLv.y - 1 && map[x + 1, z].roomPlanned)
        {
            GameObject doorObj = Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 90f, 0f), room.transform);
        }
        // Back
        if (z > 0 && map[x, z - 1].roomPlanned)
        {
            GameObject doorObj = Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 180f, 0f), room.transform);
        }
        // Front
        if (z < numRoomPerLv.y - 1 && map[x, z + 1].roomPlanned)
        {
            GameObject doorObj = Instantiate(doorPrefab, targetPos, Quaternion.identity, room.transform);
        }
    }

    //private void OnDrawGizmos()
    //{

    //}
}
