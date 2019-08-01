using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour {

    [SerializeField]
    LayerMask _layerMask;

    [SerializeField]
    float _cursorUpModifier = 0.1f;

    Camera _camera;

    void Start() {
        _camera = Camera.main;
    }

    void Update() {
        AimSpirteAtCursor();
    }

    void AimSpirteAtCursor() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _layerMask)) {
            gameObject.SetActive(true);
            gameObject.transform.position = hitInfo.point + Vector3.up * _cursorUpModifier;
        } else {
            gameObject.SetActive(false);
        }
    }
}
