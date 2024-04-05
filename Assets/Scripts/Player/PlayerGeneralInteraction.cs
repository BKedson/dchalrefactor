using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGeneralInteraction : MonoBehaviour
{
    public static PlayerGeneralInteraction _instance;

    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask whatIsInteractable;
    [SerializeField] private GameObject interactionUI;

    private PlayerInputAction playerInputAction;

    private BaseInteractable targetInteractable;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new PlayerInputAction();

        playerInputAction.Player.Interact.performed += Interact;
    }

    private void Update()
    {
        targetInteractable = null;

        Collider[] cols = Physics.OverlapSphere(transform.position + transform.up, detectRadius, whatIsInteractable);
        foreach (Collider col in cols)
        {
            col.TryGetComponent<BaseInteractable>(out targetInteractable);
            if (targetInteractable) break;
        }

        interactionUI.SetActive(targetInteractable != null);
    }

    private void OnEnable()
    {
        playerInputAction.Player.Interact.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Interact.Disable();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (targetInteractable) targetInteractable.OnInteract();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = (targetInteractable == null) ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.up, detectRadius);
    }
}
