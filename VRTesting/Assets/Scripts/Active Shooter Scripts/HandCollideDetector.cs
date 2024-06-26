using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollideDetector : MonoBehaviour {
    bool colliding = false;

    public bool IsColliding() {
        return colliding;
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Hand") {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.tag == "Hand") {
            colliding = false;
        }
    }
}
