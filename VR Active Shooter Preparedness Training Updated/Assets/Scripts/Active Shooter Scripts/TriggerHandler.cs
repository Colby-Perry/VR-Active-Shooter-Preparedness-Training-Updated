using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class TriggerHandler : MonoBehaviour
{
    public GameObject playerTriggers;
    private GameObject currentTrigger;
    private int triggerCount = 0;

    // Waits for player to collide with the current trigger
    public IEnumerator WaitForPlayer() {
        if (currentTrigger == null) {
            Debug.LogError("WaitForPlayer in TriggerHandler will never run as there is no currentTrigger Selected\n" +
                "NextTrigger must be ran before WaitForPlayer");
        }

        // Waits for player to collide with the current trigger
        TagCollideDetector currentDetector = currentTrigger.GetComponent<TagCollideDetector>();
        while (!currentDetector.IsColliding()) {
            yield return new WaitForFixedUpdate();
        }
    }

    // Sets the next trigger to the next child of the playerTriggers gameobject
    public void NextTrigger() {
        try {
            currentTrigger = playerTriggers.transform.GetChild(triggerCount++).gameObject;
        } catch (UnityException ex) {
            Debug.LogError("Next player trigger does not exist: " + ex.Message);
        }
    }
}
