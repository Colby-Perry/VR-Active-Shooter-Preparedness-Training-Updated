using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GazeChangeColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(TurnBlue);
        interactable.selectExited.AddListener(TurnRed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnBlue(SelectEnterEventArgs arg) {
        Debug.Log("Testing");
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
    void TurnRed(SelectExitEventArgs arg) {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

}
