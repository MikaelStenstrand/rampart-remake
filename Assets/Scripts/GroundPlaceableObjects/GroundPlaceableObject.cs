using Photon.Pun;
using UnityEngine;

public class GroundPlaceableObject : MonoBehaviour {

    [SerializeField]
    PlayerSettings _playerSettings;

    PhotonView _photonView;

    void Awake() {
        _photonView = gameObject.GetComponent<PhotonView>();
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

    
}
