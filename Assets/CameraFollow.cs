using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new Vector3(0, 12, -32);
    [SerializeField] private float smoothSpeed = 0.125f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    } 
}
