using System.Collections;
using Photon.Pun;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField]
    float _lifetime = 5.0f;

    void Awake() {
        if (PhotonNetwork.IsConnected) {
            StartCoroutine(DestroyNetworkObject());
        } else {
            Destroy(gameObject, _lifetime);
        }
    }

    IEnumerator DestroyNetworkObject() {
        yield return new WaitForSeconds(_lifetime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
