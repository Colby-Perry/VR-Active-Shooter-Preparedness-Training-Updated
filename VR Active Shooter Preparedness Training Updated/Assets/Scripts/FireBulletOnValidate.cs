using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnValidate : MonoBehaviour {
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 500;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable;
    // Start is called before the first frame update
    void Start() {
        grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void FireBullet(ActivateEventArgs arg) {
        Debug.Log("Bullet Fired");
        gameObject.GetComponent<AudioSource>().Play(); // Plays the gunshot sound

        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = fireSpeed * -spawnPoint.forward;
        Destroy(spawnedBullet, 5);
    }
}
