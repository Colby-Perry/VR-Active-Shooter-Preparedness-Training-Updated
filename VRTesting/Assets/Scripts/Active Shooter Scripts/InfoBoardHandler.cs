using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBoardHandler : MonoBehaviour
{

    public GameObject infoBoards;
    private int current = -1;

    public void NextBoard() {

        // If first enable the first board
        if(current == -1) {
            infoBoards.transform.GetChild(0).gameObject.SetActive(true);
            current++;
            return;
        }

        // Disable current board then enable the next one
        infoBoards.transform.GetChild(current).gameObject.SetActive(false);
        current++;
        infoBoards.transform.GetChild(current).gameObject.SetActive(true);
    }
}
