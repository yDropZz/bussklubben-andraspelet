using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lateralMoveDuration = 0.2f;
    [SerializeField] private float trackDistance = 10f;
    [SerializeField] private AnimationCurve lateralMoveCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private int currentTrack = 1; // 0 = left, 1 = middle, 2 = right
    private bool isMovingSideways = false;

    void Update()
    {
        // Forward movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Lateral movement
        if (!isMovingSideways)
        {
            if (Input.GetKeyDown(KeyCode.A) && currentTrack > 0)
            {
                currentTrack--;
                StartCoroutine(MoveTrack());
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentTrack < 2)
            {
                currentTrack++;
                StartCoroutine(MoveTrack());
            }
        }
    }

    private IEnumerator MoveTrack()
{
    isMovingSideways = true;

    float elapsed = 0f;
    Vector3 startPos = transform.position;
    Vector3 endPos = new Vector3((currentTrack - 1) * trackDistance, startPos.y, startPos.z);

    while (elapsed < lateralMoveDuration)
    {
        float t = elapsed / lateralMoveDuration;
        float curvedT = lateralMoveCurve.Evaluate(t);

        transform.position = Vector3.Lerp(startPos, endPos, curvedT);
        elapsed += Time.deltaTime;

        // Forward movement during sidestep
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        yield return null;
    }

    transform.position = new Vector3(endPos.x, endPos.y, transform.position.z);
    isMovingSideways = false;
}
}