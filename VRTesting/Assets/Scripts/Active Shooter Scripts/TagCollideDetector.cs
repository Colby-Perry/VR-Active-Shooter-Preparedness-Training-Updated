using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCollideDetector : MonoBehaviour
{

    [SerializeField] string myTag;
    bool colliding = false;

    public bool IsColliding() {
        return colliding;
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == myTag) {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.tag == myTag) {
            colliding = false;
        }
    }
}
