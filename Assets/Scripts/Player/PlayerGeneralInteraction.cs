using UnityEngine;
using UnityEngine.InputSystem;

// This script controls player interaction with objects with BaseInteractable.cs component
public class PlayerGeneralInteraction : MonoBehaviour
{

    public AudioClip interactSound;
    private AudioSource audioSource;

    // Singleton
    public static PlayerGeneralInteraction _instance;

    // How far can the player detact interactable objects
    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask whatIsInteractable;
    [SerializeField] private GameObject interactionUI;  // UI promt when an interaction is possible

    private PlayerInputAction playerInputAction;

    private BaseInteractable targetInteractable;  // Private helper variable that records the target interactable object

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new PlayerInputAction();

        playerInputAction.Player.Interact.performed += Interact;

        audioSource = GetComponent<AudioSource>();
        //audioSource.clip = interactSound;
    }

    private void Update()
    {
        targetInteractable = null;

        // Detect all the interactable objects within range, and return the first one of them with a BaseInteractable component
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

    // This is triggeredhen the E key (or other ineraction keybind) is pressed
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (targetInteractable) {
            //audioSource.Play();
            audioSource.PlayOneShot(interactSound);
            targetInteractable.OnInteract();
        }
    }

    // Gizmos function - for visual assistance only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = (targetInteractable == null) ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.up, detectRadius);
    }
}
