using UnityEngine;

public class BarrierController : MonoBehaviour
{
    private bool keyPress = false;

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (keyPress && other.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
