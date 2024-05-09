using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

// This class controls the surveillance camera used in a window question
// IMPORTANT: This script is added to the PARENT of the object with a camera component
// This design is so that the direction of the camera is relative to the Z direction of this object
public class SurveillanceCamManager : MonoBehaviour
{
    // This script uses the same control scheme as the player
    private PlayerInputAction playerInputAction;

    // The refence to its children object that has a camera component
    [SerializeField] private Transform camTransform;
    // Max rotation allowed on the Y axis
    [SerializeField] private float maxYRotDegree;
    // Rotation speed
    [SerializeField] private float rotSpeed;
    // Post processing visual effects to enable/disable when switching to the surveillance camera view
    [SerializeField] private ScriptableRendererFeature[] features;
    // Unity event to trigger when quiting the window question
    [SerializeField] private UnityEvent OnQuit;

    private float yRot = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new PlayerInputAction();

        //playerInputAction.UI.Escape.performed += (InputAction.CallbackContext ctx) => { OnQuit.Invoke(); };

        // Disable PP effects on awake since the game always begins in the player camera view
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

    // Update the camera dierection based on input
    void LateUpdate()
    {
        float horizontalInput = playerInputAction.Player.Movement.ReadValue<Vector2>().x;

        Vector3 camRot = camTransform.localRotation.eulerAngles;
        yRot = Mathf.Clamp(yRot + horizontalInput * rotSpeed * Time.deltaTime, -maxYRotDegree, maxYRotDegree);
        camRot.y = yRot;

        camTransform.localRotation = Quaternion.Euler(camRot);
    }
}
