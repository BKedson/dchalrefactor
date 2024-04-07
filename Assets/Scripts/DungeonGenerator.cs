using UnityEngine;


public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator _instance;

    [SerializeField] private Transform dungeonRoot;

    [Header("Room Generation")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Vector3 initialGenOffset;
    [SerializeField] private Vector3 genOffset;

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
        nextGenPos += genOffset;
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
            Gizmos.DrawCube(nextGenPos + new Vector3(0, 0, 10), new Vector3(20, 1, 20));
        }
        else
        {
            Gizmos.DrawCube(initialGenOffset + new Vector3(0, 0, 10), new Vector3(20, 1, 20));
        }
    }
}
