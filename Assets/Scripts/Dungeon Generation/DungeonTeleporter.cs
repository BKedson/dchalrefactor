using UnityEngine;

public class DungeonTeleporter : MonoBehaviour
{
    [SerializeField] private MathTopic mathTopic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DungeonGenerator._instance.SetTopic(mathTopic);
            DungeonGenerator._instance.InitializeDungeon();
        }
    }
}
