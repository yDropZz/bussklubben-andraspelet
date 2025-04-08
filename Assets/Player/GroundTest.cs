using UnityEngine;

public class GroundTest : MonoBehaviour
{
    public LayerMask groundMask;
    
    void Update()
    {
        Vector3 origin = transform.position + Vector3.down * 0.9f;
        bool grounded = Physics.Raycast(origin, Vector3.down, 3f, groundMask);
        Debug.DrawRay(origin, Vector3.down * 0.2f, grounded ? Color.green : Color.red);

        if (grounded)
            Debug.Log("✅ Grounded!");
        else
            Debug.Log("❌ Not grounded");
    }
}