using Photon.Pun;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField]
    float _lifetime = 5.0f;

    void Awake() {
        if (PhotonNetwork.IsConnected) {
            Invoke("DestroyNetworkObject", _lifetime);
        } else {
            Destroy(gameObject, _lifetime);
        }
    }

    void DestroyNetworkObject() {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
