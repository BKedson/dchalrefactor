using System.Collections;
using UnityEngine;
using UnityEngine.UI;


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

    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        nextGenPos = initialGenOffset;

        GenRoom();

        player = GameObject.Find("Player");
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

    public void ProceedLv()
    {
        StartCoroutine("ProceedLvTransition");
    }

    IEnumerator ProceedLvTransition()
    {
        TransitionUIManager._instance.StartTransition();

        TransitionUIManager._instance.StartTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetStartTransitionSpan());

        lastRoom = currentRoom;

        currentRoom = Instantiate(roomPrefab, nextGenPos, Quaternion.identity, dungeonRoot);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = nextGenPos + new Vector3(0f, 0.2f, 2f);
        player.GetComponent<CharacterController>().enabled = true;
        nextGenPos += genOffset;

        TransitionUIManager._instance.EndTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetEndTransitionSpan());
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
