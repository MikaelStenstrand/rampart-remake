using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    [RequireComponent(typeof(PhotonView))]
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
        PhotonView _photonView;

        void Start() {
            _camera = Camera.main;
            gameManager = GameManager.instance;
            _photonView = gameObject.GetComponent<PhotonView>();
        }

        void Update() {
            if (gameManager.GetGameMode() == GameMode.ATTACK && _photonView.IsMine) {
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

        /*
         * Instantiate projectile over the network
         */
        void LaunchCannonProjectile() {
            GameObject GO = PhotonNetwork.Instantiate("Prefabs/GroundPlaceableObjects/Cannon/CannonProjectile", _shootPoint.position, Quaternion.identity);
            Rigidbody cannonProjectileRB = GO.GetComponent<Rigidbody>();
            GroundPlaceableObject cannonProjectileGPO = GO.GetComponent<GroundPlaceableObject>();
            if (cannonProjectileRB != null) {
                cannonProjectileRB.velocity = _initVelocity;
            }
            if (cannonProjectileGPO) {
                cannonProjectileGPO.ChangeObjectColorToPlayerColorOverNetwork();
            }
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