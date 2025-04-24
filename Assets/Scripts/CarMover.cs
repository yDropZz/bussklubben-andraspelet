using UnityEngine;

public class CarMover : MonoBehaviour
{

    Transform playerPos;
    private float speed;
    [SerializeField] private float newSpeed = 20f;
    private bool initialized = false;

    void Awake()
    {
       playerPos = FindAnyObjectByType<PlayerController>().transform;
    }

    void Start()
    {

    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        

        if(initialized)
        {
            MoveForward();
        }
        else
        {
            Initialize();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);   
    }

    void Initialize()
    {
        // Set up the car
        if(playerPos.position.z + 250 >= transform.position.z)
        {
            initialized = true;
            Debug.Log("Car initialized: " + gameObject.name);
            speed = newSpeed;
            Destroy(gameObject, 10f);
        }
        else
        {
            initialized = false;
            speed = 0;
        }
        
    }

}
