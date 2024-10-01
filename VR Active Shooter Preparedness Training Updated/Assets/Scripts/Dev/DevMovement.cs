using UnityEngine;

public class DevMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    private float desiredY;

    void Start () {
        desiredY = transform.position.y;
    }

    void Update() {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            movement += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += transform.right;
        }

        if (movement != Vector3.zero) {
            movement.Normalize();
            movement *= moveSpeed * Time.deltaTime;
        }

        //transform.position += movement;
        transform.position = new Vector3(transform.position.x + movement.x, desiredY, transform.position.z + movement.z);
    }
}
