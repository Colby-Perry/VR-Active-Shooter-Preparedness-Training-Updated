using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class SDGrabStatusTracker : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable {

    private Boolean grabbed;

    private void Start() {
        grabbed = false;
    }

    public Boolean IsGrabbed() {
        return grabbed;
    }

    //Set grabbed to true when target is grabbed
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        grabbed = true;
    }

    //Set grabbed to true when target is dropped
    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        grabbed = false;
    }
}



    /*
    public Transform handleModel;
    public Transform parent;

    private Boolean grabbed;

    private void Start() {
        grabbed = false;
    }


    //Update is called once per frame
    void FixedUpdate() {
        FollowHandle();
    }

    //Makes target follow handle
    void FollowHandle() {

        if (grabbed) {
            return;
        }

        if (Vector3.Distance(transform.position, handleModel.position) < 0.001) {
            return;
        }

        transform.position = handleModel.position;
        transform.rotation = handleModel.rotation;
    }

    //Repeatedly runs DistanceForceDrop when player grabs the handle
    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        grabbed = true;
        InvokeRepeating("DropHandleWhenOutOfRange", 0, 0.1f);
    }

    //Forces the player to drop the handle when the hand is too far from the handle model
    //Cancels its own invoke when dropped
    //**Any new functions that stops the player from grabbing the handle needs to cancel this invoke**
    private void DropHandleWhenOutOfRange() {
        float distance = Vector3.Distance(transform.position, handleModel.transform.position);

        if(distance > .5) {
            grabbed = false;
            CancelInvoke("DropHandleWhenOutOfRange");
            ResetHandle();
        }
    }

    //Whenever the handle is dropped call ResetHandle
    //Cancels DistanceForceDrop
    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);

        CancelInvoke("DistanceForceDrop");

        if (Vector3.Distance(transform.position, handleModel.transform.position) <=.5) {
            ResetHandle();
        }
        
    }

    //Resets handle to original position
    private void ResetHandle() {
        Debug.Log("Reset Handle Called");

        //Creates a clone of this gameObject
        GameObject clone = Instantiate(gameObject);
        clone.transform.SetParent(parent);
        clone.transform.position = handleModel.transform.position;
        clone.transform.rotation = handleModel.transform.rotation;
        clone.transform.localScale = transform.localScale;
        clone.transform.name = name;

        //Creates clones of variables in this class
        TargetHandler cloneScript = clone.GetComponent<TargetHandler>();
        cloneScript.handleModel = handleModel;
        cloneScript.parent = parent;

        //Resets target in the FollowPhysics script
        //handleModel.GetComponent<FollowPhysics>().target = clone.transform;

        //Destroys this gameObject
        Console.WriteLine("Target Destroyed");
        Destroy(gameObject);
    }

}*/