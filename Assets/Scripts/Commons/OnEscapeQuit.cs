using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RampartRemake { 

    public class OnEscapeQuit : MonoBehaviour {

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }

}