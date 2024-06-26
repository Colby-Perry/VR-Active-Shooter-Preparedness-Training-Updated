using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhoneSummoner : MonoBehaviour
{
    public GameObject phonePrefab;
    public Transform hook; // Used as a refrence to the player that already has the x and z offsets for the pocket
    public GameObject phoneSummon; // Location that the phone will be summoned

    private PhoneHandler phoneHandler; // Connected to the phonePrefab, used for XR Grab information
    TagCollideDetector collisionHandler; // Attached to phoneSummon, detects if collides with players hand

    private bool enablePhoneSummoning = true;
    float yOffset = -.9f; // Offsets phone summon from hook

    // Start is called before the first frame update
    void Start() {
        collisionHandler = phoneSummon.GetComponent<TagCollideDetector>();
    }

    // Update is called once per frame
    void Update() {
        UpdateSummonBoxPosition();
        if (enablePhoneSummoning) {
            TrySummonPhone();
        }
    }

    public void SetEnabled(bool state) {
        enablePhoneSummoning = state;
    }

    public bool IsSilenced() {
        if(phoneHandler == null) {
            return false;
        }

        return phoneHandler.IsSilenced();
    }

    // Move the summon boxes to the players pocket
    // Uses a 'hook' near the head to get where the x and z of the pocket should be
    // Uses an offset to move the boxes lower to the pockets
    private void UpdateSummonBoxPosition() {
        Vector3 newLeftPosition = new Vector3(hook.position.x, hook.position.y + yOffset, hook.position.z);
        phoneSummon.transform.position = newLeftPosition;
    }

    // If the player enters
    private void TrySummonPhone() {
        if(!collisionHandler.IsColliding()) {
            return;
        }

        // Instantiates the phone at the players pocket
        GameObject phone = Instantiate(phonePrefab, phoneSummon.transform);
        phone.transform.position = phoneSummon.transform.position;

        phoneHandler = phone.GetComponent<PhoneHandler>();
        // The phone class PhoneHandler needs some information, so we do a custom instantiation
        phoneHandler.CustomInit(phoneSummon.transform);
        enablePhoneSummoning = false;
        
    }
}
