using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset;
    public float smoothSpeed = 2f;

    private Transform playerPosition;

    private void Start()
    {
        playerPosition = FindObjectOfType<PlayerMovementScript>().transform;
    }

    void FixedUpdate()
    {
        Vector3 targerPosition = playerPosition.position + offset; ;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targerPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.LookAt(playerPosition);
    }
}
