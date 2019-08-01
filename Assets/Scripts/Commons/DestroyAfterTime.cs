using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField]
    float _lifetime = 5.0f;

    void Awake() {
        Destroy(gameObject, _lifetime);
    }
}
