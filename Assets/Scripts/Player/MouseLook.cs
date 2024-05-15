using UnityEngine;

// This script controls player camera movement and rotation according to mouse inputs
// IMPORTANT: This script is NOT placed on the object with a camera component, but on the player transform
// The third-person camera is set up as follow:
// The player object has a child named TPS Camera Holder
// This is the center of the vertical (X) rotation for the camera to look up and down
// The whole player is rotated horizontally (Y) to look left and right
// The TPS Camera Holder has a child named Main Camera
// This is the object with the actual camera component and has a local offset from the TPS Camera Holder
// The camera component is placed on Main Camera instead of TPS Camera Holder to preserve the offset regardless of rotations
public class MouseLook : MonoBehaviour
{
    // The transform of TPS Camera Holder, as mentioned above
    [SerializeField] private Transform camHolderTransform;
    // The transform of Main Camera, as mentioned above
    [SerializeField] private Transform camTransform;
    [SerializeField] private GameObject activePlayer;

    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
    [SerializeField] private LayerMask camPlayerCollisionMask;

    // The collision mask to avoid the camera clipping into walls
    [SerializeField] private LayerMask camCollisionMask;
    // Camera collision check uses Physics.SphereCast
    // This is the radius of the sphere and should be no smaller than the near clipping plane distance of the camera
    // Said distance can be found at "Projection - Clipping planes - Near" of the camera component
    // Note: It is not recommanded to change the near clipping plane distance. And 0.1f is a good choice for this value
    [SerializeField] private float camCollisionRadius;  // Recommanded Range [0.1-0.05]

    [SerializeField] private float playerCollisionRadius;

    // Max and min pitch (X rotation)
    [SerializeField] private float camPitchMin;
    [SerializeField] private float camPitchMax;

    // Sensitivities
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    // The local offset of Main Camera from the TPS Camera Holder, as mentioned above
    private Vector3 camDefaultLocalOffset;
    // The same offset, but when the camera is temperarily needed to be over the other shoulder of the player
    private Vector3 camFlippedLocalOffset;

    // Private helper variables to record the rotation
    private float xRot = 0;
    private float yRot = 0;

    // Whether the camera is moved closer to the player to avoid clipping into walls
    private bool camLimited = false;

    private bool camLimitedPlayer = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camDefaultLocalOffset = camTransform.localPosition;
        camFlippedLocalOffset = camDefaultLocalOffset;
        camFlippedLocalOffset.x *= -1f;
        activePlayer = GetComponent<PlayerCharacter>().GetActiveCharacter();
        playerMeshRenderer = activePlayer.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        xRot = Mathf.Clamp(xRot - Input.GetAxis("Mouse Y") * sensitivityY, camPitchMin, camPitchMax);
        yRot = (yRot + Input.GetAxis("Mouse X") * sensitivityX) % 360f;
    }

    private void LateUpdate()
    {
        // The camera may be moved to the other side of the player while wall running (which is currently not in use)
        if (PlayerMovement._instance.OnWallR())
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, camFlippedLocalOffset, 0.1f);
        }
        else
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, camDefaultLocalOffset, 0.1f);
        }

        RaycastHit hit;
        // Current camera offset relative to the camera holder (maybe closer than camDefaultLocalOffset when limited)
        Vector3 currOffset = camTransform.position - camHolderTransform.position;
        // Sphere cast to check if the camera would clip into walls and determine a new position for it
        camLimited = Physics.SphereCast(
            camHolderTransform.position,
            camCollisionRadius,
            currOffset.normalized,
            out hit,
            camDefaultLocalOffset.magnitude,
            camCollisionMask
        );
        if (camLimited)
        {
            camTransform.position =
                camHolderTransform.position +
                currOffset.normalized *
                Mathf.Max(hit.distance - camCollisionRadius, 0.1f);
            playerMeshRenderer.enabled = false;
        }
        else {playerMeshRenderer.enabled = true;}


        // Rotate the player and the camera holder
        transform.rotation = Quaternion.Euler(0, yRot, 0);
        camHolderTransform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    // Gizmos function - for visual assistance only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = camLimited ? Color.red : Color.green;
        Gizmos.DrawWireSphere(camTransform.position, camCollisionRadius);
    }

    public void SetSensitivities(float x, float y)
    {
        sensitivityX = x;
        sensitivityY = y;
    }
}