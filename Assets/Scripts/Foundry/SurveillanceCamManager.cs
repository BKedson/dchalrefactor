using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class SurveillanceCamManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    [SerializeField] private Transform camTransform;
    [SerializeField] private float maxYRotDegree;
    [SerializeField] private float rotSpeed;
    [SerializeField] private ScriptableRendererFeature[] features;
    [SerializeField] private UnityEvent OnQuit;

    private float yRot = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new PlayerInputAction();

        //playerInputAction.UI.Escape.performed += (InputAction.CallbackContext ctx) => { OnQuit.Invoke(); };

        foreach (ScriptableRendererFeature feature in features) { feature.SetActive(false); }
    }

    private void OnEnable()
    {
        //playerInputAction.UI.Enable();
        playerInputAction.Player.Movement.Enable();

        foreach (ScriptableRendererFeature feature in features) { feature.SetActive(true); }
    }

    private void OnDisable()
    {
        //playerInputAction.UI.Disable();
        //playerInputAction.Player.Movement.Disable();

        foreach (ScriptableRendererFeature feature in features) { feature.SetActive(false); }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontalInput = playerInputAction.Player.Movement.ReadValue<Vector2>().x;

        Vector3 camRot = camTransform.localRotation.eulerAngles;
        yRot = Mathf.Clamp(yRot + horizontalInput * rotSpeed * Time.deltaTime, -maxYRotDegree, maxYRotDegree);
        camRot.y = yRot;

        camTransform.localRotation = Quaternion.Euler(camRot);
    }
}
