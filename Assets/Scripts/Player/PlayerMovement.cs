using UnityEngine;
using UnityEngine.InputSystem;
using dchalrefactor.Scripts.Animations.PlayerMovement;
using UnityEditor.Callbacks;

// This is the player movement script featuring walking, sprinting, jumping, and wall running
// Note: Wallrunning is a legacy functionality. Comment out relevant code to avoid unitended behaviors if any occurs

// Wall run states
//enum WallState
//{// NotOnWall = 0, OnWallL, OnWallR
//}

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Singleton
    public static PlayerMovement _instance;
    //stores the Animations class for the player
    private PlayerAnimations animations;
    //stores all the animations classes for all playerCharacters
    public PlayerAnimations[] allCharacterAnimations;

    [Header("Movement Speed")]
    [SerializeField][Min(0)] private float walkSpeed;
    [SerializeField][Min(0)] private float runSpeed;
    // Player movement vector is based on a physics-based calculation using custom gravity strength
    // Thus the global gravity factor has no effect on the player. Adjust the value below instead if necessary
    [SerializeField] private float gravity = 9.8f;

    // Collider mask of walls and floors the player can collide with
    [SerializeField] private LayerMask whatIsEnvironment;
    // The radius of ground-check
    // Note that the player transform is at the feet of the player
    // Thus, this value is only for avoiding jitteringand should be small
    [SerializeField][Min(0)] private float groundCheckRadius;

    [Header("Jump Settings")]
    // The interval (cooldown) of jumping
    // This value is used to avoid unintended double (or multiple) jumps due to inconsistent Unity frame length
    // This value should be small to allow rapid jumps, but should not be shorter than 0.02 (fixed delta-time)
    [SerializeField][Min(0)] private float jumpInterval;
    // The force to propel the player upward
    [SerializeField][Min(0)] private float jumpForce;

    // Determine the force vector for the wall jump
   // [SerializeField][Min(0)] private float wallJumpVertForce;
   // [SerializeField][Min(0)] private float wallJumpHorizForce;

    // This force is applied:
    // 1. When the player is falling, for more natural jump physics
    // 2. When the space bar (or other jump keybind) is not pressed, to allow variable jump height
    [SerializeField] private float fallingBonus;

    [Header("Wall Run Settings")]
    // Wall checks are performed two times, one on each side of the player
    // This offset is the center of the two checks positions
   // [SerializeField] private Vector3 wallCheckCenterOffset;
    // How far to the left & right are the checks from the wallCheckCenterOffset
   // [SerializeField] private float wallCheckHorizontalOffset;
   // [SerializeField] private float wallCheckRadius;
    // The falling speed of the player on a wall
    // While wall-running, falling is controlled by this speed instead of gravity for more consistent behavior
   // [SerializeField] private float wallRunFallSpeed;
   // private bool exitingWall;
   // public float exitWallTime;
   // private float exitWallTimer;

    private PlayerInputAction playerInputAction;

    // I strongly recommand moving these combat-relate codes to a separate script
    private WeaponManager weaponManager;
    private int _health;

    private CharacterController characterController;

    private bool grounded = false;  // Stateus variable - Whether the player is grounded

    private bool sprinting = false;  // Stateus variable - Whether the player is sprinting

    private float lastJumpTime = 0f;  // Timer variable - The last time when the player jumps

    //private WallState wallState = WallState.NotOnWall;  // Stateus variable - Whether the player is on a wall/wall running
    //private bool wallRunDetached = false;  // Disable wall run after detach and before touching the ground
    // The normal vector of the wall the player is running on
    // Used for calculating movement assitance (attachment force)
   // private Vector3 wallNormal;

    // the velocity components of the player
    private Vector2 horizontalVelocity;
    private float yVelocity = 0f;
    // the velocity vector (combination of the two above) of the player
    private Vector3 movement;
    //private PlayerInventory stuff;
    private bool movementDisabled = false;  // Movement disabling flag
    private Rigidbody rb;

    void Awake()
    {
        // Singleton
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
        playerInputAction.Player.Fire.started += Attack;

        characterController = GetComponent<CharacterController>();
        // Combat related code. Consider moving it to a separate script
        weaponManager = GetComponentInChildren<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        //stuff = GetComponent<PlayerInventory>();
    }

    void Start()
    {
        //assign the current animations class-------------------------------------------------
        animations = allCharacterAnimations[GameManager.manager.GetCurrentCharacter()];
        //------------------------------------------------------------------------------------
    }
    private void OnEnable()
    {
        playerInputAction.Player.Movement.Enable();
        playerInputAction.Player.Sprint.Enable();
        playerInputAction.Player.Jump.Enable();
        playerInputAction.Player.Swap1.Enable();
        playerInputAction.Player.Swap2.Enable();
        playerInputAction.Player.Swap3.Enable();
        playerInputAction.Player.Fire.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Movement.Disable();
        playerInputAction.Player.Sprint.Disable();
        playerInputAction.Player.Jump.Disable();
        playerInputAction.Player.Swap1.Disable();
        playerInputAction.Player.Swap2.Disable();
        playerInputAction.Player.Swap3.Disable();
        playerInputAction.Player.Fire.Disable();
    }

    private void Update()
    {
        //Debug.Log(sprinting);
    }

    void FixedUpdate()
    {
        //Debug.Log(wallState);
        //transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);

        if (!movementDisabled)
        {
            // Y velocity while NOT wall running
           // if (wallState == WallState.NotOnWall)
           // {
                if (grounded)
                {
                    // If the player is grounded and not currently jumping
                    // Exert an extra downward velocity to avoid bumping/jittering
                    if (characterController.velocity.y <= 0)
                    {
                        yVelocity = -0.1f;
                    }
                }
                else
                {
                    // If the player is in the air
                    // Exert gravity
                    yVelocity -= gravity * Time.fixedDeltaTime;
                    // Apply falling bonus
                    if (characterController.velocity.y < 0 /*|| playerInputAction.Player.Jump.phase == InputActionPhase.Waiting*/)
                    {
                        yVelocity -= fallingBonus;
                    }
                }
           // }
            // Y velocity while wall running
            //else
           // {
             //   yVelocity = -wallRunFallSpeed;
           // }

            // Horizontal movement input (WASD or other movement keybind)
            horizontalVelocity = playerInputAction.Player.Movement.ReadValue<Vector2>();


            // Calculate movement vector while NOT wall running
           // if (wallState == WallState.NotOnWall)
          //  {
                // Running/walking speed
                movement =
                    (transform.right * horizontalVelocity.x + transform.forward * horizontalVelocity.y).normalized *
                    (sprinting ? runSpeed : walkSpeed);
                // Sprinting: if the player is walking/running slowly or running/running fast
           // }
          //  else if(exitingWall){
          //      wallState = WallState.NotOnWall;
           //     if (exitWallTimer > 0){
           //         exitWallTimer -= Time.deltaTime;
           //     }
           //     if (exitWallTimer <= 0){
          //          exitingWall = false;
          //      }
          //  }
            // Calculate movement vector while wall running
           // else
          //  {
                // Align movement direction with wall bitangent
                // This movement assistance is used so that view direction does not have to align with moving direction
            //    movement = Vector3.ProjectOnPlane(transform.forward * horizontalVelocity.y, wallNormal);
          //      movement = (movement + transform.right * horizontalVelocity.x).normalized * (sprinting ? runSpeed : walkSpeed);
          //  }

            movement.y = yVelocity;

            characterController.Move(movement * Time.fixedDeltaTime);

            // Ground check
            grounded = Physics.CheckSphere(transform.position, groundCheckRadius, whatIsEnvironment);

            //if (grounded)
           // {
           //     wallState = WallState.NotOnWall;
           // }
            // Only check for walls when the player is falling
            // Otherwise wallrun interrupts jumping
            /*
            else if (characterController.velocity.y <= 0.05f)
            {
                // Check for wall - Left
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
                // Check for wall - Right
                else if (Physics.CheckSphere(
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
                // No wall in reach on both sides
                else
                {
                    wallState = WallState.NotOnWall;
                }
            }*/
           // else  // Jumping (from wall, detach the player from wall)
           // {
            //    wallState = WallState.NotOnWall;
            //}
            // Combat related code. Consider moving it to a separate script
            if (playerInputAction.Player.Fire.phase == InputActionPhase.Performed) { weaponManager.currentAttack(false); }
        }
        //else  // Jumping (from ground)
        //{
           // wallState = WallState.NotOnWall;
        //}
        // Combat related code. Consider moving it to a separate script
        if (playerInputAction.Player.Fire.phase == InputActionPhase.Performed) { weaponManager.currentAttack(false); }
    }

    // Animation
    private void LateUpdate()
    {
        Vector3 horizontalMovement = new Vector3(movement.x, 0f, movement.z);
        float horizontalMagnitude = horizontalMovement.magnitude;

        // Run Anim
        if (horizontalMagnitude > 3)
        {
            if (sprinting)
            {
                //Stop Running slow (animation) if the player was running slow
                animations.StopRunSlow();
                // Running fast (animation)
                animations.RunFast();

            }
            else
            {
                //Stop Running fast (animation) if the player was running Fast
                animations.StopRunFast();
                // Running slowly (animation)
                animations.RunSlow();
            }
        }
        else
        {
            // Idel anim
            animations.StopRunSlow();
            animations.StopRunFast();
        }
    }

    // Space key (or other jump keybind) callback
    private void Jump(InputAction.CallbackContext ctx)
    {
        // Allow jump only if the player is grounded and jump CD is ready
        if (grounded && Time.time - lastJumpTime > jumpInterval)
        {
            yVelocity = jumpForce;
            lastJumpTime = Time.time;
            grounded = false;

            // Jump animation
            animations.Jump();
        }
        //Allow walljump
        /*
        else if(wallState == WallState.OnWallL || wallState == WallState.OnWallR){

            Debug.Log("walljump");

            exitingWall = true;
            exitWallTimer = exitWallTime;

            Vector3 forceToApply = transform.up * wallJumpVertForce + wallNormal * wallJumpHorizForce;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.y);
            rb.AddForce(forceToApply, ForceMode.Impulse);

            lastJumpTime = Time.time;
            grounded = false;

            // Jump animation
            animations.Jump();
        } */
    }

    // Combat related code. Consider moving it to a separate script
    private void Attack(InputAction.CallbackContext ctx)
    {
        weaponManager.currentAttack(true);
        // Play animation
        animations.Attack();
    }

    // Combat related code. Consider moving it to a separate script
    private void gSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(2);
    }

    // Combat related code. Consider moving it to a separate script
    private void sSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(1);
    }

    // Combat related code. Consider moving it to a separate script
    private void cSwap(InputAction.CallbackContext ctx)
    {
        weaponManager.swap(3);
    }

    // Disable and enable movement
    // These two functions maybe be overlapping with built-in function and are redundant
    public void disable()
    {
        movementDisabled = true;
    }

    public void enable()
    {
        movementDisabled = false;
    }

    // Gizmos function - For visual assistance only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

       // Gizmos.color = wallState == WallState.OnWallL ? Color.green : Color.red;
      //  Gizmos.DrawWireSphere(
      //      transform.position + wallCheckCenterOffset - transform.right * wallCheckHorizontalOffset, wallCheckRadius
      //  );
     //   Gizmos.color = wallState == WallState.OnWallR ? Color.green : Color.red;
     //   Gizmos.DrawWireSphere(
      //      transform.position + wallCheckCenterOffset + transform.right * wallCheckHorizontalOffset, wallCheckRadius
      //  );
    }

    // Enable/diable input
    // This function maybe overlapping with OnEnable() and OnDisable() and are redundant
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
            playerInputAction.Player.Fire.Enable();
        }
        else
        {
            playerInputAction.Player.Movement.Disable();
            playerInputAction.Player.Sprint.Disable();
            playerInputAction.Player.Jump.Disable();
            playerInputAction.Player.Swap1.Disable();
            playerInputAction.Player.Swap2.Disable();
            playerInputAction.Player.Swap3.Disable();
            playerInputAction.Player.Fire.Disable();
        }
    }

    public void Reset() {
        animations = allCharacterAnimations[GameManager.manager.GetCurrentCharacter()];
    }

    // Getter function for wall running state
    // Unsed to move camera for better views on wall
   // public bool OnWallR()
  //  {
   //     return wallState == WallState.OnWallR;
   // }
}
