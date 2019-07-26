using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rampart.Remake { 

    public class OnEscapeQuit : MonoBehaviour {

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }

}