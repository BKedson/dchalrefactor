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
    private float camDefaultLocalOffsetMag;

    private float xRot = 0;
    private float yRot = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camDefaultLocalOffset = camTransform.localPosition;
        camDefaultLocalOffsetMag = camTransform.localPosition.magnitude;
    }

    private void Update()
    {
        xRot = Mathf.Clamp(xRot - Input.GetAxis("Mouse Y") * sensitivityY, camPitchMin, camPitchMax);
        yRot = (yRot + Input.GetAxis("Mouse X") * sensitivityX) % 360f;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 dir = (camTransform.position - camHolderTransform.position).normalized;
        Debug.Log(dir);
        if (Physics.SphereCast(
            camHolderTransform.position,
            camCollisionRadius,
            dir,
            out hit,
            camDefaultLocalOffsetMag,
            camCollisionMask
        ))
        {
            Debug.Log(hit.point - camHolderTransform.position);
            camTransform.position =
                camHolderTransform.position +
                dir *
                Mathf.Max((hit.point - camHolderTransform.position).magnitude - camCollisionRadius, 0.1f);
        }
        else
        {
            camTransform.localPosition = camDefaultLocalOffset;
        }

        transform.rotation = Quaternion.Euler(0, yRot, 0);
        camHolderTransform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    public void SetSensitivities(float x, float y)
    {
        sensitivityX = x;
        sensitivityY = y;
    }
}