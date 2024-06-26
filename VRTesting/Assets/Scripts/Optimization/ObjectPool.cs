using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    GameObject[] objectArray;
    Stack<GameObject> myStack;
    void Start() {
        objectArray = MyInitializer().ToArray();
        foreach (GameObject obj in objectArray) {
            obj.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            foreach (GameObject obj in objectArray) {
                obj.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            foreach (GameObject obj in objectArray) {
                obj.SetActive(true);
            }
        }
    }
    
    public Stack<GameObject> MyInitializer() {
        myStack = new Stack<GameObject>();
        Transform parent = transform.parent;
        RecursiveScriptSearch(parent);


        return myStack;
    }

    private bool RecursiveScriptSearch(Transform myTransform) {

        bool unload = (myTransform.GetComponent<NoUnload>() == null);

        if (myTransform.childCount == 0) {
            if (unload) myStack.Push(myTransform.gameObject);
            return unload;
        }

        if (myTransform.childCount > 0) {
            foreach (Transform child in myTransform) {
                bool childUnload = RecursiveScriptSearch(child);
                unload = (unload && childUnload);
            }
        }

        if (unload) {
            myStack.Push(myTransform.gameObject);
        }

        return unload;
    }

    /*
    private bool RecursiveScriptSearch(Transform parent) {

        bool isNoUnload = false;

        if (transform.childCount == 0) {
            isNoUnload = isNoUnload || false;
        }

        foreach (Transform child in parent) {
            NoUnload noUnload = child.GetComponent<NoUnload>();
            if (noUnload == null) {
                Debug.Log("Adding" + child.name + " from " + parent.name);
                isNoUnload = (isNoUnload || RecursiveScriptSearch(child));

                if(isNoUnload) {
                    return isNoUnload;
                }
            }
        }

        myStack.Push(parent.gameObject);
        return isNoUnload;
    } */


}
