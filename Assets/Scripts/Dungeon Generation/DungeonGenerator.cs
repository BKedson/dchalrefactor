using System.Collections.Generic;
using UnityEngine;

public enum MathType
{
    Addition, Subtraction, Multiplication, Division, Fraction
}

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator _instance;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField, Min(2), Tooltip("Min / Max")] private Vector2Int numRoomPerLv;
    [SerializeField, Min(2)] private int maxRecursionDepth;
    [SerializeField] private float roomOffset;
    [SerializeField] private Transform dungeonRoot;
    //[SerializeField] private LayerMask whatIsRoom;

    [Header("Exit Generation")]
    [SerializeField] private GameObject exitPrefab;

    //[Header("Normal Loot")]
    //[Range(0f, 1f)]
    //[SerializeField] private float normalLootChance;
    //[SerializeField] private GameObject normalLootPrefab;

    //[Header("Rare Loot")]
    //[Range(0f, 1f)]
    //[SerializeField] private float rareLootChance;
    //[SerializeField] private GameObject rareLootPrefab;

    //[Header("Clue")]
    //[Range(0f, 1f)]
    //[SerializeField] private float clueChance;
    //[SerializeField] private GameObject cluePrefab;

    [Header("Math Operation")]
    [SerializeField] private MathType mathOperation;

    private int numRoomPlanned = 0;
    private bool[,] map;

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

    public MathType GetOperation()
    {
        return mathOperation;
    }

    public void SetOperation(MathType op)
    {
        mathOperation = op;
    }

    public void InitializeDungeon()
    {
        DestroyDungeon();

        InspectorValueAssertion();

        while (numRoomPlanned < numRoomPerLv.x)
        {
            numRoomPlanned = 0;
            map = new bool[numRoomPerLv.y, numRoomPerLv.y];
            map[0, 0] = true;
            numRoomPlanned++;
            RecursivePlanRoom(Vector2Int.zero);
        }

        for (int i = 0; i < numRoomPerLv.y; i++)
        {
            for (int j = 0; j < numRoomPerLv.y; j++)
            {
                if (map[i, j]) GenRoom(i, j);
            }
        }
    }

    private void RecursivePlanRoom(Vector2Int currPos, int depth = 1)
    {
        if (depth > maxRecursionDepth) return;
        // Front
        if (numRoomPlanned < numRoomPerLv.y && Random.Range(0.0f, 1.0f) < 0.8f)
        {
            Vector2Int temp = currPos;
            temp.y++;
            if (!map[temp.x, temp.y])
            {
                map[temp.x, temp.y] = true;
                numRoomPlanned++;
                RecursivePlanRoom(temp, depth + 1);
            }
        }
        // Right
        if (numRoomPlanned < numRoomPerLv.y && Random.Range(0.0f, 1.0f) < 0.6f)
        {
            Vector2Int temp = currPos;
            temp.x++;
            if (!map[temp.x, temp.y])
            {
                map[temp.x, temp.y] = true;
                numRoomPlanned++;
                RecursivePlanRoom(temp, depth + 1);
            }
        }
        // Left
        if (numRoomPlanned < numRoomPerLv.y && Random.Range(0.0f, 1.0f) < 0.5f)
        {
            Vector2Int temp = currPos;
            temp.x--;
            if (temp.x >= 0 && !map[temp.x, temp.y])
            {
                map[temp.x, temp.y] = true;
                numRoomPlanned++;
                RecursivePlanRoom(temp, depth + 1);
            }
        }
        // Back
        if (numRoomPlanned < numRoomPerLv.y && Random.Range(0.0f, 1.0f) < 0.5f)
        {
            Vector2Int temp = currPos;
            temp.y--;
            if (temp.y >= 0 && !map[temp.x, temp.y])
            {
                map[temp.x, temp.y] = true;
                numRoomPlanned++;
                RecursivePlanRoom(temp, depth + 1);
            }
        }
    }

    public void DestroyDungeon()
    {
        for (int i = 0; i < dungeonRoot.childCount; i++)
        {
            Destroy(dungeonRoot.GetChild(i).gameObject);
        }

        numRoomPlanned = 0;
    }

    //private Tuple<string, string> GenMathQA(MathType t)
    //{
    //    int a, b;
    //    switch (t)
    //    {
    //        case MathType.Addition:
    //            a = UnityEngine.Random.Range(0, 300);
    //            b = UnityEngine.Random.Range(0, 300);
    //            return Tuple.Create(a + " + " + b + " = ?", (a + b).ToString());
    //        case MathType.Subtraction:
    //            a = UnityEngine.Random.Range(0, 300);
    //            b = UnityEngine.Random.Range(0, 200);
    //            return Tuple.Create(a + " - " + b + " = ?", (a - b).ToString());
    //        case MathType.Multiplication:
    //            a = UnityEngine.Random.Range(0, 10);
    //            b = UnityEngine.Random.Range(0, 10);
    //            return Tuple.Create(a + " X " + b + " = ?", (a * b).ToString());
    //        case MathType.Division:
    //            a = UnityEngine.Random.Range(1, 10);
    //            b = UnityEngine.Random.Range(1, 10);
    //            return Tuple.Create(a * b + " / " + b + " = ?", a.ToString());
    //        case MathType.Fraction:
    //            a = UnityEngine.Random.Range(0, 10);
    //            b = UnityEngine.Random.Range(1, 10);
    //            int k = UnityEngine.Random.Range(2, 10);
    //            string q = a * k + "/" + b * k + " = ?\n\n";
    //            string ans = a + "/" + b;
    //            int currectAnsIdx = UnityEngine.Random.Range(1, 4);
    //            for (int i = 1; i < 5; i++)
    //            {
    //                if (i == currectAnsIdx)
    //                {
    //                    q += "  " + i + ". " + ans + "  ";
    //                }
    //                else
    //                {
    //                    a = UnityEngine.Random.Range(0, 100);
    //                    b = UnityEngine.Random.Range(1, 10);
    //                    q += "  " + i + ". " + a + "/" + b + "  ";
    //                }
    //            }
    //            return Tuple.Create(q, currectAnsIdx.ToString());
    //    }

    //    return Tuple.Create("", "");
    //}

    public void GenRoom(int x, int z)
    {
        Vector3 targetPos = new Vector3(x * roomOffset, 0f, z * roomOffset);

        GameObject room = Instantiate(roomPrefab, dungeonRoot);
        room.transform.position = targetPos;

        // Left
        if (x > 0 && map[x - 1, z])
            Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 270f, 0f), room.transform);
        // Right
        if (x < numRoomPerLv.y - 1 && map[x + 1, z])
            Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 90f, 0f), room.transform);
        // Back
        if (z > 0 && map[x, z - 1])
            Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 180f, 0f), room.transform);
        // Front
        if (z < numRoomPerLv.y - 1 && map[x, z + 1])
            Instantiate(doorPrefab, targetPos, Quaternion.Euler(0f, 0f, 0f), room.transform);
    }

    //private void OnDrawGizmos()
    //{

    //}
}
