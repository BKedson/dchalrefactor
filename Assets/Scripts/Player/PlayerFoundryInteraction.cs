using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFoundryInteraction : MonoBehaviour
{
    public static PlayerFoundryInteraction _instance;

    [Header("Ore Selection Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float pickUpDist;
    [SerializeField] private LayerMask whatToAim;
    [Header("Ore Behavior Settings")]
    [SerializeField] private Transform oreFloatCenter;
    [SerializeField] private float oreAttractionForce;

    private PlayerInputAction playerInputAction;

    private OreManager targetedOre;
    private OreManager currentOre;
    //private Rigidbody currentOreRGBD;  // Save reference once to avoid frequent Getcomponent
    private FoundryIntakeManager targetIntake;
    private RaycastHit hit;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new PlayerInputAction();
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
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, pickUpDist, whatToAim))
        {
            // Aiming at an ore
            if (hit.collider.tag == "Ore")
            {
                Debug.Log("Ore detected");
                if (targetIntake) targetIntake.Unselect();
                targetIntake = null;

                // If the targeted ore is not the one previously targeted
                if (targetedOre && hit.collider.gameObject != targetedOre.gameObject) targetedOre.Highlight(false);
                targetedOre = hit.collider.GetComponent<OreManager>();
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
                if (targetedOre) targetedOre.Highlight(false);
                targetedOre = null;

                if (targetIntake && targetIntake.gameObject != hit.collider.gameObject) targetIntake.Unselect();
                targetIntake = hit.collider.GetComponent<FoundryIntakeManager>();
                targetIntake.Select();
            }
            // Aiming at other interactables
            else
            {
                if (targetedOre) targetedOre.Highlight(false);
                targetedOre = null;

                if (targetIntake) targetIntake.Unselect();
                targetIntake = null;
            }
        }
        else
        {
            if (targetedOre) targetedOre.Highlight(false);
            targetedOre = null;

            if (targetIntake) targetIntake.Unselect();
            targetIntake = null;
        }

        if (currentOre) currentOre.Highlight(true);
    }

    private void FixedUpdate()
    {
        if (currentOre)
        {
            Vector3 posDiff = oreFloatCenter.position - currentOre.transform.position;

            float d = posDiff.magnitude;
            d = Mathf.Min(d * d, 9f);

            currentOre.Drag(posDiff.normalized * oreAttractionForce * d);
        }
    }

    private void PickupOrInsert(InputAction.CallbackContext ctx)
    {
        if (targetedOre)
        {
            if (currentOre) currentOre.OnDrop();

            currentOre = targetedOre.GetComponent<OreManager>();
            currentOre.OnPickUp();

            targetedOre.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(targetIntake)
        {
            // Try to insert
            if (currentOre)
            {
                if (targetIntake.Insert(currentOre.gameObject))
                {
                    currentOre.OnInsert();
                    currentOre = null;
                }
            }
            // Try to get by q ejecting inserted ore
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

    private void DropDown(InputAction.CallbackContext ctx)
    {
        if (currentOre)
        {
            currentOre.OnDrop();
            currentOre = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(camTransform.position, camTransform.forward * pickUpDist);
    }
}
