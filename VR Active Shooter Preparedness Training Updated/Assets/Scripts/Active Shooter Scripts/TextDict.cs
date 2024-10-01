using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDict : MonoBehaviour {

    [TextArea(5, 3)] public string afterGunShots = "";
    [TextArea(5, 3)] public string enteredRoom = "";

    Dictionary<string, string> promptDict;
    // Start is called before the first frame update
    void Start() {
        promptDict = new Dictionary<string, string> {
            { nameof(afterGunShots), afterGunShots },
            { nameof(enteredRoom), enteredRoom }
        };
    }

    public string GetPrompt(string promptName) {

        return promptDict[promptName];
    }
}