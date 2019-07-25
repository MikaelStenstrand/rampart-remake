﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class NetworkManager : MonoBehaviourPunCallbacks {

    [SerializeField]
    private byte maxPlayersPerRoom = 2;

    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private GameObject progressLabel;


    void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start() {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        ConnectToMaster();
    }

    void ConnectToMaster() {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    void CreateRoom() {
        PhotonNetwork.CreateRoom(null, new RoomOptions {
            MaxPlayers = maxPlayersPerRoom
        });
    }

    void JoinGame() {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected) {
            Debug.Log("JoinGame");
            PhotonNetwork.JoinRandomRoom();
        } else {
            this.ConnectToMaster();
        }
    }

    public void OnJoinGameButtonPressed() {
        Debug.Log("OnJoinGameButtonPressed");
        this.JoinGame();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.Log("NETWORK: Connected to master");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("NETWORK: OnDisconnected(): cause {0}", cause);
    }
    public override void OnLeftRoom() {
        base.OnLeftRoom();
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.Log("NETWORK: OnLeftRoom()");
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("NETWORK: OnJoinedRoom()");
        progressLabel.SetActive(false);
        controlPanel.SetActive(false);
        // TODO: load scene
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogFormat("NETWORK: OnCreatedRoomFailed(): code: {0}, message {1}", returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        //base.OnJoinRandomFailed(returnCode, message);
        Debug.LogFormat("NETWORK: OnJoinRandomFailed(): code: {0}, message {1}", returnCode, message);
        if (returnCode == 32760) {  // 32760 = No room found
            this.CreateRoom();
        }
    }

}