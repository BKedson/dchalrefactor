using UnityEngine;
using UnityEngine.InputSystem;

public class SurveillanceCamManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    [SerializeField] private Transform camTransform;
    [SerializeField] private float maxYRotDegree;
    [SerializeField] private float rotSpeed;

    private float yRot = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new PlayerInputAction();

        playerInputAction.UI.Escape.performed += OnExit;
    }

    private void OnEnable()
    {
        playerInputAction.UI.Enable();
        playerInputAction.Player.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.UI.Disable();
        playerInputAction.Player.Movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = playerInputAction.Player.Movement.ReadValue<Vector2>().x;

        Vector3 camRot = camTransform.localRotation.eulerAngles;
        yRot = Mathf.Clamp(yRot + horizontalInput * rotSpeed * Time.deltaTime, -maxYRotDegree, maxYRotDegree);
        camRot.y = yRot;

        camTransform.localRotation = Quaternion.Euler(camRot);
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        transform.parent.GetComponent<FoundryRoom>().OnWindowChallengeFinish();
    }
}
