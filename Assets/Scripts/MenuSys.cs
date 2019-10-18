using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSys : MonoBehaviour {

    public void PlayAgain() {

        Application.LoadLevel("Level");
    }
    public void LaunchGameMenu() {
        Application.LoadLevel("GameMenu");
    }
}
