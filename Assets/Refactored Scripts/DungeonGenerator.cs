using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator _instance;

    [SerializeField] private Transform dungeonRoot;

    [Header("Room Generation")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private float generateOffsetZ;
    [SerializeField] private float generateOffsetY;
    [SerializeField] private Vector3 initialGenOffset;

    private Vector3 nextGenPos;
    private GameObject currentRoom;
    private GameObject lastRoom;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        nextGenPos = initialGenOffset;

        GenRoom();
    }

    public void GenRoom()
    {
        lastRoom = currentRoom;

        currentRoom = Instantiate(roomPrefab, nextGenPos, Quaternion.identity, dungeonRoot);
        nextGenPos += Vector3.forward * generateOffsetZ + Vector3.down * generateOffsetY;
    }

    public void DestroyRoom()
    {
        if (lastRoom) { Destroy(lastRoom); }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        if (Application.isPlaying)
        {
            Gizmos.DrawCube(nextGenPos, new Vector3(20, 1, 20));
        }
        else
        {
            Gizmos.DrawCube(initialGenOffset + Vector3.down * 0.5f, new Vector3(20.01f, 1.01f, 20.01f));
        }
    }
}
