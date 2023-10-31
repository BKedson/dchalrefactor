using System;
using System.Collections.Generic;
using UnityEngine;

public enum MathType
{
    Addition, Subtraction, Multiplication, Division, Fraction
}

[Serializable]
struct RoomTemplate
{
    public GameObject prefab;
    public List<Vector2Int> extraColCheckOffset;
}

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator _instance;

    [SerializeField] private LayerMask whatIsRoom;

    [SerializeField] private float roomWidth;
    [SerializeField] private float roomGenOffsetDist;
    [SerializeField] private List<RoomTemplate> roomPrefabs;
    [SerializeField] private List<Tuple<GameObject, List<Vector2Int>>> templates;
    [SerializeField] private Transform dungeonRoot;

    [Header("Normal Loot")]
    [Range(0f, 1f)]
    [SerializeField] private float normalLootChance;
    [SerializeField] private GameObject normalLootPrefab;

    [Header("Normal Loot")]
    [Range(0f, 1f)]
    [SerializeField] private float rareLootChance;
    [SerializeField] private GameObject rareLootPrefab;

    [Header("Normal Loot")]
    [Range(0f, 1f)]
    [SerializeField] private float clueChance;
    [SerializeField] private GameObject cluePrefab;

    [Header("Normal Loot")]
    [Range(0f, 1f)]
    [SerializeField] private float exitChance;
    [SerializeField] private GameObject exitPrefab;

    [SerializeField] private MathType mathOperation;

    private Transform transformInfoToGenRoom = null;
    private GameObject doorToDisable = null;
    private bool toGenRoom = false;
    int roomPrefabIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public MathType GetOperation()
    {
        return mathOperation;
    }

    public void SetOperation(MathType op)
    {
        mathOperation = op;
    }

    public void PrepareToGenRoom(Transform originatorDoorTransform, GameObject door)
    {
        transformInfoToGenRoom = originatorDoorTransform;
        doorToDisable = door;
        toGenRoom = true;
    }

    public void InitializeDungeon()
    {
        DestroyDungeon();
        SetupRoomAndPuzzle(Vector3.zero, 0);
    }

    private Transform SetupRoomAndPuzzle(Vector3 targetPos, int roomPrefabIdx)
    {
        Transform roomTransform =
            Instantiate(roomPrefabs[roomPrefabIdx].prefab, targetPos, Quaternion.identity, dungeonRoot).transform;
        foreach (DoorController dc in roomTransform.GetComponentsInChildren<DoorController>())
        {
            Tuple<string, string> qa = GenMathQA();
            dc.SetPuzzle(qa.Item1, qa.Item2);
        }

        return roomTransform;
    }

    private Tuple<string, string> GenMathQA(MathType t)
    {
        int a, b;
        switch (t)
        {
            case MathType.Addition:
                a = UnityEngine.Random.Range(0, 300);
                b = UnityEngine.Random.Range(0, 300);
                return Tuple.Create(a + " + " + b + " = ?", (a + b).ToString());
            case MathType.Subtraction:
                a = UnityEngine.Random.Range(0, 300);
                b = UnityEngine.Random.Range(0, 200);
                return Tuple.Create(a + " - " + b + " = ?", (a - b).ToString());
            case MathType.Multiplication:
                a = UnityEngine.Random.Range(0, 10);
                b = UnityEngine.Random.Range(0, 10);
                return Tuple.Create(a + " X " + b + " = ?", (a * b).ToString());
            case MathType.Division:
                a = UnityEngine.Random.Range(1, 10);
                b = UnityEngine.Random.Range(1, 10);
                return Tuple.Create(a * b + " / " + b + " = ?", a.ToString());
            case MathType.Fraction:
                a = UnityEngine.Random.Range(0, 10);
                b = UnityEngine.Random.Range(1, 10);
                int k = UnityEngine.Random.Range(2, 10);
                string q = a * k + "/" + b * k + " = ?\n\n";
                string ans = a + "/" + b;
                int currectAnsIdx = UnityEngine.Random.Range(1, 4);
                for (int i = 1; i < 5; i++)
                {
                    if (i == currectAnsIdx)
                    {
                        q += "  " + i + ". " + ans + "  ";
                    }
                    else
                    {
                        a = UnityEngine.Random.Range(0, 100);
                        b = UnityEngine.Random.Range(1, 10);
                        q += "  " + i + ". " + a + "/" + b + "  ";
                    }
                }
                return Tuple.Create(q, currectAnsIdx.ToString());
        }

        return Tuple.Create("", "");
    }

    private Tuple<string, string> GenMathQA()
    {
        return GenMathQA(mathOperation);
    }

    public void OnCorrectAnswer()
    {
        Vector3 targetPos = transformInfoToGenRoom.position + transformInfoToGenRoom.forward * roomGenOffsetDist;
        if (!Physics.CheckBox(
                targetPos,
                Vector3.one,
                Quaternion.identity,
                whatIsRoom
        ))
        {
            while (roomPrefabs.Count > 1)
            {
                bool noCollision = true;
                roomPrefabIdx = UnityEngine.Random.Range(0, roomPrefabs.Count - 1);
                foreach (Vector2Int offset in roomPrefabs[roomPrefabIdx].extraColCheckOffset)
                {
                    Vector3 checkPos = targetPos +
                        transformInfoToGenRoom.right * offset.x * roomWidth +
                        transformInfoToGenRoom.forward * offset.y * roomWidth;
                    noCollision &= !Physics.CheckBox(
                        checkPos,
                        Vector3.one,
                        Quaternion.identity,
                        whatIsRoom
                    );
                }
                if (noCollision) { break; }
            }

            Transform roomTransform = SetupRoomAndPuzzle(targetPos, roomPrefabIdx);
            roomTransform.rotation = transformInfoToGenRoom.rotation;

            if (UnityEngine.Random.Range(0f, 1f) < normalLootChance)
            {
                Instantiate(normalLootPrefab, targetPos + Vector3.up, Quaternion.identity, roomTransform); ;
            }
            else if (UnityEngine.Random.Range(0f, 1f) < rareLootChance)
            {
                GameObject obj = Instantiate(rareLootPrefab, targetPos + Vector3.up, Quaternion.identity, roomTransform);
                Tuple<string, string> qa = GenMathQA(MathType.Fraction);
                obj.GetComponent<RareLootController>().SetUpQuestion(qa.Item1, qa.Item2);
            }
            else if (UnityEngine.Random.Range(0f, 1f) < clueChance)
            {
                Instantiate(cluePrefab, targetPos + Vector3.up, Quaternion.identity, roomTransform);
            }
            else if (UnityEngine.Random.Range(0f, 1f) < exitChance)
            {
                Instantiate(exitPrefab, targetPos + Vector3.up, Quaternion.identity, roomTransform);
            }
        }

        if (doorToDisable) doorToDisable.SetActive(false);

        toGenRoom = false;
    }

    public void OnLeaveDungeon()
    {
        PlayerInventory._instance.DropAllClue();
        PlayerMovement._instance.LeaveDungeon();
        DestroyDungeon();
        UIManager._instance.ActivateDungeonExitComfirmation(false);

        TrainingRoomManager._instance.UnlockTrainingRoom();
    }

    public void DestroyDungeon()
    {
        for (int i = 0; i < dungeonRoot.childCount; i++)
        {
            Destroy(dungeonRoot.GetChild(i).gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (transformInfoToGenRoom)
        {
            Vector3 targetPos = transformInfoToGenRoom.position + transformInfoToGenRoom.forward * roomGenOffsetDist;
            Gizmos.DrawWireCube(targetPos, new Vector3(2f, 2f, 2f));
            foreach (Vector2Int offset in roomPrefabs[roomPrefabIdx].extraColCheckOffset)
            {
                Vector3 checkPos = targetPos +
                    transformInfoToGenRoom.right * offset.x * roomWidth +
                    transformInfoToGenRoom.forward * offset.y * roomWidth;
                Gizmos.DrawWireCube(checkPos, new Vector3(2f, 2f, 2f));
            }
        }
    }
}
