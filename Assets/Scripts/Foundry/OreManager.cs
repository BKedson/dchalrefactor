using TMPro;
using UnityEngine;

// The ore manager. This class handles behaviors of an ore when selected/picked up/dropped/inserted into a forge
[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class OreManager : MonoBehaviour
{
    // The billboard UI canvas
    [SerializeField] private Transform billboardTransform;
    // The billboard UI text element indicating the ore power
    [SerializeField] private TMP_Text powerDisplay;
    // The power value. Note it is public get, private set (see function SetPower(int power))
    [SerializeField] public int orePower { get; private set; }

    // A reference to the MeshRenderer component
    private MeshRenderer meshRenderer;
    // A reference to the Rigidbody component
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
        // Reposition the billboard to face and offset in the direction of the camera
        billboardTransform.position = transform.position + (Camera.main.transform.position - transform.position).normalized;
    }

    // Setter function of orePower
    public void SetPower(int power)
    {
        orePower = power;
        powerDisplay.text = power.ToString();
    }

    // Behavior when selected (toHighlight = true) or deselected (toHighlight = false)
    // See PlayerFoundryInteraction.cs for example usage
    public void Highlight(bool toHighlight)
    {
        meshRenderer.enabled = toHighlight;
    }

    // Behavior when held by the player.
    // Vector3 force is the force that drags this ore with the player. Its values is calculated in PlayerFoundryInteraction.cs
    public void Drag(Vector3 force)
    {
        rgbd.AddForce(force);
    }

    // Behavior when picked up by the player.
    public void OnPickUp()
    {
        rgbd.drag = 10f;
        rgbd.useGravity = true;
    }

    // Behavior when dropped by the player.
    public void OnDrop()
    {
        rgbd.drag = 1f;
        rgbd.useGravity = true;

        Highlight(false);
    }

    // Behavior when inserted into a foundry.
    public void OnInsert()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;

        Highlight(false);
    }

    // Behavior when ejected from a foundry.
    // This happens when the player wants to retreive the oreor when the foundry rejects it on answer check
    public void OnEject()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
