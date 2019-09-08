using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {

    [RequireComponent(typeof(PhotonView))]
    public class Cannon : MonoBehaviourPun {

        [SerializeField]
        CannonProperty _cannonProperty;
        
        [SerializeField]
        Transform _shootPoint;

        // upgrades to the cannon & projectile to be added to cannonProperties

        Vector3 _initVelocity;


        public void RotateCannonTo(Vector3 targetPoint) {
            if (photonView.IsMine || !PhotonNetwork.IsConnected ) {
                // rotation delay to be added here
                _initVelocity = CalculateVelocity(targetPoint, _shootPoint.position, _cannonProperty.ProjectileTravelTime);
                transform.rotation = Quaternion.LookRotation(_initVelocity);
            }
        }


        void LaunchCannonProjectile() {
            //this.LaunchCannonProjectileLocally(_cannonProperty.ShootPoint.position, _initVelocity, PhotonNetwork.LocalPlayer);
            //if (PhotonNetwork.IsConnected && photonView.IsMine) {
            //    photonView.RPC("LaunchCannonProjectileRPC", RpcTarget.Others, _cannonProperty.ShootPoint.position, _initVelocity);
            //}
        }

        [PunRPC]
        void LaunchCannonProjectileRPC(Vector3 shootPointPosition, Vector3 initVelocity, PhotonMessageInfo info) {
            LaunchCannonProjectileLocally(shootPointPosition, initVelocity, info.Sender);
        }

        void LaunchCannonProjectileLocally(Vector3 shootPointPosition, Vector3 initVelocity, Photon.Realtime.Player player) {
            //GameObject GO = Instantiate(_cannonProperty.ProjectilePrefab, shootPointPosition, Quaternion.identity);
            //Rigidbody cannonProjectileRB = GO.GetComponent<Rigidbody>();
            //if (cannonProjectileRB != null) {
            //    cannonProjectileRB.velocity = initVelocity;
            //}
            //ColorChanger colorChanger = GO.GetComponent<ColorChanger>();
            //if (colorChanger != null) {
            //    colorChanger.ChangeObjectColorToPlayerColor(player);
            //}
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