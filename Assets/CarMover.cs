using UnityEngine;

public class CarMover : MonoBehaviour
{

    private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = -newSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);   
    }

}
