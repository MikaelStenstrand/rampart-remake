using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {

    [RequireComponent(typeof(PhotonView))]
    public class Cannon : MonoBehaviourPun {

        [SerializeField]
        CannonProperty _cannonProperty;
        
        [SerializeField]
        Transform _shootPoint;

        Vector3 _initVelocity;
        float _nextTimeToFire = 0.0f;


        public void RotateCannonTo(Vector3 targetPoint) {
            if (photonView.IsMine || !PhotonNetwork.IsConnected ) {
                _initVelocity = CalculateVelocity(targetPoint, _shootPoint.position, _cannonProperty.ProjectileTravelTime);
                transform.rotation = Quaternion.LookRotation(_initVelocity);
            }
        }

        void LaunchCannonProjectile() {
            this.LaunchCannonProjectileLocally(_shootPoint.position, _initVelocity, PhotonNetwork.LocalPlayer);
            photonView.RPC("LaunchCannonProjectileRPC", RpcTarget.Others, _shootPoint.position, _initVelocity);
        }

        void LaunchCannonProjectileLocally(Vector3 shootPointPosition, Vector3 initVelocity, Photon.Realtime.Player player) {
            GameObject GO = Instantiate(_cannonProperty.ProjectilePrefab, shootPointPosition, Quaternion.identity);
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

        [PunRPC]
        void LaunchCannonProjectileRPC(Vector3 shootPointPosition, Vector3 initVelocity, PhotonMessageInfo info) {
            this.LaunchCannonProjectileLocally(shootPointPosition, initVelocity, info.Sender);
        }

        public bool IsAvailableToFire() {
            if (Time.time >= _nextTimeToFire) {
                return true;
            }
            return false;
        }

        public void Fire() {
            if (PhotonNetwork.IsConnected) {
                if (photonView.IsMine) {
                    _nextTimeToFire = Time.time + _cannonProperty.CoolDown;
                    this.LaunchCannonProjectile();
                }
            } else {
                // Offline
                _nextTimeToFire = Time.time + _cannonProperty.CoolDown;
                this.LaunchCannonProjectileLocally(_shootPoint.position, _initVelocity, null);
            }
        }

    }
}