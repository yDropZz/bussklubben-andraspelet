using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
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
    private float slideCooldown = 0.5f;
    private bool canSlide = true;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRayLength = 1.2f;
    public LayerMask groundMask;

    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private float verticalVelocity = 0f;
    private bool isGrounded;

    [Header("Animation")]
    public Animator animator;
    public float animationSpeed = 1f;

    [Header("Rocket Boots")]
    [SerializeField] private float rocketBootsBoost = 2f;
    [SerializeField] private float rocketBootsDuration = 8f;
    [SerializeField] private GameObject rocketBootsPrefab1;
    [SerializeField] private GameObject rocketBootsPrefab2;
    private bool rocketBoots = false;

    [Header("Rocket Bus")]
    [SerializeField] private GameObject rocketBusPrefab;
    [SerializeField] private float busSpeed = 120f;
    [SerializeField] private float busDuration = 5f;
    [SerializeField] private ParticleSystem busParticleSystem;
    private bool inBus = false;
    private bool ignoreNormalMovement = false;

    private CharacterController controller;
    private float targetX; // Store the target X position for lane switching

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleInput();
        CheckGround();
    }

    void HandleInput()
    {
        //if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
        //if (Input.GetKeyDown(KeyCode.D)) MoveRight();
        if(!inBus)
        {
        HandleLaneSwitching();
        if (Input.GetKeyDown(KeyCode.W)) HandleJump();
        if (Input.GetKeyDown(KeyCode.Space)) HandleJump();
        if (Input.GetKeyDown(KeyCode.S)) Slide();
        }
    }

    

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleLaneSwitching()
{
    if (Input.GetKeyDown(KeyCode.A))
    {
        currentLane = Mathf.Max(0, currentLane - 1); // Move left
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
        currentLane = Mathf.Min(2, currentLane + 1); // Move right
    }

    // Calculate the target X position based on the current lane
    targetX = (currentLane - 1) * laneDistance;
}

    void HandleJump()
    {
        if (isGrounded)
        {
            verticalVelocity = jumpForce * (rocketBoots ? rocketBootsBoost : 1f);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    void Slide()
    {
        if (!canSlide) return;

        if (!isGrounded)
        {
            verticalVelocity = -jumpForce * 0.5f;
        }
        else
        {
            animator.SetTrigger("Slide");
            StartCoroutine(SlideCooldown());
        }
    }

    IEnumerator SlideCooldown()
    {
        canSlide = false;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

    bool GetGroundNormal(out Vector3 normal)
    {
        normal = Vector3.up;

        // Cast slightly ahead and down to read slopes
        Vector3 origin = groundCheck.position + transform.forward * 0.5f;
        Vector3 castDir = (Vector3.down + Vector3.forward * 0.4f).normalized;

        if (Physics.Raycast(origin, castDir, out RaycastHit hit, groundRayLength, groundMask))
        {
            normal = hit.normal;
            return true;
        }

        return false;
    }

    

    void ApplyMovement()
{
    if (ignoreNormalMovement) return; // Skip normal movement during bus power-up

    // --- LANE SWITCHING ---
    float smoothedX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime);
    float deltaX = smoothedX - transform.position.x;

    // --- GRAVITY ---
    if (!isGrounded)
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    // --- VERTICAL ---
    float deltaY = verticalVelocity * Time.deltaTime;

    // --- FORWARD MOVEMENT ---
    float deltaZ = forwardSpeed * Time.deltaTime;

    // Combine movement
    Vector3 move = new Vector3(deltaX, deltaY, deltaZ);
    controller.Move(move);
}

    void CheckGround()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f;
        }
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
        else if (other.CompareTag("Trampoline"))
        {
            verticalVelocity = jumpForce * 1.5f;
        }
        else if (other.CompareTag("Rocketboots"))
        {
            StartCoroutine(ApplyRocketBoots(rocketBootsDuration));
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Buss"))
        {
            StartCoroutine(ApplyRocketBus(busDuration));
            Destroy(other.gameObject);
        }
    }

    IEnumerator ApplyRocketBoots(float duration)
    {
        if (rocketBoots) yield break;

        rocketBoots = true;
        rocketBootsPrefab1.SetActive(true);
        rocketBootsPrefab2.SetActive(true);
        Debug.Log("Rocket boots activated!");
        yield return new WaitForSeconds(duration);
        rocketBootsPrefab1.SetActive(false);
        rocketBootsPrefab2.SetActive(false);
        rocketBoots = false;
        Debug.Log("Rocket boots deactivated!");
    }

    IEnumerator ApplyRocketBus(float duration)
{
    if (inBus) yield break;

    inBus = true;
    ignoreNormalMovement = true;

    // Play particle effects for the bus
    if (busParticleSystem != null) busParticleSystem.Play();
    rocketBusPrefab.SetActive(true);

    // Store original forward speed
    float originalSpeed = forwardSpeed;
    forwardSpeed = busSpeed;

    // --- Upward Phase ---
    float targetY = 20f; // Height to reach
    float upwardDuration = 1.5f; // Time to reach target height
    float elapsedTime = 0f;
    float startY = transform.position.y;

    while (elapsedTime < upwardDuration)
    {
        HandleLaneSwitching(); // Allow lane switching while flying

        // Smooth upward movement
        float t = elapsedTime / upwardDuration;
        float y = Mathf.Lerp(startY, targetY, t); // Smoothly interpolate Y position
        float deltaY = y - transform.position.y;  // Get difference in Y
        float deltaX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime) - transform.position.x; // Smooth X movement
        Vector3 move = new Vector3(deltaX, deltaY, forwardSpeed * Time.deltaTime);
        controller.Move(move);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // --- Hover Phase ---
    float hoverTime = duration - upwardDuration; // Remaining time after upward phase
    elapsedTime = 0f;
    float hoverY = transform.position.y; // Current Y position during hover

    while (elapsedTime < hoverTime)
    {
        HandleLaneSwitching(); // Allow lane switching while hovering

        // Lock Y position to the hoverY value and move forward
        float deltaX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime) - transform.position.x; // Smooth X movement
        Vector3 move = new Vector3(deltaX, 0f, forwardSpeed * Time.deltaTime);
        controller.Move(move);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // --- Descend Phase ---
    float descendDuration = 1f; // Duration of the descent
    elapsedTime = 0f;
    float groundY = 1f; // Ground level

    while (elapsedTime < descendDuration)
    {
        HandleLaneSwitching(); // Allow lane switching while descending

        // Smooth descent
        float t = elapsedTime / descendDuration;
        float y = Mathf.Lerp(hoverY, groundY, t); // Smoothly interpolate down to the ground
        float deltaY = y - transform.position.y;
        float deltaX = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime) - transform.position.x; // Smooth X movement
        Vector3 move = new Vector3(deltaX, deltaY, forwardSpeed * Time.deltaTime);
        controller.Move(move);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // --- End Bus Ride ---
    if (busParticleSystem != null) busParticleSystem.Stop();
    rocketBusPrefab.SetActive(false);
    forwardSpeed = originalSpeed; // Restore the original forward speed
    inBus = false;
    ignoreNormalMovement = false; // Allow normal movement again
}

}
