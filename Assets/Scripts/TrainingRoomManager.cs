using UnityEngine;

public class TrainingRoomManager : MonoBehaviour
{
    public static TrainingRoomManager _instance;

    [SerializeField] private GameObject TrainingRoomDoor;
    [SerializeField] private GameObject[] trainingBots;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TrainingRoomDoor.SetActive(true);

            foreach (GameObject go in trainingBots)
            {
                go.GetComponent<EnemyControl>().enabled = true;
            }
        }
    }

    public void UnlockTrainingRoom()
    {
        TrainingRoomDoor.SetActive(false);
    }
}
