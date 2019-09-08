using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    [RequireComponent(typeof(PhotonView))]
    public class CannonProjectileController : MonoBehaviourPun {

        [SerializeField]
        GameObject _cannonProjectilePrefab;

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
            if (gameManager.GetGameMode() == GameMode.ATTACK && (photonView.IsMine || !PhotonNetwork.IsConnected)) {
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
            this.LaunchCannonProjectileLocally(_shootPoint.position, _initVelocity, PhotonNetwork.LocalPlayer);
            if (PhotonNetwork.IsConnected && photonView.IsMine) {
                photonView.RPC("LaunchCannonProjectileRPC", RpcTarget.Others, _shootPoint.position, _initVelocity);
            }
        }

        [PunRPC]
        void LaunchCannonProjectileRPC(Vector3 shootPointPosition, Vector3 initVelocity, PhotonMessageInfo info) {
            LaunchCannonProjectileLocally(shootPointPosition, initVelocity, info.Sender);
        }

        void LaunchCannonProjectileLocally(Vector3 shootPointPosition, Vector3 initVelocity, Photon.Realtime.Player player) {
            GameObject GO = Instantiate(_cannonProjectilePrefab, shootPointPosition, Quaternion.identity);
            Rigidbody cannonProjectileRB = GO.GetComponent<Rigidbody>();
            if (cannonProjectileRB != null) {
                cannonProjectileRB.velocity = initVelocity;
            }
            ColorChanger colorChanger = GO.GetComponent<ColorChanger>();
            if (colorChanger != null) {
                colorChanger.ChangeObjectColorToPlayerColor(player);
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