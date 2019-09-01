using Photon.Pun;
using UnityEngine;

    namespace Rampart.Remake { 

    public class CannonProjectile : MonoBehaviour {

        [SerializeField]
        Rigidbody _cannonProjectilePrefab;

        [SerializeField]
        Transform _shootPoint;

        [SerializeField]
        LayerMask _layerMask;

        [SerializeField]
        float _cannonProjectileTravelTime = 1f;

        Camera _camera;
        Vector3 _initVelocity;
        GameManager gameManager;

        void Start() {
            _camera = Camera.main;
            gameManager = GameManager.instance;
        }

        void Update() {
            if (gameManager.GetGameMode() == GameMode.ATTACK) {
                this.AimAtCursor();

                if (Input.GetMouseButtonDown(0)) {
                    this.LaunchCannonProjectile();
                }
            }

        }

        void AimAtCursor() {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _layerMask)) {
                _initVelocity = CalculateVelocity(hitInfo.point, _shootPoint.position, _cannonProjectileTravelTime);
                transform.rotation = Quaternion.LookRotation(_initVelocity);
            } 
        }

        void LaunchCannonProjectile() {
            GameObject cannonProjectileGO = PhotonNetwork.Instantiate("Prefabs/GroundPlaceableObjects/Cannon/CannonProjectile", _shootPoint.position, Quaternion.identity);
            Rigidbody cannonProjectileRB = cannonProjectileGO.GetComponent<Rigidbody>();
            cannonProjectileRB.velocity = _initVelocity;
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
}