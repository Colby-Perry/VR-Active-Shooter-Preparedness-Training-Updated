using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    void Start() {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Get mouse movement inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the vertical rotation (look up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp rotation to prevent flipping

        // Apply the rotations to the object
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y + mouseX, 0f);
    }
}
