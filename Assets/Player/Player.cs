using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 2.5f;
    public float laneSwitchSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    public float gravity = -30f;

    [Header("Slide")]
    private float slideCooldown = .5f;
    private bool canSlide = true;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRayLength = 1.2f;
    public LayerMask groundMask;

    private int currentLane = 1;
    private float verticalVelocity = 0f;
    private bool isGrounded;

    [Header("Animation")]
    public Animator animator;
    public float animationSpeed = 1f;


    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        HandleJump();
        Debug.DrawRay(groundCheck.position + transform.forward * 0.5f, (Vector3.down + Vector3.forward * 0.4f).normalized * groundRayLength, Color.magenta);

        if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.D)) MoveRight();
        if(Input.GetKeyDown(KeyCode.S)) Slide();

    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyMovement();
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    void CheckGround()
{
    isGrounded = false;

    // Cast from multiple points (center, front, back)
    Vector3[] origins = new Vector3[]
    {
        groundCheck.position,
        groundCheck.position + transform.forward * 0.4f,
        groundCheck.position - transform.forward * 0.4f
    };

    foreach (var origin in origins)
    {
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundRayLength, groundMask))
        {
            if (verticalVelocity <= 0f)
            {
                isGrounded = true;
                verticalVelocity = 0f;
                break;
            }
        }
    }
}

    void Slide()
    {
        

        if(!isGrounded)
        {
            //if in air when sliding, play downwards anim.

            // Apply a downward force to simulate a slide
            verticalVelocity = -jumpForce * 0.5f;
        }
        else if(isGrounded && canSlide)
        {
            // If on ground when sliding, play slide anim.
            animator.SetTrigger("Slide");

            // Reset vertical velocity to zero when sliding on the ground
            verticalVelocity = 0f;

            // Apply a forward force to simulate sliding
            rb.AddForce(transform.forward * forwardSpeed * 0.5f, ForceMode.VelocityChange);

            StartCoroutine(SlideCooldown());

        }

    }

    private IEnumerator SlideCooldown()
    {
        canSlide = false;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

    void ApplyMovement()
    {
        // Lateral lane switching (X axis)
        float targetX = (currentLane - 1) * laneDistance;
        float smoothX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneSwitchSpeed);

        // Apply gravity if airborne
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        float newY = transform.position.y + verticalVelocity * Time.deltaTime;

        // Forward movement (Z) adjusted to slope normal
        Vector3 forwardMovement = Vector3.forward * forwardSpeed * Time.deltaTime;
        if (GetGroundNormal(out Vector3 groundNormal))
        {
            forwardMovement = Quaternion.FromToRotation(Vector3.up, groundNormal) * forwardMovement;
        }

        // Final movement vector
        Vector3 newPosition = new Vector3(smoothX, newY, transform.position.z) + forwardMovement;
        transform.position = newPosition;
    }

    bool GetGroundNormal(out Vector3 normal)
{
    normal = Vector3.up;

    // Where we'll cast FROM: slightly ahead and down
    Vector3 origin = groundCheck.position + transform.forward * 0.5f;

    // We'll cast down and slightly forward to anticipate slopes
    Vector3 castDir = (Vector3.down + Vector3.forward * 0.4f).normalized;

    if (Physics.Raycast(origin, castDir, out RaycastHit hit, groundRayLength, groundMask))
    {
        normal = hit.normal;
        return true;
    }

    return false;
}

    public void MoveLeft()
    {
        currentLane = Mathf.Max(0, currentLane - 1);
    }

    public void MoveRight()
    {
        currentLane = Mathf.Min(2, currentLane + 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("💥 Hit obstacle!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}