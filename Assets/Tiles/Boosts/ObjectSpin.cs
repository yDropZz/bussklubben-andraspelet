using UnityEngine;

public class ObjectSpin : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatAmplitude = .5f;
    [SerializeField] private float floatFrequency = 1f;
    [SerializeField] private float floatSpeed = 1f;
    private Vector3 startPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        //Sine wave to calculate new pos
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        transform.position = new Vector3(transform.position.x, newY, startPosition.z);
        
    }
}
