using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    public class WallDestroyController : MonoBehaviourPun {

        public void DestroyWallPiece(int childIndex) {
            if (IsWallCompletelyDestroyed()) {
               photonView.RPC("DestroyWholeWallObject", RpcTarget.All);
            } else {
                DestroyChildGO(childIndex);
                photonView.RPC("DestroyWallPieceRPC", RpcTarget.Others, childIndex);
            }
        }

        void DestroyChildGO(int childIndex) {
            Destroy(gameObject.transform.GetChild(0).GetChild(1).GetChild(childIndex).gameObject);
        }

        bool IsWallCompletelyDestroyed() {
            int wallPiecesCount = gameObject.transform.GetChild(0).GetChild(1).childCount;
            if (wallPiecesCount <= 1) { // called before destroying last wall pieces, i.e. wall count = 1
                return true;
            }
            return false;
        }

        [PunRPC]
        void DestroyWallPieceRPC(int childIndex) {
            DestroyChildGO(childIndex);
        }

        [PunRPC]
        void DestroyWholeWallObject() {
            if (photonView.IsMine) {
                PhotonNetwork.Destroy(gameObject);
            }
        }

    }
}