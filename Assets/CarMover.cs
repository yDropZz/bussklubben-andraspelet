using UnityEngine;

public class CarMover : MonoBehaviour
{

    [SerializeField] private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);   
    }

    void Initialize()
    {
        // Set up the car
        
    }

}
