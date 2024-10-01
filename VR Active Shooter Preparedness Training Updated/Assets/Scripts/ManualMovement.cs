using UnityEngine;

public class ManualMovement : MonoBehaviour {

    public GameObject theNPC;
    private Animator animator;

    public bool isWalking;
    public bool isRunning;
    public bool isSuperRunning;

    private void Start() {
        animator = theNPC.GetComponent<Animator>();
    }

    void Update() {

        if (Input.GetKey(KeyCode.Alpha1)) {
            isWalking = true;
            Debug.Log("HelloWorld");
        } else {
            isWalking = false;
        }

        if (Input.GetKey(KeyCode.Alpha2)) {
            isRunning = true;
        } else {
            isRunning = false;
        }

        if (Input.GetKey(KeyCode.Alpha3)) {
            isSuperRunning = true;
        } else {
            isSuperRunning = false;
        }

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isSuperRunning", isSuperRunning);

    }
}
