using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectile : MonoBehaviour {

    [SerializeField]
    Rigidbody _canonProjectilePrefab;

    [SerializeField]
    Transform _shootPoint;

    [SerializeField]
    LayerMask _layerMask;

    [SerializeField]
    float _canonProjectileTravelTime = 1f;

    Camera _camera;
    Vector3 _initVelocity;

    void Start() {
        _camera = Camera.main;
    }

    void Update() {
        this.AimAtCursor();

        if (Input.GetMouseButtonDown(0)) {
            this.LaunchCanonProjectile();
        }
    }

    void AimAtCursor() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _layerMask)) {
            _initVelocity = CalculateVelocity(hitInfo.point, _shootPoint.position, _canonProjectileTravelTime);
            transform.rotation = Quaternion.LookRotation(_initVelocity);
        } 
    }

    void LaunchCanonProjectile() {
        Rigidbody canonProjectileGO = Instantiate(_canonProjectilePrefab, _shootPoint.position, Quaternion.identity);
        canonProjectileGO.velocity = _initVelocity;
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time) {

        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxy = distanceXZ.magnitude;

        float Vxz = Sxy / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


}
