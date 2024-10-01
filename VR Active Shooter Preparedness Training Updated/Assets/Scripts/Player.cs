using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionProperty lGrab;
    [SerializeField] InputActionProperty rGrab;

    [SerializeField] InputActionProperty lTrigger;
    [SerializeField] InputActionProperty rTrigger;

    [SerializeField] InputActionProperty move;
    [SerializeField] InputActionProperty turn;

    [SerializeField] InputActionProperty lPosition;
    [SerializeField] InputActionProperty rPosition;

    Camera myCamera;
    float nearClipPlane;


    // Start is called before the first frame update
    void Start() {
        myCamera = gameObject.GetNamedChild("CameraOffset").GetNamedChild("Main Camera").GetComponent<Camera>();
        nearClipPlane = myCamera.nearClipPlane;

    }

    // Update is called once per frame
    void Update() {

    }

    public float GetLeftGrab() {
        return lGrab.action.ReadValue<float>();
    }
    public float GetRightGrab() {
        return rGrab.action.ReadValue<float>();
    }

    public float GetLeftTrigger() {
        return lTrigger.action.ReadValue<float>();
    }

    public float GetRightTrigger() {
        return rTrigger.action.ReadValue<float>();
    }

    public Vector2 GetMove() {
        return move.action.ReadValue<Vector2>();
    }

    public Vector2 GetTurn() {
        return turn.action.ReadValue<Vector2>();
    }

    public Vector3 GetLeftPosition() {
        return lPosition.action.ReadValue<Vector3>();
    }

    public Vector3 GetRightPosition() {
        return rPosition.action.ReadValue<Vector3>();
    }

    public Camera GetCamera() {
        return myCamera;
    }

    public float GetNearClipPlane() {
        return nearClipPlane;
    }
}
