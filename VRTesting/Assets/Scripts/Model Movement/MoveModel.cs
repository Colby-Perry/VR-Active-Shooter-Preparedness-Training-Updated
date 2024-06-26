using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class MoveModel : MonoBehaviour {

    public GameObject locations;
    private int locationCount;

    public GameObject theNPC;
    public GameObject player;
    public RaycastHit shot;

    private Transform target;
    private Animator animator;
    private bool targetReached = true;
    private float totalDistance;

    private void Start() {
        animator = theNPC.GetComponent<Animator>();

    }

    private void Update() {
        MoveToTarget();

    }

    // Sets a new target location
    private void SetTarget(Transform newTarget) {
        targetReached = false;
        target = newTarget;
        Physics.Raycast(theNPC.transform.position, theNPC.transform.TransformDirection(Vector3.forward), out shot);
        totalDistance = Vector3.Distance(theNPC.transform.position, target.transform.position);

    }

    // Moves closer to target location
    private void MoveToTarget() {

        // If the NPC has already reached its target look at the player
        if (targetReached) {
            theNPC.transform.LookAt(new Vector3(player.transform.position.x, theNPC.transform.position.y, player.transform.position.z));
            return;
        }

        if (target == null) {
            return;
        }

        theNPC.transform.LookAt(target);

        // Determines distance to the target
        Physics.Raycast(theNPC.transform.position, theNPC.transform.TransformDirection(Vector3.forward), out shot);
        //float targetDistance = shot.distance;
        float targetDistance = Vector3.Distance(theNPC.transform.position, target.position);

        // Walks to the target, runs if far enough
        if (targetDistance >= .1f) {
            animator.SetBool("isWalking", true);
            if (totalDistance > 20) {
                animator.SetBool("isRunning", true);
            }

        } else {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            targetReached = true;
        }
    }

    // Sets the models target to the next target
    public void NextTarget() {
        try {
            SetTarget(locations.transform.GetChild(locationCount++));
        } catch (UnityException ex) {
            Debug.LogError("Next target does not exist: " + ex.Message);
        }
    }

    public IEnumerator WaitForFinish() {
        while (!targetReached) {
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
