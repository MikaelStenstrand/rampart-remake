using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {
    public class GroundPlaceableObject : MonoBehaviourPun {

        [SerializeField]
        PlayerSettings _playerSettings;

        public string Filename;

        Color _defaultColor;

        [HideInInspector]
        public bool isPlaceable = true;
        static string Tag = "GroundPlaceableObject";

        void Awake() {
            _defaultColor = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>()[0].material.color;
        }

        /*
         * Expects the Game Object to have the following sturcutre
         * GAME OBJECT
         * - objects
         * -- ColorModifiedObjects (objects' color within this group will be changed)
         * -- etc.
         * -- etc.
         */
        public void ChangeObjectColorToLocalPlayerColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                Color color = (Color)_playerSettings.GetLocalPlayerColor(); // TODO: change before foreach
                renderer.material.color = color;
            }
        }

        public void ChangeObjectColorToPlayerColor(Photon.Realtime.Player player) {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
            Color color = _playerSettings.GetPlayerColor(player);
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = color;
            }
        }

        public void ChangeObjectColorToPlayerColorOverNetwork() {
            if (PhotonNetwork.IsConnected)
                photonView.RPC("RPCChangeObjectColorToPlayerColor", RpcTarget.All);
        }

        [PunRPC]
        void RPCChangeObjectColorToPlayerColor(PhotonMessageInfo info) {
            Color color = _playerSettings.GetPlayerColor(info.Sender);
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = color;
            }
        }

        void ChangeToNotPlaceableColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = Color.red;
            }
        }

        void ChangeToDefaultColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = _defaultColor;
            }
        }

        void GOIsNotPlaceable() {
            isPlaceable = false;
            ChangeToNotPlaceableColor();
        }

        void GOIsPlaceable() {
            isPlaceable = true;
            ChangeToDefaultColor();
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == Tag)
                this.GOIsNotPlaceable();
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == Tag)
                this.GOIsPlaceable();
        }
    }
}