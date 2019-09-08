using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {

    [RequireComponent(typeof(ColorChanger))]
    public class GroundPlaceableObject : MonoBehaviourPun {

        public string Filename;
        [HideInInspector]
        public bool isPlaceable = true;

        ColorChanger _colorChanger;
        static string Tag = "GroundPlaceableObject";

        void Awake() {
            _colorChanger = gameObject.GetComponent<ColorChanger>();
        }

        public void ChangeObjectColorToPlayerColorOverNetwork() {
            if (PhotonNetwork.IsConnected)
                photonView.RPC("RPCChangeObjectColorToPlayerColor", RpcTarget.All);
        }

        public void ChangeObjectColorToLocalPlayerColor() {
            if (_colorChanger != null) {
                _colorChanger.ChangeObjectColorToLocalPlayerColor();
            }
        }

        void GOIsNotPlaceable() {
            isPlaceable = false;
            _colorChanger.ChangeToNotPlaceableColor();
        }

        void GOIsPlaceable() {
            isPlaceable = true;
            _colorChanger.ChangeToDefaultColor();
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == Tag)
                this.GOIsNotPlaceable();
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == Tag)
                this.GOIsPlaceable();
        }

        [PunRPC]
        void RPCChangeObjectColorToPlayerColor(PhotonMessageInfo info) {
            _colorChanger.ChangeObjectColorToPlayerColor(info.Sender);
        }
    }
}