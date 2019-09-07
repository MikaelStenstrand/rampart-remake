using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    public class DestroyWall : MonoBehaviour {

        [SerializeField] string collisionTag = "";

        void OnTriggerEnter(Collider other) {
            if (other.tag == collisionTag) {
                Debug.Log("Trigger " + other.tag);

                if (PhotonNetwork.IsMasterClient) {
                    int index = transform.GetSiblingIndex();
                    
                    Debug.Log("INDEX OF CHILD: " + index.ToString());
                    
                    gameObject.GetComponentInParent<WallDestroyController>().DestroyWallPiece(index);
                }


            }
        }


    }
}