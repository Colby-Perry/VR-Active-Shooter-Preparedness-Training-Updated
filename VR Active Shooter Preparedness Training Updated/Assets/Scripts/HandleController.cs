using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleController : MonoBehaviour {

    [SerializeField] Transform handleModel; // Reference to the handle's visual model
    [SerializeField] Transform handlePivot; // Pivot point for rotation
    TargetHandler targetHandler; // Reference to a script for handling target interactions

    // These are only used to determine the direction the door is facing
    [SerializeField] Transform doorFront; // Front side of the door
    [SerializeField] Transform doorBack; // Back side of the door

    float maxRotation = 60; // Maximum rotation angle for the handle
    float rotation; // Current rotation angle of the handle

    void Start() {
        targetHandler = handleModel.GetComponent<TargetHandler>(); // Get the TargetHandler script component from the handle model
        
    }

    // Update is called once per frame
    void Update() {

        rotation = handleModel.eulerAngles.x; // Get the current X rotation of the handle

        if (targetHandler.IsTargetGrabbed() && !targetHandler.IsLocked()) {
            UpdateRotation(); // Call a function to update the rotation of the handle based on user input

        } else if (rotation > 1f && rotation < 358f) {
            UnrestricedRotate(-1.5f); // Rotate the handle back towards its original position if not grabbed
        }
    }

    bool CanRotate(float amount) {
        // Check if the handle can rotate based on its current rotation and the rotation amount
        if (rotation >= 355f && amount < 0) {
            return false;
        } else if (rotation >= maxRotation && rotation < 355f && amount > 0) {
            return false;
        } else {
            return true;
        }
    }

    public bool IsHandleOpen() {
        return rotation > 30f && rotation < 355; // Check if the handle is in an open position based on its rotation angle
    }

    public bool IsLocked() {
        return targetHandler.IsLocked();
    }

    void UpdateRotation() {
        float yDifference = handleModel.transform.position.y - targetHandler.GetTargetTransform().transform.position.y;

        // Check if the handle can rotate based on the difference in Y positions between the handle and a target
        if (CanRotate(yDifference)) {

            if (yDifference > 0.3) {
                yDifference = 0.3f; // Limit the rotation amount if the Y difference is too large
            }

            UnrestricedRotate(yDifference * 30); // Rotate the handle based on the Y difference
        }
    }

    void UnrestricedRotate(float amount) {
        Vector3 facingDirection = doorFront.position - doorBack.position; // Calculate the facing direction of the handle
        handleModel.RotateAround(handlePivot.position, facingDirection, amount); // Rotate the handle around the pivot point in the calculated direction
    }
}