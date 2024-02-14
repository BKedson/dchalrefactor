using UnityEngine;
using UnityEngine.InputSystem;

enum WallState
{
    NotOnWall = 0, OnWallL, OnWallR
}

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement _instance;

    [Header("Movement Speed")]
    [SerializeField][Min(0)] private float walkSpeed;
    [SerializeField][Min(0)] private float runSpeed;
    [SerializeField] private float gravity = 9.8f;

    [SerializeField] private LayerMask whatIsEnvironment;
    [SerializeField][Min(0)] private float groundCheckRadius;

    [Header("Jump Settings")]
    [SerializeField][Min(0)] private float jumpInterval;
    [SerializeField][Min(0)] private float jumpForce;
    [SerializeField] private float fallingBonus;

    [Header("Wall Run Settings")]
    [SerializeField] private Vector3 wallCheckCenterOffset;
    [SerializeField] private float wallCheckHorizontalOffset;
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private float wallRunFallSpeed;

    private PlayerInputAction playerInputAction;
    private WeaponManager weaponManager;

    private CharacterController characterController;
    private bool grounded = false;

    private bool sprinting = false;

    private float lastJumpTime = 0f;

    private WallState wallState = WallState.NotOnWall;
    //private bool wallRunDetached = false;  // Disable wall run after detach and before touching the ground
    private Vector3 wallNormal;

    private Vector2 horizontalVelocity;
    private float yVelocity = 0f;
    private PlayerInventory stuff;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Sprint.started += StartSprint => { sprinting = true; };
        playerInputAction.Player.Sprint.performed += StopSprint => { sprinting = false; };
        playerInputAction.Player.Jump.started += Jump;
        playerInputAction.Player.Swap1.started += sSwap;
        playerInputAction.Player.Swap2.started += gSwap;
        playerInputAction.Player.Swap3.started += cSwap;
        playerInputAction.Player.Attack.started += Attack;

        characterController = GetComponent<CharacterController>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        stuff = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Movement.Enable();
        playerInputAction.Player.Sprint.Enable();
        playerInputAction.Player.Jump.Enable();
        playerInputAction.Player.Swap1.Enable();
        playerInputAction.Player.Swap2.Enable();
        playerInputAction.Player.Swap3.Enable();
        playerInputAction.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Movement.Disable();
        playerInputAction.Player.Sprint.Disable();
        playerInputAction.Player.Jump.Disable();
        playerInputAction.Player.Swap1.Disable();
        playerInputAction.Player.Swap2.Disable();
        playerInputAction.Player.Swap3.Disable();
        playerInputAction.Player.Attack.Disable();
    }

    private void Update()
    {
        //Debug.Log(sprinting);
    }

    void FixedUpdate()
    {
        //Debug.Log(wallState);
        //transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);

        // Behavior while NOT wall running
        if (wallState == WallState.NotOnWall)
        {
            // Ensure on ground
            if (grounded)
            {
                if (characterController.velocity.y <= 0)  // Not jumping
                {
                    yVelocity = -0.1f;
                }
            }
            else
            {
                yVelocity -= gravity * Time.fixedDeltaTime;
                if (characterController.velocity.y < 0 || playerInputAction.Player.Jump.phase == InputActionPhase.Waiting)
                {
                    yVelocity -= fallingBonus;
                }
            }
        }
        // Behavior wall running
        else
        {
            yVelocity = -wallRunFallSpeed;
        }

        horizontalVelocity = playerInputAction.Player.Movement.ReadValue<Vector2>();

        Vector3 movement;
        if (wallState == WallState.NotOnWall)
        {
            movement =
                (transform.right * horizontalVelocity.x + transform.forward * horizontalVelocity.y).normalized * 
                (sprinting ? runSpeed : walkSpeed);
        }
        else
        {
            movement = Vector3.ProjectOnPlane(transform.forward * horizontalVelocity.y, wallNormal);
            movement = (movement + transform.right * horizontalVelocity.x).normalized * (sprinting ? runSpeed : walkSpeed);
        }
        movement.y = yVelocity;
        characterController.Move(movement * Time.fixedDeltaTime);

        grounded = Physics.CheckSphere(transform.position, groundCheckRadius, whatIsEnvironment);

        if (grounded)
        {
            wallState = WallState.NotOnWall;
        }
        else if (characterController.velocity.y <= 0.05f)
        {
            if (Physics.CheckSphere(
                transform.position + wallCheckCenterOffset - transform.right * wallCheckHorizontalOffset,
                wallCheckRadius,
                whatIsEnvironment
            ))
            {
                RaycastHit hit;
                Physics.Raycast(
                    transform.position + wallCheckCenterOffset,
                   -transform.right,
                    out hit,
                    wallCheckHorizontalOffset + wallCheckRadius,
                    whatIsEnvironment
                );
                wallNormal = hit.normal;
                wallState = WallState.OnWallL;
            }
            else
            if (Physics.CheckSphere(
                transform.position + wallCheckCenterOffset + transform.right * wallCheckHorizontalOffset,
                wallCheckRadius,
                whatIsEnvironment
            ))
            {
                RaycastHit hit;
                Physics.Raycast(
                    transform.position + wallCheckCenterOffset,
                    transform.right,
                    out hit,
                    wallCheckHorizontalOffset + wallCheckRadius,
                    whatIsEnvironment
                );
                wallNormal = hit.normal;
                wallState = WallState.OnWallR;
            }
            else
            {
                wallState = WallState.NotOnWall;
            }
        }
        else  // Jumping (from ground)
        {
            wallState = WallState.NotOnWall;
        }
        if(playerInputAction.Player.Attack.phase == InputActionPhase.Performed){ weaponManager.currentAttack(false);}
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (grounded && Time.time - lastJumpTime > jumpInterval)
        {
            yVelocity = jumpForce;
            lastJumpTime = Time.time;
            grounded = false;
        }
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        weaponManager.currentAttack(true);
    }

    private void gSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(2);
    }

    private void sSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(1);
    }
    private void cSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

        Gizmos.color = wallState == WallState.OnWallL ? Color.green : Color.red;
        Gizmos.DrawWireSphere(
            transform.position + wallCheckCenterOffset - transform.right * wallCheckHorizontalOffset, wallCheckRadius
        );
        Gizmos.color = wallState == WallState.OnWallR ? Color.green : Color.red;
        Gizmos.DrawWireSphere(
            transform.position + wallCheckCenterOffset + transform.right * wallCheckHorizontalOffset, wallCheckRadius
        );
    }

    public void EnableInput(bool toEnable)
    {
        if (toEnable)
        {
            playerInputAction.Player.Movement.Enable();
            playerInputAction.Player.Sprint.Enable();
            playerInputAction.Player.Jump.Enable();
            playerInputAction.Player.Swap1.Enable();
            playerInputAction.Player.Swap2.Enable();
            playerInputAction.Player.Swap3.Enable();
            playerInputAction.Player.Attack.Enable();
        }
        else
        {
            playerInputAction.Player.Movement.Disable();
            playerInputAction.Player.Sprint.Disable();
            playerInputAction.Player.Jump.Disable();
            playerInputAction.Player.Swap1.Disable();
            playerInputAction.Player.Swap2.Disable();
            playerInputAction.Player.Swap3.Disable();
            playerInputAction.Player.Attack.Disable();
        }
    }

    public void MoveToDungeon()
    {
        characterController.enabled = false;
        transform.position = Vector3.zero;
        characterController.enabled = true;
    }

    public void LeaveDungeon()
    {
        characterController.enabled = false;
        transform.position = new Vector3(0f, 50f, 0f);
        characterController.enabled = true;
    }

    public bool OnWallR()
    {
        return wallState == WallState.OnWallR;
    }
}
