using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHandler : MonoBehaviour {
    //private MoveModel moveModel; // Moves the guide to preset locations
    //private TriggerHandler triggerHandler; // Triggers for the player
    //public GameObject handlers; // An object that holds a bunch of handlers that relate to the active shooting event

    [SerializeField] GameObject BuildingToDisable;
    [SerializeField] GameObject OtherToDisable;

    [SerializeField] GameObject controllerModels;
    GameObject lControllerModel;
    GameObject rControllerModel;
    Controller_Refrences lControllerRefrences;
    Controller_Refrences rControllerRefrences;

    [SerializeField] GameObject playerRefrence;
    private Player player;

    [SerializeField] GameObject interactionSample;

    // Each corresponds to leftTrigger, rightTrigger, leftGrab, rightGrab, leftTrackpad, and rightTrackpad
    bool[] playerFlags = {false, false, false, false, false, false};

    const double TRIGGER_THRESHOLD = 0.8;
    const double GRAB_THRESHOLD = 0.5;
    const double MOVE_THRESHOLD = 0.1;
    const double TURN_THRESHOLD = 0.1;

    public void StartTutorial() {
        lControllerModel = controllerModels.GetNamedChild("Left Controller");
        rControllerModel = controllerModels.GetNamedChild("Right Controller");
        lControllerRefrences = lControllerModel.GetComponent<Controller_Refrences>();
        rControllerRefrences = rControllerModel.GetComponent<Controller_Refrences>();

        interactionSample.SetActive(false);
        lControllerModel.SetActive(false);
        rControllerModel.SetActive(false);

        player = playerRefrence.GetComponent<Player>();
        StartCoroutine("TutorialCoroutine");
    }

    public void Update() {
        playerFlags[0] = playerFlags[0] || player.GetLeftTrigger() > TRIGGER_THRESHOLD;
        playerFlags[1] = playerFlags[1] || player.GetRightTrigger() > TRIGGER_THRESHOLD;
        playerFlags[2] = playerFlags[2] || player.GetLeftGrab() > GRAB_THRESHOLD;
        playerFlags[3] = playerFlags[3] || player.GetRightGrab() > GRAB_THRESHOLD;
        playerFlags[4] = playerFlags[4] || (player.GetMove().x > MOVE_THRESHOLD) || (player.GetMove().y > MOVE_THRESHOLD)
            || (player.GetMove().x < -MOVE_THRESHOLD) || (player.GetMove().y < -MOVE_THRESHOLD);
        playerFlags[5] = playerFlags[5] || player.GetTurn().x > TURN_THRESHOLD || player.GetTurn().x < -(TURN_THRESHOLD);

        // Move the controllers in front of the camera
        Vector3 viewportPosition = new Vector3(0, 0, player.GetNearClipPlane());
        Vector3 worldPosition = player.GetCamera().ViewportToWorldPoint(viewportPosition);
        controllerModels.transform.position = worldPosition;

    }

    private IEnumerator TutorialCoroutine() {

        // Disable the main building to reduce lag
        if (BuildingToDisable != null) BuildingToDisable.SetActive(false);
        if (OtherToDisable != null) OtherToDisable.SetActive(false);

        lControllerModel.SetActive(true);
        rControllerModel.SetActive(true);

        // 
        Debug.Log("Move your controllers around to get started");
        float lControlDistanceMoved = 0;
        float rControlDistanceMoved = 0;

        // Stays in loop until both left and right controllers have moved
        while (!(lControlDistanceMoved > .5 && rControlDistanceMoved > .5)) {
            Vector3 prevLeftPos = player.GetLeftPosition();
            Vector3 prevRightPos = player.GetRightPosition();
            yield return new WaitForEndOfFrame();

            float leftVelocity = Vector3.Distance(prevLeftPos, player.GetLeftPosition());
            float rightVelocity = Vector3.Distance(prevRightPos, player.GetRightPosition());

            if (leftVelocity > 0.01f && leftVelocity < 1f && lControlDistanceMoved < 10) {
                lControlDistanceMoved += leftVelocity;
            }

            if (leftVelocity > 0.01f && rightVelocity < 1f && rControlDistanceMoved < 10) {
                rControlDistanceMoved += rightVelocity;
            }

            Debug.Log(Vector3.Distance(prevLeftPos, player.GetLeftPosition()) + " " + Vector3.Distance(prevRightPos, player.GetRightPosition()));
            Debug.Log(lControlDistanceMoved + "" + rControlDistanceMoved);
        }

        


        Debug.Log("Turn with right trackpad.");
        playerFlags[1] = false;
        while (!playerFlags[5]) {
            yield return HighlightObjects(rControllerRefrences.trackpad);
        }

        Debug.Log("Move with left trackpad.");
        playerFlags[0] = false;
        while (!playerFlags[4]) {
            yield return HighlightObjects(lControllerRefrences.trackpad);
        }

        Debug.Log("You can grab objects by pressing the buttons on the back side of the controller.");
        int frames = 60;
        for (int i = 0; i < frames; i++) {
            lControllerModel.transform.Rotate(60/frames, 180/frames, 0);
            rControllerModel.transform.Rotate(-60/frames, -180/frames, 0);
            yield return new WaitForEndOfFrame();
        }
        playerFlags[2] = false;
        playerFlags[3] = false;
        GameObject[] grabObjects = { 
            lControllerRefrences.backLeftButton, 
            lControllerRefrences.backRightButton,
            rControllerRefrences.backLeftButton,
            rControllerRefrences.backRightButton};
        while (!(playerFlags[2] || playerFlags[3])) {
            yield return HighlightObjects(grabObjects);
        }

        
        Debug.Log("You can interact buttons by pressing the trigger on the back.");
        while (!playerFlags[1]) {
            yield return HighlightObjects(rControllerRefrences.trigger);
        }

        lControllerModel.SetActive(false);
        rControllerModel.SetActive(false);

        // Summons a table with interactable cubes on it
        float moveAmount = 5.0f;
        interactionSample.transform.position += new Vector3(0, moveAmount, 0);
        interactionSample.SetActive(true);
        frames = 120;
        for (int i = 0; i < frames; i++) {
            interactionSample.transform.position -= new Vector3(0, (moveAmount / frames), 0);
            yield return new WaitForEndOfFrame();
        }
        
        // Initializes the three cubes on the interaction table
        TutorialCube[] colorCubes = {interactionSample.GetNamedChild("Cube1").GetComponent<TutorialCube>(),
                                    interactionSample.GetNamedChild("Cube2").GetComponent<TutorialCube>(),
                                    interactionSample.GetNamedChild("Cube3").GetComponent<TutorialCube>()};
        foreach (TutorialCube cube in colorCubes) {
            cube.CustomStart();
        }

        // Waits for the user to change the color of all the cubes
        bool allCubedChanged = false;
        while(!allCubedChanged) {
            allCubedChanged = true; // This is not true, it will be turned false if not all cubes have changed color
            foreach (TutorialCube cube in colorCubes) {
                allCubedChanged = allCubedChanged && cube.HasColorChanged();
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("AAAAAAAAAAAAAAAAAAAAA TutorialHandler line ~170");


        yield return null;

        // Bring up controllers to the user and say "These are your controllers"
        // Highlight the right controllers trackpad and say "On your right controller, use your thumb on the highlighed trackpad to move around"
        // Wait for user to move around
        // Highlight the left controllers trackpad and say "Use the left trackpad to look around"
        // Wait for user to look around

        // Teleport the player in front of a table with some cubes and a gun on it
        // Rotate the controllers so that the back is facing the player
        // Highlight the grab buttons on both controllers
        // Tell the user "You can grab by squeezing the controller which will press these buttons on the back, go ahead an pickup and throw one of these cubes"

        // Highlight the trigger button on both controllers and say "You can interact with objects by using the trigger, you can do things such as shoot the gun, turn on and off lights, and lock doors"
        // Say "Go ahead and pickup then fire the gun
        // Wait for the player to fire the gun, if the player has not fired the gun within 7 seconds prompt them again with "Grab the gun by squeezing the controller while you hand is near the gun, then fire the gun by pressing in the trigger"

        // Say "Okay good, now that you know the basics go ahead and open the door by grabbing the handle"
        // Wait for the player to open the door
        // Say "Now turn the light off by pressing the trigger button when your hand is near the light"
        // Wait for the player to interact with the light

        // Say "Finally close and lock the door, you can lock the door by using the trigger while your hand is near the handle"
        // Wait for the player to close and lock the door

        // For any of these prompt use a built in timer to re-prompt the user if they do not do the action with a set time

    }

    private IEnumerator HighlightObjects(GameObject obj) {
        GameObject[] objs = { obj };
        yield return HighlightObjects(objs);
    }

    private IEnumerator HighlightObjects(GameObject[] objs) {

        UnityEngine.Color[] originalColors = new UnityEngine.Color[objs.Length];
        UnityEngine.Color targetColor = UnityEngine.Color.yellow;

        float transitionDuration = 1.2f; // Duration of each transition in seconds
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration) {
            float t = elapsedTime / transitionDuration;
            for (int i = 0; i < objs.Length; i++) {
                objs[i].GetComponent<Renderer>().material.color = UnityEngine.Color.Lerp(originalColors[i], targetColor, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(.15f);

        // Ensure the target color is set exactly to avoid interpolation errors.
        for (int i = 0; i < objs.Length; i++) {
            objs[i].GetComponent<Renderer>().material.color = targetColor;
        }

        // Now revert back to the original color
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration) {
            float t = elapsedTime / transitionDuration;
            for (int i = 0; i < objs.Length; i++) {
                objs[i].GetComponent<Renderer>().material.color = UnityEngine.Color.Lerp(targetColor, originalColors[i], t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the original color is set exactly to avoid interpolation errors.
        for (int i = 0; i < objs.Length; i++) {
            objs[i].GetComponent<Renderer>().material.color = originalColors[i];
        }

        yield return new WaitForSecondsRealtime(.15f);
    }
}
