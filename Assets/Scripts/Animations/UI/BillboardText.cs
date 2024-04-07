using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
