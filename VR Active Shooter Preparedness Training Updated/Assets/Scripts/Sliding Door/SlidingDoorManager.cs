using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class SlidingDoorManager : MonoBehaviour {

    // References to various objects in the scene
    [SerializeField] GameObject handleModel;
    [SerializeField] GameObject temp;
    [SerializeField] GameObject lockModel;

    [SerializeField] Renderer handle0;
    [SerializeField] Renderer handle1;
    [SerializeField] Renderer handleBase;

    [SerializeField] Material unlockedMaterial;
    [SerializeField] Material lockedMaterial;


    TargetHandler targetHandler;
    HandleController handleController;

    bool doorOpen = false;
    bool currentlyOperating = false;


    private void Start() {
        targetHandler = handleModel.GetComponent<TargetHandler>();
        handleController = handleModel.GetComponent<HandleController>();
    }

    private void Update() {

        SetLock();

        if (!targetHandler.IsTargetGrabbed()) {
            return;
        }

        if (handleController.IsLocked() && !doorOpen) {
            return;
        }

        if (currentlyOperating) {
            return;
        }

        StartCoroutine(MoveDoor());

        //temp.transform.Translate(-0.002f, 0, 0);
    }
    public bool IsLocked() {
        return handleController.IsLocked();
    }

    public bool IsClosed() {
        return !doorOpen;
    }

    IEnumerator MoveDoor() {

        currentlyOperating = true;

        int moveDirection;

        if(doorOpen) {
            moveDirection = 1;
        }else {
            moveDirection = -1;
        }

        for (int i = 0; i < 100; i++) {
            transform.Translate(0.01f * moveDirection, 0, 0);
            yield return null;
        }

        doorOpen = !doorOpen;
        currentlyOperating = false;
    }

    private void SetLock() {
        
        if(handleController.IsLocked() && !lockModel.activeSelf) {
            lockModel.SetActive(true);
            handle0.material = lockedMaterial;
            handle1.material = lockedMaterial;
            handleBase.material = lockedMaterial;
            return;
        }

        if (!handleController.IsLocked() && lockModel.activeSelf) {
            lockModel.SetActive(false);
            handle0.material = unlockedMaterial;
            handle1.material = unlockedMaterial;
            handleBase.material = unlockedMaterial;
            return;
        }
    }


}
