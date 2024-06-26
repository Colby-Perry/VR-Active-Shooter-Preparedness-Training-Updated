using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class TargetHandler : MonoBehaviour {

    [SerializeField] GameObject sampleTarget; // Prefab for the target object

    private GameObject target; // Current target object
    private GrabStatusTracker grabStatusTracker; // Component to track grab status

    private bool targetJustGrabbed; // Flag to track if the target was just grabbed
    private bool isLocked;

    private void Start() {
        InstantiateTarget(); // Create the initial target
        targetJustGrabbed = false;
        isLocked = false;
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (grabStatusTracker.IsGrabbed()) {
            DropHandleWhenOutOfRange(); // Handle dropping logic when grabbed
            targetJustGrabbed = true;

        } else if (targetJustGrabbed) {
            ResetTarget(); // Reset the target when it was just grabbed but not being held

        } else {
            TargetFollowHandle(); // Make the target follow the handle's position and rotation
        }
    }

    public Boolean IsTargetGrabbed() {
        return grabStatusTracker.IsGrabbed();
    }

    public Transform GetTargetTransform() {
        return target.transform;
    }

    // Make target follow handle when not being held
    void TargetFollowHandle() {
        if (Vector3.Distance(target.transform.position, transform.position) < 0.001f) {
            return; // Skip if target and handle positions are very close
        }

        target.transform.position = transform.position;
        target.transform.rotation = transform.rotation;
    }

    // Forces the player to drop the handle when too far from the handle model
    private void DropHandleWhenOutOfRange() {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 0.5f) {
            ResetTarget(); // Reset the target if out of range
        }
    }

    private void ToggleLocked(ActivateEventArgs arg0) {
        gameObject.GetComponent<AudioSource>().Play();
        isLocked = !isLocked;
    }

    public bool IsLocked() {
        return isLocked;
    }

    // Forces the player to drop the target by destroying and replacing it
    private void ResetTarget() {
        grabStatusTracker.activated.RemoveAllListeners();
        Destroy(target);
        InstantiateTarget();
        grabStatusTracker.activated.AddListener(ToggleLocked);
        targetJustGrabbed = false;
    }

    // Creates a target and sets its size and position to match the handle model
    private void InstantiateTarget() {
        target = Instantiate(sampleTarget); // Instantiate the target prefab
        target.transform.SetParent(transform.parent); // Set target's parent to the same parent as the handle
        target.transform.position = transform.position;
        target.transform.rotation = transform.rotation;
        target.transform.localScale = transform.localScale;
        target.transform.name = "Target";

        grabStatusTracker = target.GetComponent<GrabStatusTracker>(); // Get the GrabStatusTracker component from the new target
    }
}
