using UnityEngine;
using UnityEngine.InputSystem;

// This script controls player behavior when interacting with foundry items (foundry intakes and ores)
public class PlayerFoundryInteraction : MonoBehaviour
{

    public AudioClip interactSound;
    private AudioSource audioSource;

    // Singleton
    public static PlayerFoundryInteraction _instance;

    [Header("Ore Selection Settings")]
    [SerializeField] private Transform camTransform;  // A reference to the camera to get look direction
    [SerializeField] private float pickUpDist;  // Max distance to pick up an ore
    [SerializeField] private LayerMask whatToAim;  // Mask to mark interactable objects (foundry intakes and ores)
    [Header("Ore Behavior Settings")]
    [SerializeField] private Transform oreFloatCenter;  // The center ores float around when picked up
    [SerializeField] private float oreAttractionForce;  // How much force is the ore dragged to oreFloatCenter with

    private PlayerInputAction playerInputAction;

    private OreManager targetedOre;  // The ore being aimed at (if any)
    private OreManager currentOre;  // The ore being held (if any)

    //private Rigidbody currentOreRGBD;  // Save reference once to avoid frequent Getcomponent

    private FoundryIntakeManager targetIntake;  // The intake being aimed at (if any)

    private RaycastHit hit;  // Raycast result

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new PlayerInputAction();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Fire.performed += PickupOrInsert;
        playerInputAction.Player.AltFire.performed += DropDown;

        playerInputAction.Player.Fire.Enable();
        playerInputAction.Player.AltFire.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Fire.performed -= PickupOrInsert;
        playerInputAction.Player.AltFire.performed -= DropDown;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for intakes or ores aimed at by the player
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, pickUpDist, whatToAim))
        {
            // Aiming at an ore
            if (hit.collider.tag == "Ore")
            {
                //Debug.Log("Ore detected");
                // If aiming at and highlighting an intake, cancel highlight and selection
                if (targetIntake) targetIntake.Unselect();
                targetIntake = null;

                // If the targeted ore is not the one previously targeted, then cancel highlight
                if (targetedOre && hit.collider.gameObject != targetedOre.gameObject) targetedOre.Highlight(false);
                // Highlight current ore
                targetedOre = hit.collider.GetComponentInParent<OreManager>();
                // targetedOre = hit.collider.GetComponent<OreManager>();
                targetedOre.Highlight(true);

                //// If the targeted ore is not the currently held one
                //if (!currentOre || targetedOre != currentOre)
                //{
                //    targetedOre.GetComponent<MeshRenderer>().enabled = true;
                //}
            }

            // Aiming at an intake
            else if (hit.collider.tag == "Foundry Intake")
            {
                // If aiming at and highlighting an ore, cancel highlight and selection
                if (targetedOre) targetedOre.Highlight(false);
                targetedOre = null;

                // If the targeted intake is not the one previously targeted, then cancel highlight
                if (targetIntake && targetIntake.gameObject != hit.collider.gameObject) targetIntake.Unselect();
                // Highlight current intake
                targetIntake = hit.collider.GetComponent<FoundryIntakeManager>();
                targetIntake.Select();
            }

            // Aiming at other interactables then cancel highlight on any previously highlighted ore or intake
            else
            {
                if (targetedOre) targetedOre.Highlight(false);
                targetedOre = null;

                if (targetIntake) targetIntake.Unselect();
                targetIntake = null;
            }
        }
        // If not aiming at anything then cancel highlight on any previously highlighted ore or intake
        else
        {
            if (targetedOre) targetedOre.Highlight(false);
            targetedOre = null;

            if (targetIntake) targetIntake.Unselect();
            targetIntake = null;
        }

        // If the player is holding an ore, then it is always highlighted
        if (currentOre) currentOre.Highlight(true);
    }

    private void FixedUpdate()
    {
        // Attract the currently held ore to oreFloatCenter
        if (currentOre)
        {
            Vector3 posDiff = oreFloatCenter.position - currentOre.transform.position;

            // Variable d is the distance factor of the attraction force, and is the square of the distance
            // This factor is limited above 9, to void jittering
            // Change this 9 if global gravity factor is changed
            float d = posDiff.magnitude;
            d = Mathf.Min(d * d, 9f);

            currentOre.Drag(posDiff.normalized * oreAttractionForce * d);
        }
    }

    // Behavior when mouse button left (or other attack keybind) is pressed
    private void PickupOrInsert(InputAction.CallbackContext ctx)
    {

        // If targeting at an ore, then pick it up
        if (targetedOre)
        {
            audioSource.PlayOneShot(interactSound);

            // Drop the currently held ore to pick up the new one
            if (currentOre) currentOre.OnDrop();

            currentOre = targetedOre.GetComponent<OreManager>();
            currentOre.OnPickUp();

            targetedOre = null;
        }
        // If targeting at an intake, then insert the currently held ore, if any, or retreive the inserted ore, if any
        else if (targetIntake)
        {
            audioSource.PlayOneShot(interactSound);
            
            // If holding an ore, the try to insert
            if (currentOre)
            {
                
                GameObject obj = targetIntake.Eject();

                if (targetIntake.Insert(currentOre.gameObject))
                {
                    currentOre.OnInsert();
                    currentOre = null;
                }
                // If there is an ore already in the intake, swap ores
                if (obj)
                {
                    currentOre = obj.GetComponent<OreManager>();
                    currentOre.OnEject();
                    currentOre.OnPickUp();
                }

                
            }
            // If not holding an ore, then try to get one by ejecting inserted ore
            else
            {
                GameObject obj = targetIntake.Eject();
                if (obj)
                {
                    currentOre = obj.GetComponent<OreManager>();
                    currentOre.OnEject();
                    currentOre.OnPickUp();
                }
            }
        }
    }

    // Drop the current held ore when mouse button right (or other mouse secondary action keybind) is pressed
    private void DropDown(InputAction.CallbackContext ctx)
    {
        if (currentOre)
        {
            audioSource.PlayOneShot(interactSound);
            currentOre.OnDrop();
            currentOre = null;
        }
    }

    // Gizmos function - for visual assistance only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(camTransform.position, camTransform.forward * pickUpDist);
    }
}
