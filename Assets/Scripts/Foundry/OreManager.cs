using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class OreManager : MonoBehaviour
{
    [SerializeField] private Transform billboardTransform;
    [SerializeField] public int orePower { get; private set; }

    private MeshRenderer meshRenderer;
    private Rigidbody rgbd;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rgbd = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        billboardTransform.position = transform.position + (Camera.main.transform.position - transform.position).normalized;
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
        collider.enabled = false;

        Highlight(false);
    }

    public void OnEject()
    {
        collider.enabled = true;
    }
}
