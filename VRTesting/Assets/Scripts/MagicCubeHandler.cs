using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    public GameObject hand;
    private Vector3 handSpeed = Vector3.zero;
    private Vector3 lastHandLocation;

    // Start is called before the first frame update
    void Start()
    {
        hand = transform.parent.gameObject.GetComponent<SummonMagicCube>().hand;
        lastHandLocation = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 handLocation = hand.transform.position;
        handSpeed = handLocation - lastHandLocation;

        if(handSpeed.x > 0.01 || handSpeed.y > 0.01 || handSpeed.z > 0.01 ||
            handSpeed.x < -0.01 || handSpeed.y < -0.01 || handSpeed.z < -0.01) {
            GetComponent<Rigidbody>().velocity = handSpeed * 1000;
        }

        lastHandLocation = handLocation;
    }
}
