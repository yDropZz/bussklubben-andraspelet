using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Float Settings")]
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Smooth spin around Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        // Gentle vertical bob using sine wave
        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z);
    }
}