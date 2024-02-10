using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFoundryInteraction : MonoBehaviour
{
    public static PlayerFoundryInteraction _instance;

    [Header("Ore Selection Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float pickUpDist;
    [SerializeField] private LayerMask whatIsOre;
    [Header("Ore Behavior Settings")]
    [SerializeField] private Transform oreFloatCenter;
    [SerializeField] private float oreAttractionForce;
    [SerializeField] private float oreFloatDrag;

    private PlayerInputAction playerInputAction;

    private GameObject targetedOre;
    //private GameObject currentOre;
    private Rigidbody currentOreRGBD;  // Save reference once to avoid frequent Getcomponent
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
        //Physics.OverlapBox(camTransform.position + camTransform.forward * pickUpDist / 2f, )
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, pickUpDist, whatIsOre))
        {
            if (hit.collider.tag == "Ore")
            {
                if (targetIntake)
                {
                    targetIntake.Unselect();
                }
                targetIntake = null;

                // If the targeted ore is not the one previously targeted
                if (targetedOre && hit.collider.gameObject != targetedOre)
                {
                    targetedOre.GetComponent<MeshRenderer>().enabled = false;
                }
                targetedOre = hit.collider.gameObject;

                // If the targeted ore is not the currently held one
                if (!currentOreRGBD || targetedOre != currentOreRGBD.gameObject)
                {
                    targetedOre.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else if (hit.collider.tag == "Foundry Intake")
            {
                if (targetedOre)
                {
                    targetedOre.GetComponent<MeshRenderer>().enabled = false;
                }
                targetedOre = null;

                targetIntake = hit.collider.GetComponent<FoundryIntakeManager>();
                targetIntake.Select();
            }
            else
            {
                if (targetedOre)
                {
                    targetedOre.GetComponent<MeshRenderer>().enabled = false;
                }
                targetedOre = null;

                if (targetIntake)
                {
                    targetIntake.Unselect();
                }
                targetIntake = null;
            }
        }
        else
        {
            if (targetedOre)
            {
                targetedOre.GetComponent<MeshRenderer>().enabled = false;
            }
            targetedOre = null;

            if (targetIntake)
            {
                targetIntake.Unselect();
            }
            targetIntake = null;
        }
    }

    private void FixedUpdate()
    {
        if (currentOreRGBD)
        {
            Vector3 posDiff = oreFloatCenter.position - currentOreRGBD.position;

            float d = posDiff.magnitude;
            d = Mathf.Min(d * d, 9f);

            currentOreRGBD.AddForce(posDiff.normalized * oreAttractionForce * d);
        }
    }

    private void PickupOrInsert(InputAction.CallbackContext ctx)
    {
        if (targetedOre)
        {
            Debug.Log(targetedOre.gameObject);
            if (currentOreRGBD)
            {
                currentOreRGBD.drag = 1;
                currentOreRGBD.useGravity = true;
                currentOreRGBD = null;
            }
            currentOreRGBD = targetedOre.GetComponent<Rigidbody>();
            currentOreRGBD.drag = oreFloatDrag;
            currentOreRGBD.useGravity = false;

            targetedOre.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(targetIntake)
        {
            if (currentOreRGBD)
            {
                currentOreRGBD.GetComponent<Collider>().enabled = false;
                if (targetIntake.Insert(currentOreRGBD.gameObject))
                {
                    currentOreRGBD = null;
                }
            }
            else
            {
                GameObject obj = targetIntake.Eject();
                if (obj)
                {
                    currentOreRGBD = obj.GetComponent<Rigidbody>();
                    currentOreRGBD.GetComponent<Collider>().enabled = true;
                    currentOreRGBD.drag = oreFloatDrag;
                    currentOreRGBD.useGravity = false;
                }
            }
        }
    }

    private void DropDown(InputAction.CallbackContext ctx)
    {
        if (currentOreRGBD)
        {
            currentOreRGBD.drag = 1;
            currentOreRGBD.useGravity = true;
            currentOreRGBD = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(camTransform.position, camTransform.forward * pickUpDist);
    }
}
