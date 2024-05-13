using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// A dungeon/room layout generator
public class DungeonGenerator : MonoBehaviour
{
    // Singleton
    public static DungeonGenerator _instance;

    // The root transform of all the generated room objects
    [SerializeField] private Transform dungeonRoot;

    [Header("Room Generation")]
    // Room prefab
    [SerializeField] private GameObject roomPrefab;
    // The location to generate the first room.
    // Adjust this value if the first room is not to be generated at (0f, 0f, 0f)
    [SerializeField] private Vector3 initialGenOffset;
    // The offset between each generated room
    [SerializeField] private Vector3 genOffset;

    // Private helpers
    // The location to generate the next room
    private Vector3 nextGenPos;
    // A reference of last generated room. Variable lastRoom will be set to this on generating new rooms
    private GameObject currentRoom;
    // A reference of the second last generated room, to be destroyed when needed
    private GameObject lastRoom;
    // Basically, currentRoom and lastRoom together work as a buffer of rooms generated.
    // New rooms goes into currentRoom, and currentRoom goes into lastRoom when there is a newly generated room
    // LastRoom will be destroyed when there is a newly generated room

    // A reference to the current player object
    private GameObject player;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        // Setup singleton
        if (_instance == null)
        {
            _instance = this;
        }

        nextGenPos = initialGenOffset;

        // generate a first room on Awake
        GenRoom();

        // Set the player reference
        player = GameObject.Find("Player");
    }

    // Note: This function serves similar functionality to ProceedLv()
    // This function only generates a new room, it does NOT move the player
    // Use this function if the room layout includes bridging pathway bewteen rooms
    public void GenRoom()
    {
        // Update the buffer
        lastRoom = currentRoom;

        // Generate new room
        currentRoom = Instantiate(roomPrefab, nextGenPos, Quaternion.identity, dungeonRoot);
        
        // TODO: Find a better way to reset than this, this is horrible
        // Reset the player's location
        player = GameObject.Find("Player");
        CharacterController charController = player.GetComponent<CharacterController>();
        charController.enabled = false; 
        gameManager = GameObject.Find("Game Manager");
        gameManager.GetComponent<GameManager>().SetSpawnPoint(dungeonRoot.position);
        charController.enabled = true; 

        // Update offset
        nextGenPos += genOffset;
    }

    public void DestroyRoom()
    {
        // Destroy finished rooms
        if (lastRoom) { Destroy(lastRoom); }
    }


    // Note: This function serves similar functionality to GenRoom()
    // This function generates a new room, and move the player to the new room
    // Use this function if the room layout does NOT include bridging pathway bewteen rooms
    public void ProceedLv()
    {
        StartCoroutine("ProceedLvTransition");
    }

    IEnumerator ProceedLvTransition()
    {
        // Play transition UI anim to bring up the balckscreen
        TransitionUIManager._instance.StartTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetStartTransitionSpan());

        // Unpdate the buffer
        lastRoom = currentRoom;

        // Generate new room
        currentRoom = Instantiate(roomPrefab, nextGenPos, Quaternion.identity, dungeonRoot);
        // Move the player
        // Note: It is necessary to disable CharacterController before manually setting its position
        // CharacterController does NOT allow such adjustment
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = nextGenPos + new Vector3(0f, 0.2f, 2f);
        // Re-enable CharacterController
        player.GetComponent<CharacterController>().enabled = true;
        // Deactivate weapons obtained in the previous room
        player.GetComponent<PlayerCollectibles>().GetActiveCharacterWeapons().DeactivateAllWeapons();
        // Reset player health
        player.GetComponent<PlayerCharacter>().FullHealth();
        // Update offset
        nextGenPos += genOffset;

        // Remove the transition screen
        TransitionUIManager._instance.EndTransition();
        yield return new WaitForSeconds(TransitionUIManager._instance.GetEndTransitionSpan());
    }

    // Gizmos function - For visual assistance in the editor only
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
