using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SummonMagicCube : MonoBehaviour
{
    public GameObject cube;
    public GameObject hand;
    public InputActionProperty grabAction;

    private Boolean timerActive = false;
    private Boolean unGrabbed = false;
    private Boolean cubeSummoned = false;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        float grabAmount = grabAction.action.ReadValue<float>();

        if (!timerActive && grabAmount >= .9f) {
            StartCoroutine(SummonTimer());
            timerActive = true;
        }

        if (timerActive && grabAmount < .9f) {
            unGrabbed = true;
        }

        if(timerActive && unGrabbed && grabAmount >= .9f) {
            timerActive = false;
            unGrabbed = false;
            SummonCube();
        }


    }
    private IEnumerator SummonTimer() {
        yield return new WaitForSeconds(1.2f);
        if (cubeSummoned) yield break;
        timerActive = false;
        unGrabbed = false;
    }

    private void SummonCube() {
        GameObject spawnedCube = Instantiate(cube, this.transform);
        spawnedCube.transform.position = hand.transform.position + new Vector3(0, 0, -5);
        Destroy(spawnedCube, 40);
    }
}
