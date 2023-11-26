using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private string question;
    private string answer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Vector3.Dot(other.transform.position - transform.position, transform.forward) < 0)
            {
                //DungeonGenerator._instance.PrepareToGenRoom(transform, door);
                UIController._instance.SetUpQuestion(question, answer, QuestionType.DoorLock);
            }
            else
            {
                door.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIController._instance.CancelAttempt();
    }

    public void SetPuzzle(string p, string a)
    {
        question = p;
        answer = a;
    }
}
