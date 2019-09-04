using UnityEngine;

namespace Rampart.Remake { 
    public class DestroyWall : MonoBehaviour {

        [SerializeField] string collisionTag = "";

        void OnTriggerEnter(Collider other) {
            if (other.tag == collisionTag) {
                Debug.Log("Trigger " + other.tag);
                this.DestroyWallPiece();
            }
        }

        void DestroyWallPiece() {
            Destroy(this.gameObject);
        }

    }
}