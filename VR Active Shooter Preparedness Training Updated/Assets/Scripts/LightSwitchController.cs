using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Lumin;


public class LightSwitchController : MonoBehaviour {

    //button object declaration
    public GameObject lightSource;
    public GameObject interactionArea;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simpleInteractable;

    private void Start() {
        simpleInteractable = interactionArea.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(ToggleLight);
        simpleInteractable.activated.AddListener(ToggleLight);
    }

    private void ToggleLight(System.Object args) {
        Debug.Log("Light Toggled");
        gameObject.GetComponent<AudioSource>().Play();
        lightSource.SetActive(!lightSource.activeSelf);
    }

    public bool IsLightOn() {
        return lightSource.activeSelf;
    }
}