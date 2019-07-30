using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Rampart.Remake {

    public class NetworkManager : MonoBehaviourPunCallbacks {

        [SerializeField]
        byte _maxPlayersPerRoom = 2;

        [SerializeField]
        GameObject _controlPanel;

        [SerializeField]
        GameObject _progressLabel;

        [Tooltip("Play in Online or Offline mode")]
        [SerializeField]
        bool _onlineMode = true;

        bool _isConnected;

        void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;

            if (!_onlineMode)
                PhotonNetwork.OfflineMode = true;
        }

        void Start() {
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
            _isConnected = PhotonNetwork.IsConnected;
            ConnectToMaster();
        }

        void ConnectToMaster() {
            _progressLabel.SetActive(true);
            _controlPanel.SetActive(false);

            if (!PhotonNetwork.IsConnected) {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        void CreateRoom() {
            if (!_isConnected)
                return;
            PhotonNetwork.CreateRoom(null, new RoomOptions {
                MaxPlayers = _maxPlayersPerRoom,
                IsVisible = true,
                IsOpen = true,
            });
        }

        void JoinGame() {
            if (!_isConnected)
                return;
            _progressLabel.SetActive(true);
            _controlPanel.SetActive(false);
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
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
            _isConnected = true;
        }

        public override void OnDisconnected(DisconnectCause cause) {
            base.OnDisconnected(cause);
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(true);
            _isConnected = false;
            Debug.LogWarningFormat("NETWORK: OnDisconnected(): cause {0}", cause);
        }

        public override void OnJoinedRoom() {
            // move to UI handler
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(false);
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.LogFormat("NETWORK: OnCreatedRoomFailed(): code: {0}, message {1}", returnCode, message);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.LogFormat("NETWORK: OnJoinRandomFailed(): code: {0}, message {1}", returnCode, message);
            if (returnCode == 32760 && _isConnected) {  // 32760 = No room found
                this.CreateRoom();
            }
        }

        // used only for debugging purposes. Only called from the Debug Button in Lobby Scene
        public void DebugPlayAloneGame() {
            _progressLabel.SetActive(false);
            _controlPanel.SetActive(false);

            PhotonNetwork.CreateRoom("testRoom", new RoomOptions {
                MaxPlayers = 1,
                IsVisible = true,
                IsOpen = true,
            });
        }
    }
}
