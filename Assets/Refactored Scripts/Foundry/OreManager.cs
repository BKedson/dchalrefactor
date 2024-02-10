using UnityEngine;

public class OreManager : MonoBehaviour
{
    [SerializeField] private Transform billboardTransform;
    [SerializeField] private int orePower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        billboardTransform.position = transform.position + (Camera.main.transform.position - transform.position).normalized * 1f;
        billboardTransform.rotation = Quaternion.identity;
    }

    public int GetPower()
    {
        return orePower;
    }
}
