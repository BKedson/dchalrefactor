using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    [SerializeField] private MathType mathOperation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DungeonGenerator._instance.SetOperation(mathOperation);
            DungeonGenerator._instance.InitializeDungeon();

            PlayerMovement._instance.MoveToDungeon();
        }
    }
}
