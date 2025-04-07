using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 offset = new Vector3(0, 12, -32);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
