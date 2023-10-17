using UnityEngine;
using System.Collections;

// basic WASD-style movement control
// commented out line demonstrates that transform.Translate instead of charController.Move doesn't have collision detection

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = 9.8f;
    //public float groundDist = 0.4f;
    public LayerMask groundMask;

    private CharacterController _charController;
    //private UIController myUI;
    [SerializeField] private float jumpInterval;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallingBonus;
    private bool grounded;
    public Transform groundCheck;

    private float lastJumpTime = 0f;

    Vector3 velocity;

    void Start()
    {
        Application.targetFrameRate = 60;

        _charController = GetComponent<CharacterController>();
        //myUI = GetComponent<UIController>();

        grounded = false;
    }

    void Update()
    {
        //transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);

        // Ensure on ground
        if (grounded/* && velocity.y < 0*/)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastJumpTime + jumpInterval)
            {
                velocity.y = jumpForce;
                lastJumpTime = Time.time;
                grounded = false;
            }
            else if (_charController.velocity.y <= 0)
            {
                velocity.y = -1f;
            }
        }
        else
        {
            velocity.y -= gravity;
            if (_charController.velocity.y < 0)
            {
                velocity.y -= fallingBonus;
            }
        }
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        //float deltaX = Input.GetAxis("Horizontal");
        //float deltaZ = Input.GetAxis("Vertical");

        //Vector3 movement = transform.right * deltaX + transform.forward * deltaZ;

        //_charController.Move(movement * speed * Time.deltaTime);

        //// Jump
        //if (grounded && Input.GetKeyDown(KeyCode.Space))
        //{
        //    velocity.y = jumpForce;
        //}

        //velocity.y += gravity * Time.deltaTime;
        //if ((_charController.collisionFlags & CollisionFlags.Above) != 0)
        //{
        //    if (velocity.y > 0)
        //    {
        //        velocity.y = -velocity.y;
        //    }
        //}
    }
    private void FixedUpdate()
    {
        Vector3 movement = (transform.right * velocity.x + transform.forward * velocity.z) * speed;
        movement.y = velocity.y;
        _charController.Move(movement * Time.fixedDeltaTime);

        grounded = Physics.CheckSphere(groundCheck.position, 0.45f, groundMask);
    }
}
