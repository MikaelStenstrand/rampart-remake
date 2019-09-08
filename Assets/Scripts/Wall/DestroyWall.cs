using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    public class DestroyWall : MonoBehaviour {

        [SerializeField] string collisionTag = "";
        WallDestroyController _wallDestroyController;


        void Start() {
            _wallDestroyController = GetComponentInParent<WallDestroyController>();
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == collisionTag) {
                if (PhotonNetwork.IsMasterClient) {
                    int index = transform.GetSiblingIndex();
                    if (_wallDestroyController != null) {
                        _wallDestroyController.DestroyWallPiece(index);
                    }
                }
            }
        }


    }
}