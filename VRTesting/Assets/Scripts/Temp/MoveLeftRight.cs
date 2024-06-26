using UnityEngine;

public class MoveLeftRight : MonoBehaviour {
    // Speed at which the object moves
    public float speed = 2f;
    // Distance the object moves from side to side
    public float distance = 3f;

    // Initial position of the object
    private Vector3 startPosition;
    // Rigidbody component
    private Rigidbody rb;

    void Start() {
        // Store the initial position of the object
        startPosition = transform.position;
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Ensure the Rigidbody is not affected by gravity and kinematic
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void FixedUpdate() {
        // Calculate new position using PingPong for smooth back-and-forth motion
        float newX = startPosition.x + Mathf.PingPong(Time.time * speed, distance * 2) - distance;

        // Apply new position using Rigidbody
        rb.MovePosition(new Vector3(newX, startPosition.y, startPosition.z));
    }
}
