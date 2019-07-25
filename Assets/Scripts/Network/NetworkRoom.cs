using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkRoom : MonoBehaviourPunCallbacks {

    bool _inRoom = false;
    bool _readyToStartGame = false;
    bool _cancelWaitingForPlayers = false;
    byte _gameSceneIndex = 1;

    [SerializeField]
    GameObject _WaitingForPlayersPanel;

    [SerializeField]
    Text _playerCountText;

    [SerializeField]
    Button _cancelButton;

    private void Update() {
        if (_cancelWaitingForPlayers)
            StopCoroutine("CheckForPlayersInRoom");
        if (_readyToStartGame) {
            Debug.Log("READY TO START GAME!!");
            StopCoroutine("CheckForPlayersInRoom");
            this.LoadGameScene();
        }
    }

    void DisplayWaitingForPlayers() {
        Debug.Log("DisplayWaitingForPlayers()");
        if (_inRoom) {
            _WaitingForPlayersPanel.SetActive(true);
            _playerCountText.text = string.Format("( {0} / {1} )", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
        }
    }

    void LoadGameScene() {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(_gameSceneIndex);
    }

    void OnCancelButtonPressed() {
        // TODO: Implement cancel button to call this function
        _cancelWaitingForPlayers = true;
        PhotonNetwork.LeaveRoom();
    }

    IEnumerator CheckForPlayersInRoom() {
        while (!_readyToStartGame || !_cancelWaitingForPlayers) {
            if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers) {
                DisplayWaitingForPlayers();
            } else {
                _readyToStartGame = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        this._inRoom = true;
        Debug.Log("NETWORK ROOM: OnJoinedRoom()");

        StartCoroutine("CheckForPlayersInRoom");
    }
}
