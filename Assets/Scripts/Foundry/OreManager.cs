using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class OreManager : MonoBehaviour
{
    [SerializeField] private Transform billboardTransform;
    [SerializeField] private TMP_Text powerDisplay;
    [SerializeField] public int orePower { get; private set; }

    private MeshRenderer meshRenderer;
    private Rigidbody rgbd;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rgbd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        billboardTransform.position = transform.position + (Camera.main.transform.position - transform.position).normalized;
    }

    public void SetPower(int power)
    {
        orePower = power;
        powerDisplay.text = power.ToString();
    }

    public void Highlight(bool toHighlight)
    {
        meshRenderer.enabled = toHighlight;
    }

    public void Drag(Vector3 force)
    {
        rgbd.AddForce(force);
    }

    public void OnPickUp()
    {
        rgbd.drag = 10f;
        rgbd.useGravity = true;
    }

    public void OnDrop()
    {
        rgbd.drag = 1f;
        rgbd.useGravity = true;

        Highlight(false);
    }

    public void OnInsert()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;

        Highlight(false);
    }

    public void OnEject()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
