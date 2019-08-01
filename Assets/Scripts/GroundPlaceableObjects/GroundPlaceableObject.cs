using Photon.Pun;
using UnityEngine;

public class GroundPlaceableObject : MonoBehaviour {

    [SerializeField]
    PlayerSettings _playerSettings;

    PhotonView _photonView;
    Color _defaultColor;

    public bool isPlaceable = true;
    const string _groundPlaceableObjectTag = "GroundPlaceableObject";

    void Awake() {
        _photonView = gameObject.GetComponent<PhotonView>();
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
    public void ChangeObjectColorToPlayerColor() {
        Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers) {
            Color color = (Color)_playerSettings.GetLocalPlayerColor();
            renderer.material.color = color;
        }
    }

    public void ChangeObjectColorToPlayerColorOverNetwork() {
        if (PhotonNetwork.IsConnected)
            _photonView.RPC("RPCChangeObjectColorToPlayerColor", RpcTarget.All);
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
        if (other.tag == _groundPlaceableObjectTag)
            this.GOIsNotPlaceable();
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == _groundPlaceableObjectTag)
            this.GOIsPlaceable();
    }
}
