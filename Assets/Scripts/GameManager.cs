using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RampartRemake {


    public class GameManager : MonoBehaviourPunCallbacks {

        #region Singelton
        public static GameManager instance;

        void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Debug.LogWarning("More than one instance of GameManager exists!");
                return;
            }
        }
        #endregion Singelton

        void Start() {
            
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                LeaveRoom();
            }
        }

        void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer) {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.LogFormat("OnPlayerEnteredRoom: Player entered: {0}", newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.LogFormat("OnPlayerLeftRoom: Player left: {0}", otherPlayer);
        }


        public override void OnLeftRoom() {
            base.OnLeftRoom();
            Debug.Log("NETWORK: OnLeftRoom()");
            SceneManager.LoadScene(0);
        }

        public void OnLeaveGameButtonPressed() {
            Debug.Log("OnLeaveGameButtonPressed");
            this.LeaveRoom();
        }
    }

}