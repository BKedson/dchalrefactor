using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PlayerInventory._instance.GetClueNum() >= 3)
        {
            UIManager._instance.ActivateDungeonExitComfirmation(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager._instance.ActivateDungeonExitComfirmation(false);
    }
}
