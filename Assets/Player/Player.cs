using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;       // Speed at which the character moves forward
    public float laneDistance = 2.5f;      // Distance between lanes
    public float laneSwitchSpeed = 10f;    // Speed of lane switching
    private int currentLane = 1;           // Current lane (0, 1, 2 for 3 lanes)

    [Header("Jumping")]
    public float jumpForce = 8f;           // Jump force applied when the player jumps
    public float gravity = -30f;           // Custom gravity applied to the player

    [Header("Ground Detection")]
    public LayerMask groundMask;           // Layer mask to detect ground
    public float groundRayLength = 1.2f;   // Ray length for ground detection
    public Transform groundCheck;          // Position to check for ground

    private float verticalVelocity = 0f;    // Current vertical velocity (for jumping and gravity)
    private bool isGrounded = false;        // Check if the player is on the ground
    private float groundY;                 // Store the Y position of the ground

    private Rigidbody rb;                  // Rigidbody component reference

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // We handle gravity manually
        rb.linearDamping = 0f;           // Prevent drag slowing down the player
        rb.angularDamping = 0f;    // Prevent angular drag
    }

    void FixedUpdate()
    {
        // Update ground detection and handle movement
        CheckGround();
        ApplyMovement();
    }

    void Update()
    {
        HandleJump();
        HandleLaneSwitch();
    }

    #region Movement

    // Handle movement, lane switching, and applying gravity
    void ApplyMovement()
    {
        // Handle X (lane switching)
        float targetX = (currentLane - 1) * laneDistance;
        float smoothX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneSwitchSpeed);

        // Handle Z (forward speed)
        Vector3 velocity = rb.linearVelocity;
        velocity.x = smoothX - transform.position.x;  // Smooth lane switching on X-axis
        velocity.z = forwardSpeed;                     // Constant forward movement (no gradual slowdown)

        // Handle Y (gravity and jump)
        if (isGrounded)
        {
            verticalVelocity = 0f; // No gravity when grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity if not grounded
        }

        velocity.y = verticalVelocity; // Apply vertical velocity (jump/gravity)

        // Apply the final movement using Rigidbody's velocity
        rb.linearVelocity = velocity;
    }

    #endregion

    #region Jumping

    // Handle jumping mechanics
    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce; // Apply jump force
            isGrounded = false;           // Mark as not grounded during jump
        }
    }

    #endregion

    #region Ground Detection

    // Check if the player is grounded by casting a ray from the groundCheck position
    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundRayLength, groundMask))
        {
            isGrounded = true;
            groundY = hit.point.y;  // Store the ground Y position for consistency
        }
        else
        {
            isGrounded = false;
        }

        // Keep player aligned with the ground Y position to avoid floating or falling through
        if (isGrounded)
        {
            rb.position = new Vector3(rb.position.x, groundY, rb.position.z);
        }
    }

    #endregion

    #region Lane Switching

    // Handle lane switching (left and right)
    void HandleLaneSwitch()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }

    // Move the player to the left lane
    public void MoveLeft()
    {
        currentLane = Mathf.Max(0, currentLane - 1);
    }

    // Move the player to the right lane
    public void MoveRight()
    {
        currentLane = Mathf.Min(2, currentLane + 1);
    }

    #endregion

    #region Collision Handling

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Handle obstacle collision (you can reset the level or trigger a game over)
            Debug.Log("Hit obstacle!");
        }
    }

    #endregion
}