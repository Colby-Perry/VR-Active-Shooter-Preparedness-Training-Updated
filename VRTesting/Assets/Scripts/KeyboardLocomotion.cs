using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicPlayerController : MonoBehaviour {
    /* 
    Create a variable called 'rb' that will represent the 
    rigid body of this object.
    */
    private Rigidbody rb;

    // Create a public variable for the cameraTarget object
    public GameObject XROrigin;
    /* 
    You will need to set the cameraTarget object in Unity. 
    The direction this object is facing will be used to determine
    the direction of forces we will apply to our player.
    */
    public float movementIntensity;
    /* 
    Creates a public variable that will be used to set 
    the movement intensity (from within Unity)
    */

    void Start() {
        movementIntensity *= .02f;
    }

    void Update() {
        /* 
    	Establish some directions 
    	based on the cameraTarget object's orientation 
    	*/

        //values multiplied by -1 beacause directions are reversed for some reason
        var ForwardDirection = XROrigin.transform.forward * -1;
        var RightDirection = XROrigin.transform.right * -1;

        // Move Forwards
        if (Input.GetKey(KeyCode.W)) {
            XROrigin.transform.position += ForwardDirection * movementIntensity;
            /* You may want to try using velocity rather than force.
            This allows for a more responsive control of the movement
            possibly better suited to first person controls, eg: */
            //rb.velocity = ForwardDirection * movementIntensity;
        }
        // Move Backwards
        if (Input.GetKey(KeyCode.S)) {
            // Adding a negative to the direction reverses it
            XROrigin.transform.position += -ForwardDirection * movementIntensity;
        }
        // Move Rightwards
        if (Input.GetKey(KeyCode.D)) {
            XROrigin.transform.position += RightDirection * movementIntensity;
        }
        // Move Leftwards
        if (Input.GetKey(KeyCode.A)) {
            XROrigin.transform.position += -RightDirection * movementIntensity;
        }
    }
}