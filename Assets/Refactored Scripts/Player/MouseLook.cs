using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform camHolderTransform;
    [SerializeField] private LayerMask camCollisionMask;
    [SerializeField] private float camCollisionRadius;

    [SerializeField] private float camPitchMin;
    [SerializeField] private float camPitchMax;

    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    private Vector3 camDefaultLocalOffset;
    private Vector3 camFlippedLocalOffset;

    private float xRot = 0;
    private float yRot = 0;

    private bool camLimited = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camDefaultLocalOffset = camTransform.localPosition;
        camFlippedLocalOffset = camDefaultLocalOffset;
        camFlippedLocalOffset.x *= -1f;
    }

    private void Update()
    {
        xRot = Mathf.Clamp(xRot - Input.GetAxis("Mouse Y") * sensitivityY, camPitchMin, camPitchMax);
        yRot = (yRot + Input.GetAxis("Mouse X") * sensitivityX) % 360f;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        // Current camera offset relative to the camera holder
        Vector3 currOffset = camTransform.position - camHolderTransform.position;
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
        }

        transform.rotation = Quaternion.Euler(0, yRot, 0);
        camHolderTransform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    private void LateUpdate()
    {
        if (PlayerMovement._instance.OnWallR())
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, camFlippedLocalOffset, 0.1f);
        }
        else
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, camDefaultLocalOffset, 0.1f);
        }
    }

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