using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorRotationManager : MonoBehaviour {
    // References to various objects in the scene
    [SerializeField] Transform doorModel;
    [SerializeField] Transform doorFront;
    [SerializeField] Transform doorBack;
    [SerializeField] Transform handleModel;
    [SerializeField] Transform doorPivot;
    [SerializeField] Transform handlePivot;

    // Limits for door rotation angle
    [SerializeField] float unconvertedMinAngle = -90f;
    [SerializeField] float maxAngle = 90f;

    [SerializeField] bool useHandle = true;

    // An array that is used to store the past five frames of movement
    // Take the average of the array to calculate speed
    float[] speedTracker = new float[5];
    // Used to determine what position the need frame of movement should be stored in
    int speedTrackerCount;

    float minAngle;

    // Current angle of the door
    float currentAngle;

    // Reference to a script component for handling target interaction
    // Needed because target will get deleted, so refrence the target via targetHandler.GetTargetTransform()
    TargetHandler targetHandler;

    // Refrence to the script that controlls the handle models rotation
    // Used to get angle of the handle model
    HandleController handleController;

    // Scripts of doorFront and doorBack that track if they are currently colliding with something
    CollisionDetector frontCollider;
    CollisionDetector backCollider;

    // Flag to control whether the door can be rotated
    bool canRotate;

    bool doorOpen;

    void Start() {

        // Converts min angle as an angle cannot be negative
        minAngle = (360f + unconvertedMinAngle);

        // Gets various classes
        targetHandler = handleModel.GetComponent<TargetHandler>();
        handleController = handleModel.GetComponent<HandleController>();
        frontCollider = doorFront.GetComponent<CollisionDetector>();
        backCollider = doorBack.GetComponent<CollisionDetector>();

        // defaults speedTrackerCount to 0
        speedTrackerCount = 0;

        canRotate = true;
    }

    void Update() {
        // Current angle of the door
        currentAngle = doorModel.transform.localRotation.eulerAngles.y;

        // If the doors angle isn't close to zero
        doorOpen = (currentAngle > 3f && currentAngle < 357f);

        // If the handle is disabled or the handle is opened
        bool doorAbleToOpen = (!useHandle || handleController.IsHandleOpen());

        // Only active is useHandle is true
        // If both the handle and door aren't open return
        if (!doorOpen && !doorAbleToOpen) {
            if(currentAngle == 0) {
                return;
            } else {
                UnrestricedRotateDoor(0 - currentAngle);
                return;
            }
        }

        if (targetHandler.IsTargetGrabbed()) {
            RotateHandler();

        } else {
            PhysicsRotator();
        }

        //Counts up to 5 then resets to 0
        speedTrackerCount = (speedTrackerCount + 1) % 5;
    }

    public bool IsLocked() {
        return handleController.IsLocked();
    }

    public bool IsClosed() {
        return !doorOpen;
    }

    // Calculates front distance between target and the handle and calls RotateDoor to rotate that amount in degrees * 24
    void RotateHandler() {
        // Calculate the vector between the handle and the target
        Vector3 distanceVector = handleModel.position - targetHandler.GetTargetTransform().position;

        // Calculate the direction in which the door is facing
        Vector3 facingDirection = doorFront.position - doorBack.position;

        // Calculate the distance along the direction vector
        float distanceAlongAxis = Vector3.Dot(distanceVector, facingDirection);

        // Rotate the door based on the calculated distance
        RotateDoor(distanceAlongAxis * 24);

    }

    // Simulates door physics
    void PhysicsRotator() {
        
       /* This is meant to run when the player stops grabbing the door and we need to simulate the physics of it swinging
        * Does this by getting the average amount that the door has rotated the past five frames
        * Rotates the door by the average again, but decrease the rotation amount by .015, which slowly brings the average to zero
        * The average gets close to zero we know the momentum of the door has stopped
        * It also simulates a spring affect so the door automatically closes
        * Does this by rotating the door to the original position when the average is close to zero
        */

        // Finds the average speed of the speedTracker array
        float sum = 0.0f;
        foreach (float value in speedTracker) {
            sum += value;
        }
        float average = sum / speedTracker.Length;

        // Rotates door based on average
        if (average > 0.01f) {
            RotateDoor(average - .015f);

        } else if (average < -0.01f) {
            RotateDoor(average + .015f);

        } else if (currentAngle > 180) {
            RotateDoor(.03f);
            speedTracker[speedTrackerCount] = 0; // Don't want to include this action in the averago, so we erase it

        } else if (currentAngle < 180) {
            RotateDoor(-.03f);
            speedTracker[speedTrackerCount] = 0; // Don't want to include this action in the averago, so we erase it
        }
        

    }

    bool CanDoorRotate(float amount) {

        if (!canRotate) {
            return false;
        }

        // Stops the door from rotating if it is near the minAngle and would continue to rotate that direction
        if (currentAngle > minAngle - .5f && currentAngle < minAngle + .5f && amount < 0) {
            return false;
        }

        // Stops the door from rotating if it is near the maxAngle and would continue to rotate that direction
        if (currentAngle > maxAngle - .5f && currentAngle < maxAngle + .5f && amount > 0) {
            return false;
        }

        // If the front of the door is colliding with something and the door would rotate towards that way return false
        if (frontCollider.IsColliding() && amount < 0) {
            return false;
        }

        // If the back of the door is colliding with something and the door would rotate towards that way return false
        if (backCollider.IsColliding() && amount > 0) {
            return false;
        }

        // Return true if nothing would stop the door from rotating
        return true;
    }

    // Attempts to rotate door amount while handling desired exceptions
    void RotateDoor(float amount) {
        
        // Initializes current rotation speed
        // Will change based on how hard the player is pulling the handle
        float rotationSpeed = 0;

        if (!CanDoorRotate(amount)) {
            return;
        }

        // Handle rotation scenarios based on angle limits
        if (currentAngle + amount <= minAngle && currentAngle + amount >= minAngle - 10f) {
            // Rotate the door to the minimum angle
            UnrestricedRotateDoor((minAngle + 1) - currentAngle);
            rotationSpeed = 0; // No needs to keep momentum of the door when at its limit, speed set to zero

        } else if (currentAngle + amount >= maxAngle && currentAngle + amount <= maxAngle + 10f) {
            // Rotate the door to the maximum angle
            UnrestricedRotateDoor((maxAngle - 1) - currentAngle);
            rotationSpeed = 0; // No needs to keep momentum of the door when at its limit, speed set to zero

        } else {
            // Rotate the door by the specified amount
            UnrestricedRotateDoor(amount);
            rotationSpeed = amount; // Tracks current rotation speed
        }

        // Tracks past 5 rotation speeds
        speedTracker[speedTrackerCount] = rotationSpeed;
    }

    // Rotate various components of the door around the pivot
    private void UnrestricedRotateDoor(float amount) {
        doorModel.transform.RotateAround(doorPivot.position, Vector3.up, amount);
        doorFront.transform.RotateAround(doorPivot.position, Vector3.up, amount);
        doorBack.transform.RotateAround(doorPivot.position, Vector3.up, amount);
        handleModel.transform.RotateAround(doorPivot.position, Vector3.up, amount);
        doorPivot.transform.RotateAround(doorPivot.position, Vector3.up, amount);
        handlePivot.transform.RotateAround(doorPivot.position, Vector3.up, amount);
    }
}
