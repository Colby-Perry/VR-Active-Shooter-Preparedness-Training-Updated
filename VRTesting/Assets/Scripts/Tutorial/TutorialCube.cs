using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialCube : MonoBehaviour
{
    Vector3 initialPos;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable;
    bool colorChanged = false;

    Renderer rend;
    int colorIndex = 0;
    Color[] rainbowColors = {
        new Color(1.0f, 0.65f, 0.0f), // Orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.29f, 0.0f, 0.51f), // Indigo
        new Color(0.55f, 0.0f, 1.0f), // Violet
        Color.red
    };

    public bool HasColorChanged() {
        return colorChanged;
    }

    public void CustomStart() {
        initialPos = transform.position;
        rend = GetComponent<Renderer>();

        grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(ChangeColor); // Calls ChangeColor() whenever this object is activated (trigger button pressed)
    }

    // When called cycle to the next color of the rainbow
    void ChangeColor(ActivateEventArgs arg) {
        colorIndex = (colorIndex + 1) % rainbowColors.Length;
        rend.material.color = rainbowColors[colorIndex];
        colorChanged = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Object")) {
            transform.position = initialPos;
        }
    }




}
