using UnityEngine;

public class OscillatingObstacle : MonoBehaviour
{
    [Header("Oscillation Settings")]
    [SerializeField] private Vector3 movementDirection = Vector3.right; // Direction of movement
    [SerializeField] private float speed = 2f; // Speed of oscillation
    [SerializeField] private float distance = 3f; // Maximum distance from the start position
    [SerializeField] private float startDelay = 0f; // Delay before oscillation starts (in seconds)

    private Vector3 startPosition;
    private float startTime;

    void Start()
    {
        startPosition = transform.position;
        startTime = Time.time + startDelay; // Set the delay before movement starts
    }

    void Update()
    {
        if (Time.time < startTime) return; // Wait for the delay to pass

        // Apply an offset based on the delay
        float offset = Mathf.Sin((Time.time - startTime) * speed) * distance;
        transform.position = startPosition + movementDirection.normalized * offset;
    }
}