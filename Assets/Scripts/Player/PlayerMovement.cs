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
    Rigidbody rB;
    public Transform orientation;

    [Header("Movement Speed")]
    [SerializeField][Min(0)] private float walkSpeed;
    [SerializeField][Min(0)] private float runSpeed;
    // Player movement vector is based on a physics-based calculation using custom gravity strength
    // Thus the global gravity factor has no effect on the player. Adjust the value below instead if necessary
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashSpeedChangeFactor;
    public float dashDuration;
    public float dashCd;
    private float dashCdTimer;
    private Vector3 delayedForceToApply;

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

   // [Header("Wall Run Settings")]
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

    [Header("Dash Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;
    public float dashForce;
    public float dashUpwardForce;

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
    public bool dashing;
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
        playerInputAction.Player.AltFire.started += Dash;

        characterController = GetComponent<CharacterController>();
        // Combat related code. Consider moving it to a separate script
        weaponManager = GetComponentInChildren<WeaponManager>();
    
        //stuff = GetComponent<PlayerInventory>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        playerInputAction.Player.AltFire.Enable();
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
        playerInputAction.Player.AltFire.Disable();
    }

    private void Update()
    {
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
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


           
                // Running/walking speed
                movement =
                    (transform.right * horizontalVelocity.x + transform.forward * horizontalVelocity.y).normalized *
                    (sprinting ? runSpeed : walkSpeed);
                // Sprinting: if the player is walking/running slowly or running/running fast
           // 

            movement.y = yVelocity;

            characterController.Move(movement * Time.fixedDeltaTime);

            // Ground check
            grounded = Physics.CheckSphere(transform.position, groundCheckRadius, whatIsEnvironment);

            // Combat related code. Consider moving it to a separate script
            if (playerInputAction.Player.Fire.phase == InputActionPhase.Performed) { weaponManager.currentAttack(false); }
        }
       
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
        
    }

    private void Dash(InputAction.CallbackContext ctx){
        Debug.Log("dash");
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        dashing = true;


        Transform forwardT;

        forwardT = orientation; /// where you're facing (no up or down)

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        dashing = false;

        if (disableGravity)
            rb.useGravity = true;
    }
    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Debug.Log("hInput: " + horizontalInput + " vInput: " + verticalInput);

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
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
