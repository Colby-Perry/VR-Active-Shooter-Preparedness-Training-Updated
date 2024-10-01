using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainEventBus : MonoBehaviour
{
    AudioSource audioSource; // Gunshot Sound

    public bool roomEventOverride = false; // dev tool to skip required events after entering the first room

    public GameObject handlers; // An object that holds a bunch of handlers that relate to the active shooting event

    // These are not specific to the active shooting event, hence why they aren't part of 'handlers'
    public GameObject door; // Refrence to door handler script
    public GameObject lightSwitch; // Refrence to light handler script

    //private InfoBoardHandler infoBoardHandler; // Used to enable and disable info boards when needed
    private MoveModel moveModel; // Moves the guide to preset locations
    private TriggerHandler triggerHandler; // Triggers for the player
    //private PhoneSummoner leftPhone; // Summones phone in left pocket
    //private PhoneSummoner rightPhone; // Summones phone in right pocket

    private SlidingDoorManager doorHandler; // Refrence to information about the door, is the door open, locked etc.
    private LightSwitchController lightHandler; // Can see if the light is on or off

    public GameObject tutorial;

    void Start() {

        //infoBoardHandler = handlers.GetComponent<InfoBoardHandler>();
        moveModel = handlers.GetComponent<MoveModel>();
        triggerHandler = handlers.GetComponent<TriggerHandler>();
        //leftPhone = handlers.GetComponents<PhoneSummoner>()[0];
        //rightPhone = handlers.GetComponents<PhoneSummoner>()[1];

        doorHandler = door.GetComponent<SlidingDoorManager>();
        lightHandler = lightSwitch.GetComponent<LightSwitchController>();
        audioSource = gameObject.GetComponent<AudioSource>();

        //StartCoroutine(MainBus());

        TutorialHandler tutorialHandler = tutorial.GetComponent<TutorialHandler>();
        tutorialHandler.StartTutorial();

    }

    // Times all active shooter events
    private IEnumerator MainBus() {

        // Plays gunshot sounds
        float wait = 4, shots = 3, interval = .3f;
        yield return new WaitForSecondsRealtime(wait);
        for (int i = 0; i < shots; i++) {
            audioSource.Play();
            yield return new WaitForSecondsRealtime(interval);
        }

        // Displays the first info board in the lobby, telling the user to take cover in the nearest room
        yield return new WaitForSecondsRealtime(1);
        //infoBoardHandler.NextBoard();
        yield return new WaitForSecondsRealtime(4);

        // Waits for the guide to go infront of the rooms door
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();

        // Waits for the player to enter the room
        triggerHandler.NextTrigger();
        yield return triggerHandler.WaitForPlayer();

        // Moves the guide in the room and displays the next board telling the user to turn off light, lock door, and silence their phone
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        //infoBoardHandler.NextBoard();

        // Waits until player has turned off light, locked door, and silenced their phone
        yield return EnterRoomEvents();
        yield return new WaitForSecondsRealtime(1);

        // Displays the info board telling the user to hide in the back and moves the guide to the back
        //infoBoardHandler.NextBoard();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();

        // Waits for the player to hide in the back of the room
        triggerHandler.NextTrigger();
        yield return triggerHandler.WaitForPlayer();

        // Instructs the player to wait for ten seconds, then instructs the player to leave
        //infoBoardHandler.NextBoard();
        yield return new WaitForSecondsRealtime(8);
        //infoBoardHandler.NextBoard();

        // Moves the guide close to the door
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();

        // Waits for the player to leave the room
        triggerHandler.NextTrigger();
        yield return triggerHandler.WaitForPlayer();

        // Moves the guide out of the room, then moves them out of the building
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();
        moveModel.NextTarget();
        yield return moveModel.WaitForFinish();

        // Waits for the player to go outside
        triggerHandler.NextTrigger();
        yield return triggerHandler.WaitForPlayer();

        // End of event
        Debug.Log("Active Shooter Preparedness Event Completed!");
    }

    // Events that happen when the player enters the first room
    private IEnumerator EnterRoomEvents() {
        //leftPhone.SetEnabled(true);
        //rightPhone.SetEnabled(true);

        //bool phoneSilenced = false;
        bool doorLocked = false;
        bool lightOff = false;

        //If any conditions are flase continue running
        while (!(doorLocked && lightOff)) {
            //phoneSilenced = phoneSilenced || leftPhone.IsSilenced() || rightPhone.IsSilenced();
            doorLocked = doorHandler.IsClosed() && doorHandler.IsLocked();
            lightOff = !lightHandler.IsLightOn();
            yield return new WaitForSecondsRealtime(.1f);

            // A dev ovveride to skip turning of the light, shutting door, and silencing phone
            if(roomEventOverride) {
                break;
            }
        }
    }
    
}
