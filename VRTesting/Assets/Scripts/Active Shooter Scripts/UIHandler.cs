using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] GameObject canvas;
    [SerializeField] Transform mainCamera;
    [SerializeField] float uiDistance = 1f;

    //private CanvasScaler canvasScaler;

    public void ChangeText(string str) {
        mainText.text = str;

        //canvasScaler = canvas.GetComponent<CanvasScaler>();
    }

    public void AddText(string str) {
        mainText.text += str;
    }

    public void ClearText() {
        mainText.text = "";
    }

    public void FancyChangeText(string str) {
        ClearText();
        StartCoroutine(TextCoroutine(str));
    }

    private IEnumerator TextCoroutine(string str) {

        foreach (char character in str) {
            AddText(character.ToString());
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public void EnableUI() {
        canvas.SetActive(true);

        // Teleport the object in front of the camera
        Vector3 newPosition = mainCamera.position + mainCamera.forward * uiDistance;
        canvas.transform.position = newPosition;

        // Apply instant rotation to face the camera
        Vector3 targetDirection = mainCamera.position - canvas.transform.position;
        canvas.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        canvas.transform.Rotate(0f, 180f, 0f);
    }

    public void DisableUI() {
        canvas.SetActive(false);
    }


}
