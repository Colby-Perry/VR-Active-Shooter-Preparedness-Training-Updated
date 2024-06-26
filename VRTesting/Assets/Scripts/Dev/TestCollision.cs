using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour {

    [SerializeField] GameObject zoneObjects;
    void OnTriggerEnter(Collider other) {
        zoneObjects.SetActive(true);
    }

    void OnTriggerExit(Collider other) {
        zoneObjects.SetActive(false);
    }
}
