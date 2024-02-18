using System.Collections;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private float moveHeight;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine("ElevatorOperation");
        }
    }

    IEnumerator ElevatorOperation()
    {
        PlayerMovement._instance.enabled = false;
        PlayerMovement._instance.gameObject.transform.parent = transform;

        Vector3 pos = transform.position;
        float distMoved = 0f;

        while (distMoved < moveHeight)
        {
            pos.y -= moveSpeed * Time.deltaTime;
            distMoved += moveSpeed * Time.deltaTime;

            if (distMoved > moveHeight)
            {
                pos.y += distMoved - moveHeight;
            }

            transform.position = pos;

            yield return null;
        }

        PlayerMovement._instance.gameObject.transform.parent = null;
        PlayerMovement._instance.enabled = true;

        transform.parent.GetComponent<FoundryRoom>().OnElevatorArrival();

        enabled = false;
    }
}
