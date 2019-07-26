using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkRoom : MonoBehaviourPunCallbacks {

    bool _inRoom = false;
    bool _readyToStartGame = false;
    bool _cancelWaitingForPlayers = false;
    byte _gameSceneIndex = 1;

    [SerializeField]
    GameObject _waitingForPlayersPanel;

    [SerializeField]
    Text _playerCountText;

    [SerializeField]
    GameObject _cancelButton;

    private void Start() {
        _waitingForPlayersPanel.SetActive(false);
        _cancelButton.SetActive(false);
    }

    private void Update() {
        if (_cancelWaitingForPlayers) {
            StopCoroutine("CheckForPlayersInRoom");
        }
        if (_readyToStartGame && _inRoom) {
            Debug.Log("READY TO START GAME!!");
            StopCoroutine("CheckForPlayersInRoom");
            this.LoadGameScene();
        }
    }

    void DisplayWaitingForPlayers() {
        Debug.Log("DisplayWaitingForPlayers()");
        if (_inRoom) {
            _waitingForPlayersPanel.SetActive(true);
            _cancelButton.SetActive(true);
            _playerCountText.text = string.Format("( {0} / {1} )", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
        }
    }

    void LoadGameScene() {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(_gameSceneIndex);
    }

    void LeaveRoom() {
        _cancelWaitingForPlayers = true;
        _inRoom = false;
        _readyToStartGame = false;

        PhotonNetwork.LeaveRoom();
        _waitingForPlayersPanel.SetActive(false);
        _cancelButton.SetActive(false);
    }

    public void OnCancelButtonPressed() {
        this.LeaveRoom();
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
