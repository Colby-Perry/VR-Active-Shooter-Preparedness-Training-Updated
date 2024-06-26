using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhoneHandler : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable {

    private bool initialized = false;
    private bool grabbed = false;
    private bool isSilenced = false;

    private Transform spawnPoint;

    public void CustomInit(Transform spawnPoint) {
        this.spawnPoint = spawnPoint;
        initialized = true;
    }
    // Start is called before the first frame update
    void Start() {


        
    }

    // Update is called once per frame
    void Update() {
        if (!initialized) {
            return;
        }

        if (!grabbed) {
            transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            transform.rotation = Quaternion.identity;
            return;
        }
        
    }

    public bool IsSilenced() {
        return isSilenced;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args) {
        base.OnSelectEntered(args);
        grabbed = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args) {
        base.OnSelectExited(args);
        grabbed = false;
    }

    protected override void OnActivated(ActivateEventArgs args) {
        base.OnActivated(args);

        transform.GetChild(0).gameObject.SetActive(true);
        isSilenced = true;
    }


}
