using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

    bool isColliding; // A boolean to track whether the object is currently colliding with specific objects.

    private void Start() {
        isColliding = false; // Initialize the isColliding boolean to false when the object starts.
    }

    public bool IsColliding() {

        return isColliding; // A public method to retrieve the current collision status.
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerTrigger")) {
            isColliding = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayerTrigger")) {
            isColliding = false;
        }
    }
}