using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 5f;

    private CharacterController controller;
    private int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private Vector3 targetPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // Handle lane switching input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane = Mathf.Max(currentLane - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane = Mathf.Min(currentLane + 1, 2);
        }

        // Calculate target position based on current lane
        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smoothly interpolate to target position
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = (targetPosition.x - transform.position.x) * laneSwitchSpeed;
        moveDirection.z = forwardSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }
}